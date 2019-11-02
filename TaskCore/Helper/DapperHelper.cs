using TaskCore.Model;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskCore.Helper
{
    class DapperHelper
    {
        private static string constr = "server = localhost;User Id = root;password = hang3944;Database = wx";
        //private static IDbConnection connection = new MySqlConnection(constr);

        public static List<Lottery> getLotterys()
        {
            using (IDbConnection connection = new MySqlConnection(constr))
            {
                try
                {
                    return connection.Query<Lottery>("select * from lottery").ToList();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
                
        }

        public static Lottery getLotteryByID(int id)
        {
            using (IDbConnection connection = new MySqlConnection(constr))
            {
                try
                {
                    return connection.Query<Lottery>("select * from lottery where lotteryID=" + id).First();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
                
        }

        public static int setTask(LotteryTask task)
        {
            using (IDbConnection connection = new MySqlConnection(constr))
            {
                string sql = @" INSERT INTO task(lotteryID,website,lotteryType,lotteryName,state,createTime)
             VALUES(@lotteryID,@website,@lotteryType,@lotteryName,@state,@createTime); ";
                int result = connection.Execute(sql, task); //直接传送list对象
                return result;
            }
            
                                      
        }

        public static int setTask(List<LotteryTask> tasks)
        {
            using (IDbConnection connection = new MySqlConnection(constr))
            {
                string sql = @" INSERT INTO task(lotteryID,website,lotteryType,lotteryName,state,createTime)
            VALUES(@lotteryID,@website,@lotteryType,@lotteryName,@state,@createTime); ";
                int result = connection.Execute(sql, tasks); //直接传送list对象
                return result;
            }        
        }



        public static int setTaskState(LotteryTask t)
        {
            using (IDbConnection connection = new MySqlConnection(constr))
            {
                return connection.Execute("update task set state=@state where TaskID=@TaskID", t);
            }
                
        }

        
    }
}
