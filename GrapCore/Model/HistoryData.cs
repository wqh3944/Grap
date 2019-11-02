using System;
using System.Collections.Generic;
using System.Text;

namespace GrapCore.Model
{
    class HistoryData
    {
        public DataInfo data { get; set; }
    }

    class DataInfo
    {
        public List<History> history { get; set;  }
    }

    class History
    {
        public string timeDraw { get; set; }
        public string todayTime { get; set; }
        public int qishu { get; set; }
        public int[] nums { get; set; }
        public string SumBigSmall { get; set; }
        public string SumOddEven { get; set; }
        public string DragonTiger1 { get; set; }
        public string DragonTiger2 { get; set; }
        public string DragonTiger3 { get; set; }
        public string DragonTiger4 { get; set; }
        public string DragonTiger5 { get; set; }
    }
}
