using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace HZ.Framework
{
    static public class HttpHelper
    {
        /// <summary>
        /// HttpRequest
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpGet(string url, string contentType)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            string ret = string.Empty;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                if (string.IsNullOrEmpty(contentType)) contentType = "text/html;charset=UTF-8";
                request.ContentType = contentType;

                response = (HttpWebResponse)request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("utf-8")))
                    {
                        ret = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                ret = ex.Message.ToString();
            }
            finally
            {
                if (request != null) request.Abort();
                if (response != null) response.Close();
            }

            return ret;

        }


        public static string HttpPost(string url,string contentType, string data)
        {
            string ret = string.Empty;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            CookieContainer container = new CookieContainer();

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                if (string.IsNullOrEmpty(contentType)) contentType = "text/html;charset=UTF-8";
                request.ContentType = contentType;
                request.Method = "POST";
                request.CookieContainer = container;

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                int lenght = bytes.Length;
                request.ContentLength = lenght;
                using(Stream stream=response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("utf-8")))
                    {
                        ret = reader.ReadToEnd().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                ret = ex.Message.ToString();
            }
            finally
            {
                if (request != null) request.Abort();
                if (response != null) response.Close();
            }

            return ret;
        }
    }
}
