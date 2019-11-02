
using Dapper;
using LotteryWebAPI.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotteryWebAPI.Helper
{
    class DapperHelper
    {
        private static string constr = "server = localhost;User Id = root;password = hang3944;Database = wx";
        //private static IDbConnection connection = new MySqlConnection(constr);        

        public static LotteryResult getRstByNo(string lottery,string no)
        {
            using (IDbConnection connection = new MySqlConnection(constr))
            {
                try
                {
                    string sql = string.Format("select * from lottery l, lotteryresult lr where l.lotteryID = lr.lotteryID and l.LotteryName = '{0}' and lr.No like '%{1}'", lottery, no);
                    return connection.Query<LotteryResult>(sql).First();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
                
        }        

        
    }
}
