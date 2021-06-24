using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace TestAgni
{
    class GetMagazine
    {
        public List<Magazine> ReturnMagazine(string token, string category)
        {
            string RequestSubURL = "http://magazinestore.azurewebsites.net/api/magazines/" + token + "/" + category;
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

            foreach (var item in productsCatMagResponseRoot["data"])
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
