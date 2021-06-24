using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace TestAgni
{
    class GetToken
    {
        public string ReturnToken()
        {
            try
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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
