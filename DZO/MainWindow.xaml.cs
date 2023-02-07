
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Utils;
using Haley.WPF;
using ProtoBuf.WellKnownTypes;

namespace DZO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        dbConn conn = new dbConn();
        bool dark = Login.is_dark;
        public static int st_zaposlenih;
        public static int st_ostarelih;
        public static int st_so;
        public static string izpis;
        public MainWindow()
        {

            InitializeComponent();
            kraj_label.Content = "Pozdravljeni izberite kraj-->";
            foreach (var x in conn.Kraji())
            {
                kraji_display.Items.Add(x);
            }

            if (dark.Equals(false))
            {
                ResourceDictionary dict = new ResourceDictionary();
                dict.Source = new Uri("Theme/Light.xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            else if (dark.Equals(true))
            {
                ResourceDictionary dict = new ResourceDictionary();
                dict.Source = new Uri("Theme/Dark.xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
        }

        private void Minimize_onClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_onClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Login log = new Login();
            log.Show();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void info_onClick(object sender, RoutedEventArgs e)
        {

        }

        private void kraji_display_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            grid.Children.Clear();
            var split_string = kraji_display.SelectedItem.ToString().Split();
            List<string> list = split_string.ToList();
            int st = list.Count() - 1;
            list.RemoveAt(st);
            string final = string.Join(" ", list);
            kraj_label.Content = final;
            int i = 0;
            var neki = conn.Lokacije(final, split_string[st]);
            foreach (var item in neki)
            {

                var splited = item.ToString().Split();
                List<string> lokacija = splited.ToList();
                st_zaposlenih = splited.Length - 1;
                st_ostarelih = splited.Length - 2;
                st_so = splited.Length - 3;
                lokacija.RemoveRange(st_so, 3);
                izpis = string.Join(" ", lokacija).ToString();
                grid.Children.Add(add_button(izpis, i, st_zaposlenih, st_ostarelih, st_so));

                i++;
            }
            Viewer.Content = grid;

        }


        Grid grid = new Grid();

        Button  add_button(string btn_content, int i, int st_zap, int st_ost, int st_sob)
        {

            Button btn = new Button();
            btn.Name = btn_content.ToString().Replace(" ", "_").Replace("-", "_");
            btn.Click += (s, e) =>
            {
                Control control = new Control();
                control.odLabel.Text = btn_content;
                control.lokacije.SelectedItem = btn_content;
                foreach (string x in conn.lokacija(btn_content))
                {
                    control.lokacije.Items.Add(x);
                }
                control.Show();
                this.Close();
            };
            btn.Margin = new Thickness(20);
            btn.BorderThickness = new Thickness(0);
            btn.Content = btn_content;
            btn.Background = (SolidColorBrush)this.FindResource("btn");
            btn.Foreground = (SolidColorBrush)this.FindResource("Foreground");
            btn.Style = (Style)this.FindResource("roundeddynamic");
            btn.Width = 800;
            btn.Height = 90;
            btn.FontSize = 30;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            RowDefinition rowDefinition = new RowDefinition();
            grid.RowDefinitions.Add(rowDefinition);
            Grid.SetRow(btn, i);

            return btn;
        }

        private void add_lokacija(object sender, RoutedEventArgs e)
        {
            if (kraji_display.SelectedItem == null)
            {
                MessageBox.Show("Prosim izberite kraj", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                GRID_KRAJI.Visibility = Visibility.Hidden;
                GRID_ADD_Lokacija.Visibility = Visibility.Visible;
                label_za_kraj.Content = kraji_display.SelectedItem.ToString();
            }
        }

        private void ADD_finish_Click(object sender, RoutedEventArgs e)
        {
            if (!((lokacija_text.Text.Equals("")) || (st_zaposlenih_text.Text.Equals("")) || (st_ostarelih_text.Equals("")) || (st_sob_text.Text.Equals(""))))
            {
                string kraj = kraji_display.SelectedItem.ToString();
                var split_string = kraji_display.SelectedItem.ToString().Split();
                List<string> list = split_string.ToList();
                int st = list.Count() - 1;
                list.RemoveAt(st);
                string final = string.Join(" ", list);
                string final1="'"+final+"'";
                string ime_lokacije = "'"+lokacija_text.Text+"'";
                int zaposleni = Convert.ToInt32(st_zaposlenih_text.Text);
                int ostareli = Convert.ToInt32(st_ostarelih_text.Text);
                int sobe = Convert.ToInt32(st_sob_text.Text);
                conn.Insert_Lokacija(final1, ime_lokacije, zaposleni, ostareli, sobe);
                GRID_KRAJI.Visibility = Visibility.Visible;
                GRID_ADD_Lokacija.Visibility = Visibility.Hidden;
                kraji_display.SelectedItem = kraj;
                int i = 0;
                var neki = conn.Lokacije(final, split_string[st]);
                foreach (var item in neki)
                {

                    var splited = item.ToString().Split();
                    List<string> lokacija = splited.ToList();
                    st_zaposlenih = splited.Length - 1;
                    st_ostarelih = splited.Length - 2;
                    st_so = splited.Length - 3;
                    lokacija.RemoveRange(st_so, 3);
                    izpis = string.Join(" ", lokacija).ToString();
                    grid.Children.Add(add_button(izpis, i, st_zaposlenih, st_ostarelih, st_so));

                    i++;
                }
                Viewer.Content = grid;

            }

            else
            {
                MessageBox.Show("Preverite vnose", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}

