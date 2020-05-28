using System.Net;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace FGOAssetsModifyTool
{
    class HttpRequest
    {
        public static string PhttpReq(string url, string parameters)
        {

            HttpWebRequest hRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            hRequest.CookieContainer = new CookieContainer();

            hRequest.Accept = "gzip, identity";
            hRequest.UserAgent = "Dalvik/2.1.0 (Linux; U; Android 6.0.1; MI 6 Build/V417IR)";
            hRequest.ServicePoint.Expect100Continue = false;
            hRequest.KeepAlive = true;
            hRequest.Method = "POST";

            hRequest.ContentType = "application/x-www-form-urlencoded";

            bool first = true;

            hRequest.ContentLength = parameters.Length;

            byte[] dataParsed = Encoding.UTF8.GetBytes(parameters);
            hRequest.GetRequestStream().Write(dataParsed, 0, dataParsed.Length);


            hRequest.Timeout = 5 * 1000;

            HttpWebResponse response = (HttpWebResponse)hRequest.GetResponse();

            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
    }
}
