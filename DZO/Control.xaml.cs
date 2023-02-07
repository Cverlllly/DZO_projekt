using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace DZO
{

    public partial class Control : Window
    {
        dbConn con = new dbConn();
        public int st_zaposlenih = 0;
        public int st_ostarelih = 0;
        public int st_so = 0;
        public Control()
        {

            InitializeComponent();
            text_naziv.Items.Add("medicinska sestra");
            text_naziv.Items.Add("administracija");
            text_naziv.Items.Add("negovalec");
            text_naziv.Items.Add("gospodinjstvo");
            neki(odLabel.Text);

        }
        Grid grid = new Grid();
        public class Zaposleni
        {
            public int id { get; set; }

            public string ime { get; set; }

            public string priimek { get; set; }
            public string naziv { get; set; }
            public string telefon { get; set; }
        }
        Button add_button(int grid_row, string content_btn, int col, string kaje)
        {
            con.GetCountZaposleni(odLabel.Text);
            con.GetCountOstareli(odLabel.Text);
            Button btn = new Button();
            btn.Content = content_btn.ToString();
            if (kaje == "zaposleni")
            {
                btn.Click += (s, e) =>
                {
                    GRID_LOKACIJE.Visibility = Visibility.Hidden;
                    GRID_ZAPOSLENI.Visibility = Visibility.Visible;
                    Zaposleni_Lokacija.Content = odLabel.Text;
                    Izpis_zaposlenih();
                    
                };
                btn.Foreground = (SolidColorBrush)this.FindResource("Foreground");
            }
            else if (kaje == "ostareli")
            {
                btn.Click += (s, e) =>
                {
                    GRID_LOKACIJE.Visibility = Visibility.Hidden;
                    GRID_OSTARELI.Visibility = Visibility.Visible;
                    Ostareli_label.Content = odLabel.Text;
                    Izpis_ostarelih();
                    combostuff();
                };
                btn.Foreground = (SolidColorBrush)this.FindResource("Foreground");
            }
            else
            {
                btn.IsEnabled = false;
                btn.Foreground = (SolidColorBrush)this.FindResource("disabled");
            }

            btn.Margin = new Thickness(20);
            btn.BorderThickness = new Thickness(0);
            btn.Background = (SolidColorBrush)this.FindResource("btn");

            btn.Style = (Style)this.FindResource("roundeddynamic");
            btn.Width = 200;
            btn.Height = 200;
            btn.FontSize = 30;
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            ColumnDefinition rowDefinition = new ColumnDefinition();
            grid.ColumnDefinitions.Add(rowDefinition);
            Grid.SetColumn(btn, col);
            return btn;
        }
        public void combostuff()
        {
            if (text_sobe.Items.Count > 0)
            {
                text_sobe.Items.Clear();
            }
            List<string> list = new List<string>();
            int stsobe = Convert.ToInt32(con.GetCountStSob(odLabel.Text));
            foreach (Ostareli c in Ostareli_list.Items)
            {
                list.Add(c.st_sobe.ToString());
            }
            for (int x = 1; x < stsobe; x++)
            {
                if (!(list.Contains(x.ToString())))
                {
                    
                    text_sobe.Items.Add(x.ToString());
                }
                else
                {
                    this.text_sobe.Items.Add (new ComboBoxItem() { IsEnabled = false, Content = x.ToString() } );
                }
            }
        }
        public class Ostareli
        {
            public int id { get; set; }

            public string ime { get; set; }

            public string priimek { get; set; }
            public string st_sobe { get; set; }
            public int leto { get; set; }
        }
        public void Izpis_ostarelih()
        {
            List<Ostareli> items = new List<Ostareli>();
            
            foreach(var x in con.Get_osaterli(odLabel.Text))
            {
                var split = x.Split(" ");
                items.Add(new Ostareli() { id = Convert.ToInt32(split[0]), ime = split[1].ToString(), priimek = split[2].ToString(), st_sobe = (split[3].ToString()), leto = Convert.ToInt16(split[4]) });
            }
            Ostareli_list.ItemsSource = items;
        }
        public void Izpis_zaposlenih()
        {
            List<Zaposleni> items = new List<Zaposleni>();
            string telefonn;
            foreach (var x in con.Get_zaposleni(odLabel.Text))
            {
                string naziv;
                var split = x.Split(" ");
                if (x.Contains("medicinska"))
                {
                    naziv = split[3] + " " + split[4];
                    if (x.Contains("-"))
                    {
                        telefonn = split[5];
                    }
                    else
                    {
                        telefonn = split[5] + " " + split[6] + " " + split[7];
                    }
                }
                else
                {
                    naziv = split[3];
                    if (x.Contains("-"))
                    {
                        telefonn = split[4];
                    }
                    else
                    {
                        telefonn = split[4] + " " + split[5] + " " + split[6];
                    }

                }
                items.Add(new Zaposleni() { id = Convert.ToInt32(split[0]), ime = split[1].ToString(), priimek = split[2].ToString(), naziv = naziv.ToString(), telefon = telefonn });
            }
            Zaposleni_list.ItemsSource = items;
        }
        private void lokacije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            odLabel.Text=lokacije.SelectedItem.ToString();
            neki(odLabel.Text);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Close_onClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Login log = new Login();
            log.Show();
        }

        private void Minimize_onClick(object sender, RoutedEventArgs e)
        {
            this.WindowState=WindowState.Minimized;
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        List<string> lokacija;
        public async void neki(string oldname)
        {
            await Task.Delay(20);
            grid.Children.Clear();
            int i = 0;

            foreach (var item in con.Lokacije_vse(oldname))
            {

                var splited = item.ToString().Split();
                lokacija = splited.ToList();
                st_zaposlenih = splited.Length - 3;
                st_ostarelih = splited.Length - 2;
                st_so = splited.Length - 1;
                for (int x = 0; x < 3; x++)
                {
                    if (x == 0)
                    {
                        grid.Children.Add(add_button(i, con.GetCountZaposleni(odLabel.Text), x,"zaposleni"));
                    }
                    else if (x == 1)
                    {
                        grid.Children.Add(add_button(i, con.GetCountOstareli(odLabel.Text), x,"ostareli"));
                    }
                    else
                    {
                        grid.Children.Add(add_button(i, lokacija[st_so], x,"sobe"));
                    }

                }
                grid.HorizontalAlignment = HorizontalAlignment.Center;
                Viewer.Content = grid;
                i++;
            }
        }

        private void change_Click(object sender, RoutedEventArgs e)
        {

            if (!(lokacije.SelectedItem == null))
            {
                GRID_LOKACIJE.Visibility = Visibility.Hidden;
                GRID_CHANGE.Visibility = Visibility.Visible;
                lokacija_ime_text.Text = odLabel.Text;
                    stevilo_zaposlenih.Text = lokacija[st_zaposlenih];
                stevilo_ostarelih.Text = lokacija[st_ostarelih];
                stevilo_sob.Text = lokacija[st_so];
            }
            else
            {
                MessageBox.Show("Prosim izberite lokacijo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void back_to_Lokacije_Click(object sender, RoutedEventArgs e)
        {
            GRID_CHANGE.Visibility= Visibility.Hidden;
            GRID_LOKACIJE.Visibility= Visibility.Visible;
        }

        private void changed_Click(object sender, RoutedEventArgs e)
        {
            
            con.UpdateLokacije(odLabel.Text,lokacija_ime_text.Text,Convert.ToInt16(stevilo_zaposlenih.Text), Convert.ToInt16(stevilo_ostarelih.Text),Convert.ToInt16(stevilo_sob.Text));
            con.lokacija(lokacija_ime_text.Text);
            odLabel.Text = lokacija_ime_text.Text;
            neki(lokacija_ime_text.Text);
            GRID_CHANGE.Visibility = Visibility.Hidden;
            GRID_LOKACIJE.Visibility = Visibility.Visible;
        }
        int id_zap;
        private void Zaposleni_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //foreach (DataGridViewRow row in dataGridView.SelectedRows)
            //{
            //   string value1 = row.Cells[0].Value.ToString();
            //   string value2 = row.Cells[1].Value.ToString();
            //...
            //}
            Zaposleni Value=Zaposleni_list.SelectedItem as Zaposleni;
            if(Value!=null)
            {
                id_zap = Value.id;
                text_ime.Text = Value.ime;
                text_priimek.Text = Value.priimek;
                text_naziv.SelectedItem = Value.naziv;
                text_telefon.Text = Value.telefon;

            }
            else
            {

            }

        }

        private void Add_user_Click(object sender, RoutedEventArgs e)
        {
            if (!(text_ime.Text.Equals("") || text_priimek.Text.Equals("") || text_telefon.Equals("")))
            {
                bool ne = false;
                foreach (var x in con.Get_zaposleni(odLabel.Text))
                {
                    if (x.Contains(text_telefon.Text))
                    {
                        MessageBox.Show("Ta telefonska že obstaja");
                        ne = true;
                    }
                }
                if (ne == false)
                {
                    con.InsertUser(text_ime.Text, text_priimek.Text, text_naziv.Text, text_telefon.Text, odLabel.Text);
                    Izpis_zaposlenih();
                }
            }
            else
            {
                MessageBox.Show("Polja so prazna");
            }

        }

        private void edit_user_Click(object sender, RoutedEventArgs e)
        {
            con.UpdateZaposleni(text_ime.Text, text_priimek.Text, text_naziv.Text, text_telefon.Text, id_zap);
            Izpis_zaposlenih();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Delete_user_Click(object sender, RoutedEventArgs e)
        {
            con.DeleteZaposleni(id_zap);
            Zaposleni_list.SelectedIndex = 1;
            Izpis_zaposlenih();
        }
        int id_ost;
        private void Ostareli_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ostareli Value = Ostareli_list.SelectedItem as Ostareli;
            if (Value != null)
            {
                id_ost = Value.id;
                text_ime_o.Text = Value.ime;
                text_priimek_o.Text = Value.priimek;
                text_sobe.Text = Value.st_sobe;
                text_leto_o.Text = Value.leto.ToString();

            }
            else
            {

            }
        }

        private void To_Lokacija_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void Add_ostareli_Click(object sender, RoutedEventArgs e)
        {
            if(!(text_ime_o.Text.Equals("") || text_priimek_o.Text.Equals("") || text_leto_o.Text.Equals("")))
            {
                con.InsertOstareli(text_ime_o.Text, text_priimek_o.Text, text_sobe.Text, Convert.ToInt16(text_leto_o.Text), odLabel.Text);
                Izpis_ostarelih();
                combostuff();
            }
            else
            {
                int stsobe = Convert.ToInt32(text_sobe.Text);
                List<string> list = new List<string>();
                foreach (Ostareli c in Ostareli_list.Items)
                {
                    list.Add(c.st_sobe.ToString());
                }
                for (int x = 1; x < stsobe; x++)
                {
                    if (!(list.Contains(x.ToString())))
                    {

                    }
                }

            }
        }

        private void edit_ostareli_Click(object sender, RoutedEventArgs e)
        {
            Ostareli Value = Ostareli_list.SelectedItem as Ostareli;
            con.EditOstareli(text_ime_o.Text, text_priimek_o.Text, text_sobe.Text, Convert.ToInt16(text_leto_o.Text), Value.id);
            Izpis_ostarelih();
            combostuff();
        }

        private void Delete_ostareli_Click(object sender, RoutedEventArgs e)
        {
            Ostareli Value = Ostareli_list.SelectedItem as Ostareli;
            con.DeleteOstareli(Value.id); 
            Izpis_ostarelih();
            combostuff();
        }

        private void back_lokacija_Click(object sender, RoutedEventArgs e)
        {
            GRID_ZAPOSLENI.Visibility = Visibility.Hidden;
            GRID_LOKACIJE.Visibility = Visibility.Visible;
        }

        private void back_to_Lokacija_Click(object sender, RoutedEventArgs e)
        {
            GRID_OSTARELI.Visibility = Visibility.Hidden;
            GRID_LOKACIJE.Visibility = Visibility.Visible;
        }
    }
}
