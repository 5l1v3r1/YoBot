using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Masalyuk.Core;
using Newtonsoft.Json.Linq;

namespace Masalyuk.Bot
{
    /// <summary>
    /// Логика взаимодействия для BuyBot.xaml
    /// </summary>
    public partial class BuyBot : Window
    {
        private static bool buyCurr = true;
        private static bool buyOrders = false;
        public BuyBot()
        {
            InitializeComponent();
        }
        private const string tapi = "https://yobit.net/tapi";

        private async void buyButton_Click(object sender, RoutedEventArgs e)
        {
            var bot = new BotSettings();
            decimal summa = 0;
            summa = BotSettings.countCurrency;
            var pair = pBox.Text.ToLower() + "_usd";
            decimal procent = Convert.ToDecimal(BotSettings.procentPlus);
            if (buyCurr)
            {
                var realtimecount = await Api.GetSellTask(pair); // цена в стрингах
                decimal rlcount = Convert.ToDecimal(realtimecount); // цена в инт
                decimal price = 0;
                price += (rlcount + ((rlcount / 100) * procent)); // увеличенная цена
                decimal amount = summa / price;
                amount = decimal.Round(amount, 8, MidpointRounding.AwayFromZero);
                price = decimal.Round(price, 8, MidpointRounding.AwayFromZero);
                var ap = new Api();
                ap.PumpBuy(pair, price, amount);

            }

            if (buyOrders)
            {

            }
        }
        private void nowCurr_Click(object sender, RoutedEventArgs e)
        {
            buyCurr = true;
            buyOrders = false;
        }

        private void orders_Click(object sender, RoutedEventArgs e)
        {
            buyCurr = false;
            buyOrders = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "По цене : Бот закупает на указанные вами проценты выше.По ордерам : Бот закупает ордера на два выше текущего",
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
