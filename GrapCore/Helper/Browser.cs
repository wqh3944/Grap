using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrapCore.Helper
{    
    class Browser
    {        

        //static WebBrowser webBrowser1 = null;
        public static ChromeDriver cd = null;       


        public static void InitBrowser()
        {
            ChromeOptions op = new ChromeOptions();
            op.AddArguments("--headless");//开启无gui模式
            cd = new ChromeDriver(Environment.CurrentDirectory, op, TimeSpan.FromSeconds(180));
        }
                        

        public static string getHtml(string page)
        {
            cd.Navigate().GoToUrl(page);

            return cd.PageSource;

        }

        public static string getResponse(string requestURL)
        {
            //string apiUrl = string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", "wx9402286955bc5ba8", "5de87a92384ae374a7f7eba482dad524", code);

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(requestURL);
            myRequest.Method = "GET";
            myRequest.ContentType = "text/html;charset=utf-8";
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            Stream myResponseStream = myResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, System.Text.Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();


            //User u = JsonConvert.DeserializeObject<User>(retString);

            return retString;
            
        }

        
    }
}
