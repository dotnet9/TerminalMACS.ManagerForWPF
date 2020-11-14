using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickApp.Helpers
{
    public class JsonHelper
    {
        #region 反序列化
        /// <summary>
        /// 将Json报文反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象泛型</typeparam>
        /// <param name="strJson">JSON报文</param>
        /// <returns></returns>
        public static T Deserialize<T>(string strJson)
        {
            T t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(strJson);
            return t;
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 将对象序列化为Json报文
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static string QueryAString(string retString, string jsonKey)
        {
            JObject jobject = JObject.Parse(retString);
            return jobject[jsonKey].ToString();
        }

        #endregion
    }
}