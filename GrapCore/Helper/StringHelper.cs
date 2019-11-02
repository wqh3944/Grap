using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GrapCore.Helper
{
    class StringHelper
    {
        public static string trimEnd(string ori)
        {            
            return Regex.Replace(ori, @"[\n\r]", "").Trim(' ').TrimStart('0');
        }
    }
}
