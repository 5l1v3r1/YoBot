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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Masalyuk.Core;

namespace Masalyuk
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string ApiKey { get; set; } // ключ API
        public static string Secret { get;set; } // ключ, генерирующийся вместе с API
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void authButton_Click(object sender, RoutedEventArgs e)
        {
            
            if(!string.IsNullOrWhiteSpace(keyBox.Text) && !string.IsNullOrWhiteSpace(secretBox.Text)) // проверка на пустую строку
            {
                ApiKey = keyBox.Text;
                Secret = secretBox.Text;
                var form = await Auth.Auth.Authentication(ApiKey, Secret);
                if (form)
                {
                    this.Hide();
                    var frm = new Core.Core();
                    frm.Show();
                }

                else
                {
                    MessageBox.Show("Не удалось авторизоваться.Попробуйте чуть позже");
                }
            }
            else
            {
                MessageBox.Show("Поля не должны быть пустыми", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void whyButt_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Попробуйте поменять API Ключи, в большинстве случаев проблема именно в них", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void licBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Window_Activated(object sender, EventArgs e)
        {

            var n = new Lic(); ;
            this.licBox.Text = n.GetPay();
        }
    }
}
