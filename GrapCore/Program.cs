using GrapCore.Controller;
using GrapCore.Helper;
using GrapCore.Model;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace GrapCore
{
    class Program
    {
        static void Main(string[] args)
        {
            LotteryTask t = DapperHelper.getOneTask();
            if(t!=null)
            {
                Console.WriteLine(string.Format("获取到采集任务：采集网站：{0} 彩票名称：{1}", t.website, t.lotteryType));
                t.state = 1;
                DapperHelper.setTaskState(t);
                

                Picker taskConsumer = new Picker();
                bool cmp = taskConsumer.ConsumeTask(t);

                Console.WriteLine("任务结束，采集结果："+cmp);

                t.state = 2;
                DapperHelper.setTaskState(t);
                Browser.cd.Quit();
            }
                        
        }

        
    }
}
