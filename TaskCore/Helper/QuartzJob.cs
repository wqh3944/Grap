using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;
using System;
using System.Collections.Generic;
using System.Text;
using TaskCore.Model;
using static Quartz.MisfireInstruction;

namespace TaskCore.Helper
{
    public class QuartzJob
    {
        public static async void CreateJob(string name, string group,Lottery l)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();

            DateTimeOffset startTime,t1,t2;
            int min = 0;
            t1 = new DateTime(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, DateTimeOffset.Now.Hour, DateTimeOffset.Now.Minute, 0);
            t2 = new DateTime(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, l.beginTime.Hour, l.beginTime.Minute, 0);

            if(t1<t2)
            {
                //还没到开奖时间
                startTime = t2;
            }
            else
            {
                //已过开奖时间
                min = (t1 - t2).Minutes % l.frequency;
                startTime = t1;
                if (min > 0)
                    startTime = startTime.AddMinutes(l.frequency - min);
            }

            //DateTimeOffset endTime = DateBuilder.NextGivenMinuteDate(DateTime.Now, 10);

            IJobDetail job = JobBuilder.Create<MyJob>()
                             .WithIdentity(name, group)
                             .Build();
            job.JobDataMap.Put("StartTime", l.beginTime.ToString("T"));
            job.JobDataMap.Put("EndTime", l.endTime.ToString("T"));
            job.JobDataMap.Put("LotteryID",l.lotteryID);

            //DailyCalendar dailyCalendar = new DailyCalendar(l.beginTime.ToString("T"), l.endTime.ToString("T"));
            //dailyCalendar.InvertTimeRange = true;
            //await scheduler.AddCalendar("everyday", dailyCalendar, true, true);
            

            ITrigger trigger = TriggerBuilder.Create()
                .StartAt(startTime)        
                //.ModifiedByCalendar("everyday")
                .WithSimpleSchedule(b => b.WithIntervalInMinutes(l.frequency)
                .RepeatForever())//无限循环执行                
                .Build();
           


            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
    }
}
