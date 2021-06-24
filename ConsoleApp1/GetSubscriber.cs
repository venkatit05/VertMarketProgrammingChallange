using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace TestAgni
{
    class GetSubscriber
    {
        public List<Subscriber> ReturnSubscriber(string token)
        {
            HttpWebResponse webresponse = null;
            try
            {
                string result = string.Empty;
                string RequestSubURL = "http://magazinestore.azurewebsites.net/api/subscribers/" + token;
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, RequestSubURL);

                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(RequestSubURL);
                webrequest.Method = "GET";
                webrequest.ContentType = "application/x-www-form-urlencoded";
                webresponse = (HttpWebResponse)webrequest.GetResponse();
                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
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

                return lstSub;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                webresponse.Close();
            }
 
        }
    }
}
