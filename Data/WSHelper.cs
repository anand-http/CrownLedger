using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace fintech.Data
{
    public class WSHelper
    {
        public string WebServiceImageUpload(string strReqURL, string strReqBody, string strReqGetPost, out string strReqCode, out string strReqMessage)
        {
            string strJsonResponse = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(strReqURL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = strReqGetPost;

            if (strReqBody != "")
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(strReqBody);
                    streamWriter.Flush();
                }
            }
            HttpWebResponse httpResponse = null;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    strJsonResponse = streamReader.ReadToEnd();
                }
                strReqCode = "";
                strReqMessage = "";
            }
            catch (WebException we)
            {
                strJsonResponse = "";
                strReqCode = ((HttpWebResponse)we.Response).StatusCode.ToString();
                strReqMessage = ((HttpWebResponse)we.Response).StatusDescription.ToString();
            }
            return strJsonResponse;

        }
        public string WebServiceCall(string URL, string strReqLogin, string strReqPass, string strReqURL, string strReqBody, string strReqGetPost, out string strReqCode, out string strReqMessage)
        {
            string strJsonResponse = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL + strReqURL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = strReqGetPost;
            //string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(strReqLogin + ":" + strReqPass));
            //httpWebRequest.Headers.Add("Authorization", "Basic " + encoded);

            if (strReqBody != "")
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(strReqBody);
                    streamWriter.Flush();
                }
            }
            HttpWebResponse httpResponse = null;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    strJsonResponse = streamReader.ReadToEnd();
                }
                strReqCode = "";
                strReqMessage = "";
            }
            catch (WebException we)
            {
                strJsonResponse = "";
                strReqCode = ((HttpWebResponse)we.Response).StatusCode.ToString();
                strReqMessage = ((HttpWebResponse)we.Response).StatusDescription.ToString();
            }
            return strJsonResponse;

        }
        public string WebService2FACall(string strReqLogin, string strReqPass, string strReqURL, string strReqBody, string strReqGetPost, out string strReqCode, out string strReqMessage)
        {
            try
            {
                string strJsonResponse = "";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["2FAURL"].ToString() + strReqURL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = strReqGetPost;
                httpWebRequest.Timeout = 600000;
                //if (!(string.IsNullOrWhiteSpace(strReqLogin) && string.IsNullOrWhiteSpace(strReqPass)))
                //{
                //    string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(strReqLogin + ":" + strReqPass));
                //    httpWebRequest.Headers.Add("Authorization", "Basic " + encoded);
                //}

                if (strReqBody != "")
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(strReqBody);
                        streamWriter.Flush();
                    }
                }
                HttpWebResponse httpResponse = null;
                try
                {
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        strJsonResponse = streamReader.ReadToEnd();
                    }
                    strReqCode = "";
                    strReqMessage = "";
                }
                catch (WebException we)
                {
                    strJsonResponse = "";
                    strReqCode = ((HttpWebResponse)we.Response).StatusCode.ToString();
                    strReqMessage = ((HttpWebResponse)we.Response).StatusDescription.ToString();
                }

                return strJsonResponse;

            }
            catch (System.Exception ex)
            {
                strReqCode = ex.Message;
                strReqMessage = ex.Message;
                return ex.Message;
            }

        }
    }
}