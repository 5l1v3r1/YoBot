using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Masalyuk.Auth
{
    internal class Auth
    {
        private const string tapi = "https://yobit.net/tapi"; // ссылка на отправку запроса
        public static int Nonce { get; set; }
        //public static string ApiKey { get; set; }
        //public static string Secret { get; set; }
        public static Task<bool> Authentication(string key, string secret)
        {
            try
            {
                Nonce = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

                var parameters = $"method=getInfo&nonce=" +
                                    +Nonce;
                var address = $"{tapi}/";

                var keyByte = Encoding.UTF8.GetBytes(secret);

                var sign1 = string.Empty;
                var inputBytes = Encoding.UTF8.GetBytes(parameters);
                return Task.Run(() =>
                {
                    using (var hmac = new HMACSHA512(keyByte))
                    {
                        var hashValue = hmac.ComputeHash(inputBytes);

                        var hex1 = new StringBuilder(hashValue.Length * 2);
                        foreach (var b in hashValue) hex1.AppendFormat("{0:x2}", b);

                        sign1 = hex1.ToString();
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

                            using (var s = webRequest.GetResponse().GetResponseStream())
                            {
                                using (var sr = new StreamReader(s))
                                {
                                    var jsonResponse = sr.ReadToEnd();
                                    if (jsonResponse.Contains("error"))
                                        return false;
                                    else
                                        return true;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                });
            }

            catch (Exception e)
            {
                return Task.Run(() =>
                {
                    MessageBox.Show(Convert.ToString(e));
                    return false;
                });
            }
        }
    }
}