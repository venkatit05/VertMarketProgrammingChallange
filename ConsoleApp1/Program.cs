using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            try
            {
                var Jsontoken = ReturnToken();
                dynamic dytoken = JsonConvert.DeserializeObject(Jsontoken);
                string token = dytoken["token"];
                var categoriesList = ReturnCategories(token);
                dynamic productsResponseRoot = JsonConvert.DeserializeObject(categoriesList);
                List<List<Magazine>> lstMagCat = new List<List<Magazine>>();
                foreach (string item in productsResponseRoot["data"])
                {
                    lstMagCat.Add(ReturnNewsCatMag(token, item));
                }

                List<Subscriber> lstSub = new List<Subscriber>();
                lstSub = ReturnSubscriber(token);

                lstSub = IsSubscriberFound(lstMagCat, lstSub);

                string JsonSubscriber = JsonConvert.SerializeObject(lstSub);

                PostAnswers(token, JsonSubscriber);

                Console.Read();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                
            }
        }

        private static List<Subscriber> IsSubscriberFound(List<List<Magazine>> lstMagCat,List<Subscriber> lstSub)
        {
            List<Subscriber> filteredSub = new List<Subscriber>();

            foreach (List<Magazine> maz in lstMagCat)
            {
                foreach (Subscriber sub in lstSub)
                {
                    foreach (string s in sub.magIds)
                    {
                        var subscriberFnd = from a in maz where a.Id == s select a;
                        if (subscriberFnd != null)
                        {
                            int count = filteredSub.Where(P => P.Id == sub.Id).Count();
                            if (count == 0)
                                filteredSub.Add(sub);
                        }
                    }
                }

            }
            return filteredSub;
        }
        private static string  ReturnToken()
        {
            string RequestSubURL = "http://magazinestore.azurewebsites.net/api/token";
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, RequestSubURL);

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(RequestSubURL);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();

            return result;
        }
       private static List<Subscriber> ReturnSubscriber(string token)
        {
            string RequestSubURL = "http://magazinestore.azurewebsites.net/api/subscribers/" + token;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, RequestSubURL);

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(RequestSubURL);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();

            dynamic productsSubResponseRoot = JsonConvert.DeserializeObject(result);
            List<Subscriber> lstSub = new List<Subscriber>();
            foreach (var item in productsSubResponseRoot["data"])
            {
                Subscriber subIns = new Subscriber();
                subIns.FirstName = item["firstName"];
                subIns.Id = item["id"];
                subIns.LastName = item["lastName"];
                foreach (string s in item["magazineIds"])
                {
                    subIns.magIds.Add(s);
                }
                lstSub.Add(subIns);
            }

            webresponse.Close();
            return lstSub;
        }

        private static string ReturnCategories(string token)
        {
            string RequestSubURL = "http://magazinestore.azurewebsites.net/api/categories/" + token;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, RequestSubURL);

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(RequestSubURL);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            webresponse.Close();
            return result;
        }

        private static void PostAnswers(string token, string jsonObject)
        {

            string RequestSubURL = "http://magazinestore.azurewebsites.net/api/answer/" + token;

            byte[] bytes = Encoding.UTF8.GetBytes(jsonObject);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(RequestSubURL);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = bytes.Length;
            httpWebRequest.ContentType = "application/json";
            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Count());
            }
            var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            if (httpWebResponse.StatusCode != HttpStatusCode.OK)
            {
                string message = String.Format("POST failed. Received HTTP {0}", httpWebResponse.StatusCode);
                throw new ApplicationException(message);
            }
        }
        private static List<Magazine> ReturnNewsCatMag(string token,string category)
        {
            string RequestSubURL = "http://magazinestore.azurewebsites.net/api/magazines/"+ token +"/" + category ;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, RequestSubURL);

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(RequestSubURL);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            webresponse.Close();
            dynamic productsCatMagResponseRoot = JsonConvert.DeserializeObject(result);
            List<Magazine> lstMag = new List<Magazine>();
           
            foreach(var item in productsCatMagResponseRoot["data"])
            {
                Magazine Maz = new Magazine();
                Maz.Id = item["id"];
                Maz.Category = item["category"];
                Maz.Name = item["name"];
                lstMag.Add(Maz);
            }
            return lstMag;
        }
    }
}
