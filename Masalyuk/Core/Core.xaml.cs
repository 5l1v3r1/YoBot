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
using Newtonsoft.Json.Linq;

namespace Masalyuk.Core
{
    /// <summary>
    /// Логика взаимодействия для Core.xaml
    /// </summary>
    public partial class Core : Window
    {
        private const string tapi = "https://yobit.net/tapi";
        public Core()
        {

            InitializeComponent();
            GetBalance();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private async void getpairInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (pairBox.Text.Contains("/,.") || !pairBox.Text.Contains("_"))
                MessageBox.Show("Введите пару в формате XXX_YYY", "неверно введена пара", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            var pair = pairBox.Text.ToLower();
            currBuy.Text = await Api.GetBuyTask(pair);
            currSell.Text = await Api.GetSellTask(pair);
            currVol.Text = await Api.GetVolTask(pair);
        }

        public void GetBalance()
        {
            var wb = new WebBrowser();
           

            var key = MainWindow.ApiKey;
            var secret = MainWindow.Secret;
            
            var nonce = Auth.Auth.Nonce;
            nonce += 3;
            try
            {
                string bad = "Не вышло";

                string parameters = $"method=getInfo&nonce=" +
                                    +nonce;

                string address = $"{tapi}/";

                var keyByte = Encoding.UTF8.GetBytes(secret);

                string sign1 = string.Empty;
                byte[] inputBytes = Encoding.UTF8.GetBytes(parameters);
                using (var hmac = new HMACSHA512(keyByte))
                {
                    byte[] hashValue = hmac.ComputeHash(inputBytes);

                    StringBuilder hex1 = new StringBuilder(hashValue.Length * 2);
                    foreach (byte b in hashValue)
                    {
                        hex1.AppendFormat("{0:x2}", b);
                    }
                    sign1 = hex1.ToString();
                }

                var pair = "ltc_btc";
                WebRequest webRequest = (HttpWebRequest)WebRequest.Create(address);
                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.Timeout = 20000;
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    webRequest.Headers.Add("Key", key);
                    webRequest.Headers.Add("Sign", sign1);
                    



                    webRequest.ContentLength = parameters.Length;
                    using (var dataStream = webRequest.GetRequestStream())
                    {
                        dataStream.Write(inputBytes, 0, parameters.Length);
                    }
                    using (Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            var jsonResponse = sr.ReadToEnd();
                            var info = JObject.Parse(jsonResponse)["return"]["funds"].ToArray();
                            foreach (var p in info)
                            {
                                balanceBTC.Text = p.Parent["btc"].ToString();
                                balanceUSD.Text = p.Parent["usd"].ToString();
                                balanceRUR.Text = p.Parent["rur"].ToString();
                                break;
                            }

                        }
                    }
                }

                //return bad;

            }
            catch (Exception e)
            {
                //return Convert.ToString(e);

            }
        }

        private void graphButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ru.tradingview.com/");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = new Bot.BotSettings();
            MessageBox.Show("Цена покупки указывается в BTC. Все значения указываются в процентах", "Внимание!",
                MessageBoxButton.OK, MessageBoxImage.Information);
            b.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вам необходимо вписать только первую валюту из пары. Бот торгует в BTC", "Внимание!",
                MessageBoxButton.OK, MessageBoxImage.Information);
            var b = new Bot.BuyBot();
            b.ShowDialog();
        }
    }
}
