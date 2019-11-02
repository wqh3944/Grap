using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TaskCore.Model;

namespace TaskCore.Helper
{
    public class MyJob : IJob//创建IJob的实现类，并实现Excute方法。
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                JobDataMap data = context.JobDetail.JobDataMap;
                DateTime starttime = data.GetDateTimeValue("StartTime");
                DateTime endtime = data.GetDateTimeValue("EndTime");
                if (endtime < starttime)
                    endtime = endtime.AddDays(1);

                if(DateTime.Now<=endtime && DateTime.Now>=starttime)
                {
                    int lid = data.GetIntValue("LotteryID");
                    Console.WriteLine(DateTime.Now+",LotteryID:"+lid);
                    Lottery l = DapperHelper.getLotteryByID(lid);
                    if(l!=null)
                    {
                        LotteryTask t = new LotteryTask();
                        t.lotteryID = lid;
                        t.website = l.website;
                        t.lotteryType = l.lotteryType;
                        t.lotteryName = l.lotteryName;
                        t.state = 0;
                        t.createTime = DateTime.Now;
                        DapperHelper.setTask(t);
                    }
                    
                }
                
            });
        }
    }
}
