using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LotteryWebAPI.Helper;
using LotteryWebAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace LotteryWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 根据彩种和期数获取开奖结果
        /// </summary>
        /// <param name="lottery">彩种</param>
        /// <param name="no">期数</param>
        /// <returns></returns>
        [HttpGet("{lottery}/{no}")]
        public ActionResult<LotteryResult> Get(string lottery,string no)
        {
            LotteryResult lr = DapperHelper.getRstByNo(lottery, no.Substring(no.Length - 6, 6));
            return lr;
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody] string value)
        {
            return value;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
