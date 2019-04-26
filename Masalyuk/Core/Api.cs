using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Masalyuk.Bot;
using Newtonsoft.Json.Linq;

namespace Masalyuk.Core
{
    class Api
    {
        private static string tickerUrl = "https://yobit.net/api/3/ticker/";
        private const string tapi = "https://yobit.net/tapi";
        private static WebClient Client = new WebClient();

        public static Task<string> GetBuyTask(string pair)
        {
            
            return Task.Run(() =>
            {
                string currBuy = String.Empty;
                try
                {
                    var response = Client.DownloadString(tickerUrl + pair); // загружаем строку ответа
                    var pairInfo = JObject.Parse(response)[pair].ToArray();
                    foreach (var p in pairInfo)
                    {
                        currBuy = p.Parent["buy"].ToString();
                        break;
                    }

                    return currBuy;
                }
                catch (Exception e)
                {
                    return "Null";
                }
            });
        }

        public static Task<string> GetSellTask(string pair)
        {

            return Task.Run(() =>
            {
                string currSell = String.Empty;
                try
                {
                    var response = Client.DownloadString(tickerUrl + pair); // загружаем строку ответа
                    var pairInfo = JObject.Parse(response)[pair].ToArray();
                    foreach (var p in pairInfo)
                    {
                        currSell = p.Parent["sell"].ToString();
                        break;
                    }

                    return currSell;
                }
                catch (Exception e)
                {
                    return "Null";
                }
            });
        }

        public static Task<string> GetVolTask(string pair)
        {

            return Task.Run(() =>
            {
                string currVol = String.Empty;
                try
                {
                    var response = Client.DownloadString(tickerUrl + pair); // загружаем строку ответа
                    var pairInfo = JObject.Parse(response)[pair].ToArray();
                    foreach (var p in pairInfo)
                    {
                        currVol = p.Parent["vol"].ToString();
                        break;
                    }

                    return currVol;
                }
                catch (Exception e)
                {
                    return "Null";
                }
            });
        }

        public void PumpBuy(string pairs, decimal rate, decimal amount)
        {
            var wb = new WebBrowser();


            var key = MainWindow.ApiKey;
            var secret = MainWindow.Secret;

            var nonce = Auth.Auth.Nonce;
            nonce += 9;
            try
            {
                string bad = "Не вышло";

                string parameters = $"method=Trade&pair=" + pairs + "&type=buy&rate=" + rate + "&amount=" + amount + "&nonce=" + nonce;
                parameters = parameters.Replace(',', '.');
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
                        var b = new BuyBot();
                        using (StreamReader sr = new StreamReader(s))
                        {
                            var jsonResponse = sr.ReadToEnd();
                            if (jsonResponse.Contains("error"))
                                 b.yesorno.Text = "Не удалось";
                            var info = JObject.Parse(jsonResponse)["return"].ToArray();
                            
                            foreach (var p in info)
                            {
                                var temp = p.Parent["received"].ToString();
                                b.yesorno.Text = "Успешно купил " + temp;
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
    }
}
