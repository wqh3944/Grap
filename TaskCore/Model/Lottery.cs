using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskCore.Model
{
    public class Lottery
    {
        public int lotteryID { get; set; }
        public string website { get; set; }
        public int lotteryType { get; set; }
        public string lotteryName { get; set; }
        public DateTime beginTime { get; set; }
        public DateTime endTime { get; set; }
        public int frequency { get; set; }        
    }
}
