using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapCore.Model
{
    class LotteryResult
    {
        public int lotteryID { get; set; }
        public int lotteryType { get; set; }
        public string time { get; set; }
        public string No { get; set; }
        public string Number { get; set; }
        public string guanYaHe { get; set; }
        public string longHu { get; set; }
        public bool isRight { get; set; }
    }
}
