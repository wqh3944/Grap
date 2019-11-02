using System;
using System.Collections.Generic;
using TaskCore.Helper;
using TaskCore.Model;

namespace TaskCore
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Lottery> lts =  DapperHelper.getLotterys();

            foreach (Lottery l in lts)
            {               
                QuartzJob.CreateJob("task" + l.lotteryID, "CreateTask", l);
            }
            
            Console.ReadLine();
        }
    }
}
