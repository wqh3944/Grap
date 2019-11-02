using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapCore.Model
{
    class LotteryTask
    {
        public int TaskID { get; set; }
        public int lotteryID { get; set; }
        public string website { get; set; }
        public int lotteryType { get; set; }
        public string lotteryName { get; set; }
        public int state { get; set; }
        public DateTime createTime { get; set; }
    }
}
