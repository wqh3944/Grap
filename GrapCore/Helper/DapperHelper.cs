using GrapCore.Model;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapCore.Helper
{
    class DapperHelper
    {
        private static string constr = "server = localhost;User Id = root;password = hang3944;Database = wx";
        //private static IDbConnection connection = new MySqlConnection(constr);

        public static LotteryTask getOneTask()
        {
            using (IDbConnection connection = new MySqlConnection(constr))
            {
                try
                {
                    return connection.Query<LotteryTask>("select * from task where state=0 for update").First();
                }
                catch (Exception e)
                {
                    return null;
                }
            }                                
        }

        public static int setTaskState(LotteryTask t)
        {
            using (IDbConnection connection = new MySqlConnection(constr))
            {
                return connection.Execute("update task set state=@state where TaskID=@TaskID", t);
            }
                        
        }

        public static int setLotteryResult(LotteryResult rst)
        {
            using (IDbConnection connection = new MySqlConnection(constr))
            {
                string sql = @" INSERT INTO lotteryresult (lotteryID,lotteryType,time,No,Number,guanYaHe,longHu,isRight)
             VALUES(@lotteryID,@lotteryType,@time,@No,@Number,@guanYaHe,@longHu,@isRight); ";
                int result = connection.Execute(sql, rst); //直接传送list对象
                return result;
            }
                
        }

        public static List<LotteryType> getLotteryType(string page)
        {
            using (IDbConnection connection = new MySqlConnection(constr))
            {
                string sql = string.Format("select * from LotteryType where website='{0}'", page);
                return connection.Query<LotteryType>(sql).ToList();
            }
                

        }

        public static List<LotteryResult> getResultByNo(string No,int lotteryType)
        {
            using (IDbConnection connection = new MySqlConnection(constr))
            {
                string sql = string.Format("select * from lotteryresult where No like '%{0}' and lotteryType={1}", No, lotteryType);
                return connection.Query<LotteryResult>(sql).ToList();
            }                
        }

        public static int setLotteryType(List<LotteryType> types)
        {
            using (IDbConnection connection = new MySqlConnection(constr))
            {
                string sql = @" INSERT INTO LotteryType (website,LotteryName,LotteryShortName)
             VALUES(@website,@LotteryName,@LotteryShortName); ";
                int result = connection.Execute(sql, types); //直接传送list对象
                return result;
            }                
        }
    }
}
