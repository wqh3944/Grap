using GrapCore.Helper;
using GrapCore.Model;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GrapCore.Controller
{
    class Picker
    {        
        LotteryTask task=null;        
        public Picker()
        {
            
        }

        /// <summary>
        /// 获取开奖结果
        /// </summary>
        /// <param name="PageURL"></param>
        /// <param name="name"></param>
        private LotteryResult GetResult(string subUrl)
        {
            LotteryResult lr = new LotteryResult();
            lr.isRight = false;
            if (task.website.StartsWith("https://www.1393c.com"))
            {
                string subHtml = Browser.getHtml(subUrl);
                HtmlAgilityPack.HtmlDocument subdoc = new HtmlAgilityPack.HtmlDocument();
                subdoc.LoadHtml(subHtml);
                HtmlNodeCollection nodes = subdoc.DocumentNode.SelectNodes("//table[@class='lot-table']//tr[2]");
                if (nodes != null)
                {
                    List<LotteryResult> rst = new List<LotteryResult>();

                    foreach (HtmlNode node in nodes)
                    {
                        HtmlNodeCollection tds = node.SelectNodes("td");
                        int tdNum = tds.Count;
                        //lr = new LotteryResult();
                        int no = 0;
                        lr.lotteryID = task.lotteryID;
                        lr.lotteryType = task.lotteryType;
                        HtmlNodeCollection ps = tds[no++].SelectNodes("p");
                        lr.No = StringHelper.trimEnd(ps[0].InnerText).Replace("-", "");
                        lr.time = StringHelper.trimEnd(ps[1].InnerText);
                        
                        //开奖号码
                        Match m = Regex.Match(tds[no++].InnerHtml, @"\d+");
                        while (m.Success)
                        {
                            lr.Number += (StringHelper.trimEnd(m.Value) + ",");
                            m = m.NextMatch();
                        }
                        //冠亚和                    
                        lr.guanYaHe += StringHelper.trimEnd(tds[no++].InnerText) + "," + StringHelper.trimEnd(tds[no++].InnerText) + "," + StringHelper.trimEnd(tds[no++].InnerText);
                        //龙虎
                        lr.longHu = StringHelper.trimEnd(tds[no++].InnerText) + "," + StringHelper.trimEnd(tds[no++].InnerText) + "," + StringHelper.trimEnd(tds[no++].InnerText) + "," + StringHelper.trimEnd(tds[no++].InnerText) + "," + StringHelper.trimEnd(tds[no++].InnerText);

                        rst.Add(lr);
                    }
                    //DapperHelper.setLotteryResult(rst);
                }
            }
            else if (task.website.StartsWith("https://1680380.com"))
            {
                string subHtml = Browser.getHtml(subUrl);
                HtmlAgilityPack.HtmlDocument subdoc = new HtmlAgilityPack.HtmlDocument();
                subdoc.LoadHtml(subHtml);
                HtmlNodeCollection nodes = subdoc.DocumentNode.SelectNodes("//div[@id='jrsmhmtj']//tr[2]");//position()>1
                if (nodes != null)
                {
                    List<LotteryResult> rst = new List<LotteryResult>();

                    foreach (HtmlNode node in nodes)
                    {
                        HtmlNodeCollection tds = node.SelectNodes("td");
                        //lr = new LotteryResult();
                        int no = 0;
                        lr.lotteryID = task.lotteryID;
                        lr.lotteryType = task.lotteryType;
                        //时间
                        lr.time = StringHelper.trimEnd(tds[no++].InnerText);
                        //期数
                        lr.No = StringHelper.trimEnd(tds[no++].InnerText).Replace("-", "");

                        //开奖号码
                        HtmlNodeCollection lis = tds[no++].SelectNodes("ul/li/i");
                        foreach(HtmlNode li in lis)
                        {
                            lr.Number += StringHelper.trimEnd(li.InnerText) + ",";
                        }
                                                
                        //冠亚和                    
                        lr.guanYaHe += StringHelper.trimEnd(tds[no++].InnerText) + "," + StringHelper.trimEnd(tds[no++].InnerText) + "," + StringHelper.trimEnd(tds[no++].InnerText);
                        //龙虎
                        lr.longHu = StringHelper.trimEnd(tds[no++].InnerText) + "," + StringHelper.trimEnd(tds[no++].InnerText) + "," + StringHelper.trimEnd(tds[no++].InnerText) + "," + StringHelper.trimEnd(tds[no++].InnerText) + "," + StringHelper.trimEnd(tds[no++].InnerText);

                        rst.Add(lr);
                    }
                    //DapperHelper.setLotteryResult(rst);
                }
            }
            else
            {
                string apiUrl = string.Format("http://cx971.com/pc/index/getLottoDetail?lottoName={0}&page=1&showpage=10",subUrl);
                string subHtml = Browser.getResponse(apiUrl);

                HistoryData hd =  JsonConvert.DeserializeObject<HistoryData>(subHtml);

                if (hd != null)
                {
                    List<LotteryResult> rst = new List<LotteryResult>();

                    foreach (History his in hd.data.history)
                    {                       
                        //lr = new LotteryResult();
                        lr.lotteryID = task.lotteryID;
                        lr.lotteryType = task.lotteryType;
                        //时间
                        lr.time = his.timeDraw;
                        //期数
                        lr.No = (his.qishu+"").Replace("-", "");

                        //开奖号码                        
                        foreach (int n in his.nums)
                        {
                            lr.Number += StringHelper.trimEnd(n.ToString()) + ",";
                        }               
                        
                        //冠亚和                    
                        lr.guanYaHe = ((his.nums[0]+his.nums[1]) + "," + (his.SumBigSmall=="1"?"大,":"小,")  + (his.SumOddEven == "1" ? "单" : "双"));
                        //龙虎
                        lr.longHu = (his.DragonTiger1=="1"?"龙,":"虎,") + (his.DragonTiger2 == "1" ? "龙," : "虎,")+ (his.DragonTiger3 == "1" ? "龙," : "虎,")+ (his.DragonTiger4 == "1" ? "龙," : "虎,")+ (his.DragonTiger5 == "1" ? "龙" : "虎");

                        rst.Add(lr);
                        break;                           
                    }
                    //DapperHelper.setLotteryResult(rst);
                }
            }
            return lr;
        }
        //彩种种类
        private string GetLotteryType(string page)
        {
            string subPage = "";
            try
            {
                List<LotteryType> lts = DapperHelper.getLotteryType(page);
                if (page.StartsWith("https://www.1393c.com"))
                {
                    if (lts == null || lts.Count == 0)
                    {
                        string html = Browser.getHtml(page);
                        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(html);
                        HtmlNodeCollection types = doc.DocumentNode.SelectNodes("//div[@class='lot-menu']/div[1]/div");
                        if (types != null)
                        {
                            lts = new List<LotteryType>();
                            foreach (HtmlNode type in types)
                            {
                                
                                HtmlNodeCollection links = type.SelectNodes("div/a");
                                foreach (HtmlNode link in links)
                                {
                                    LotteryType lt = new LotteryType();
                                    string url = link.GetAttributeValue("href", "null");
                                    string name = link.InnerText;

                                    lt.website = page;
                                    lt.LotteryName = name;
                                    lt.LotteryShortName = url;
                                    lts.Add(lt);
                                    if (String.Equals(task.lotteryName, name, StringComparison.CurrentCultureIgnoreCase))
                                        subPage = page+url;
                                }
                            }
                            DapperHelper.setLotteryType(lts);
                        }
                    }
                    else
                    {
                        foreach (LotteryType lt in lts)
                        {
                            if (String.Equals(task.lotteryName, lt.LotteryName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                subPage = page + lt.LotteryShortName;
                                break;
                            }
                        }
                    }
                    
                }
                else if(page.StartsWith("https://1680380.com"))
                {
                    if (lts == null || lts.Count == 0)
                    {
                        string html = Browser.getHtml(page);
                        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(html);
                        HtmlNodeCollection types = doc.DocumentNode.SelectNodes("//div[@class='content_middle']//a");
                        if (types != null)
                        {
                            lts = new List<LotteryType>();
                            foreach (HtmlNode type in types)
                            {
                                LotteryType lt = new LotteryType();
                                string url = type.GetAttributeValue("href", "null");
                                string name = type.InnerText;

                                lt.website = page;
                                lt.LotteryName = name;
                                lt.LotteryShortName = url;
                                lts.Add(lt);
                                if (String.Equals(task.lotteryName, name, StringComparison.CurrentCultureIgnoreCase))
                                    subPage = page + url;
                            }
                            DapperHelper.setLotteryType(lts);
                        }
                    }
                    else
                    {
                        foreach (LotteryType lt in lts)
                        {
                            if (String.Equals(task.lotteryName, lt.LotteryName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                subPage = page + lt.LotteryShortName;
                                break;
                            }
                        }
                    }
                    
                }
                else
                {                    
                    if(lts==null || lts.Count==0)
                    {
                        string html = Browser.getHtml(page);
                        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(html);
                        HtmlNodeCollection names = doc.DocumentNode.SelectNodes("//div[@class='lotto-list']//div[@class='name']");
                        HtmlNodeCollection snames = doc.DocumentNode.SelectNodes("//div[@class='lotto-list']//div[@class='lotto-img']/div");
                        if (names != null)
                        {
                            string url = "";
                            lts = new List<LotteryType>();
                            for (int i=0;i<names.Count;i++)
                            {
                                LotteryType lt = new LotteryType();
                                lt.website = page;
                                lt.LotteryName = names[i].InnerText;
                                lt.LotteryShortName = snames[i].GetAttributeValue("class", "null-null").Split('-')[0];
                                lts.Add(lt);
                                if (String.Equals(task.lotteryName, lt.LotteryName, StringComparison.CurrentCultureIgnoreCase))
                                    subPage = page + url;
                            }
                            DapperHelper.setLotteryType(lts);
                        }
                    }
                    else
                    {
                        foreach (LotteryType lt in lts)
                        {
                            if (String.Equals(task.lotteryName, lt.LotteryName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                subPage = lt.LotteryShortName;
                                break;
                            }
                        }
                    }            
                }
                
                

            }
            catch(Exception e)
            {

            }
            return subPage;
        }            

        public bool ConsumeTask(LotteryTask t)
        {
            task = t;
            Browser.InitBrowser();                       
            
            Console.Write("开始采集:" + task.website);
            //彩种种类
            string subPage = GetLotteryType(task.website);
            //开奖结果                
            LotteryResult lr = GetResult(subPage);
            Console.Write("采集结果:" + lr.ToString());                

            bool cmp = true;
            //对比结果
            List<LotteryResult> lrs =  DapperHelper.getResultByNo(lr.No.Substring(lr.No.Length-6,6),lr.lotteryType);
            for (int i = 0; i < lrs.Count; i++)
            {
                if (lrs[i].Number != lr.Number || lrs[i].guanYaHe != lr.guanYaHe)
                    cmp = false;
            }

            lr.isRight = cmp;

            DapperHelper.setLotteryResult(lr);

            return cmp;

        }               
    }
}
