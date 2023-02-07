using System;
using System.Collections.Generic;
using System.Linq;
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
using Haley.Abstractions;
using Haley.Enums;
using Haley.Events;
using Haley.Models;
using Haley.MVVM;
using Haley.Services;
using Haley.Utils;
using Microsoft.Windows.Themes;

namespace DZO
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summar
    /// private static bool is_dark;
    
    public partial class Login : Window
    {
        dbConn conn=new dbConn();
        public static bool is_dark;

        public Login()
        {
            is_dark=true;
            InitializeComponent();
            if (Properties.Settings.Default.Username != string.Empty)
            {
                user.Text = Properties.Settings.Default.Username;
                password.Password =Properties.Settings.Default.Password;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Close_onClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Minimize_onClick(object sender, RoutedEventArgs e)
        {
            this.WindowState=WindowState.Minimized;
        }

        private void info_onClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Za vpis kot osebje uporabite: 'ime.priimek@dzo.si' \n" +
                "geslo pa 'ime'","Vpis",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Login_now_Click(object sender, RoutedEventArgs e)
        {
            if(conn.Login(user.Text,password.Password).Equals(true))
            {
                MainWindow main = new MainWindow();
                if (checkbox.IsChecked.Equals(true))
                {
                    DZO.Properties.Settings.Default.Username = user.Text;
                    DZO.Properties.Settings.Default.Password = password.Password.ToString();
                    DZO.Properties.Settings.Default.Save();

                    main.Show();
                    this.Close();

                }
                else
                {
                    main.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Geslo ni pravilno");
            }

        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            changedTheme();
            if (is_dark.Equals(false))
            {
                ResourceDictionary dict = new ResourceDictionary();
                dict.Source = new Uri("Theme/Light.xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            else if (is_dark.Equals(true))
            {
                ResourceDictionary dict = new ResourceDictionary();
                dict.Source = new Uri("Theme/Dark.xaml", UriKind.Relative);
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
        }
        private void changedTheme()
        {

            switch (is_dark)
            {
                case true: break;
                case false: break;
            }
            is_dark = !is_dark;
        }

    }
}
