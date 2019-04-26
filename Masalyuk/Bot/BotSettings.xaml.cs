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

namespace Masalyuk.Bot
{
    /// <summary>
    /// Логика взаимодействия для BotSettings.xaml
    /// </summary>
    public partial class BotSettings : Window
    {
        public static decimal procentPlus { get; set; }
        public static decimal countCurrency { get; set; }
        public BotSettings()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                countCurrency = Convert.ToDecimal(procentBTC.Text);
                procentPlus = Convert.ToDecimal(plusProcent.Text);
                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Введите сумму через запятую");
            }
            
        }
    }
}
