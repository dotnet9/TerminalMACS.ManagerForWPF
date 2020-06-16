using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TerminalMACS.TestDemo.Views.LoLGoal.Model;

namespace TerminalMACS.TestDemo.Views.LoLGoal.API
{
    public class League_V4 : Api
    {
        public League_V4(string region) : base(region)
        {
        }

        public List<PositionDTO> GetPositions(string summonerId)
        {
            //1、这是正常的API访问
            //string path = "league/v4/positions/by-summoner/" + summonerId;
            //var response = GET(GetURI(path));
            //string content = response.Content.ReadAsStringAsync().Result;
            //if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //{
            //    return JsonConvert.DeserializeObject<List<PositionDTO>>(content);
            //}
            //else
            //{
            //    return null;
            //}

            //2、这是模拟数据，正常访问LOL服务器，需要注册Key
            string[] tiers = { "Bronze", "Challenger", "Diamond", "Gold", "Grandmaster", "Iron", "Master", "Platinum", "Silver" };
            var rd = new Random(DateTime.Now.Millisecond);
            var lst = new List<PositionDTO>();
            for (int i = 0; i < rd.Next(5, 20); i++)
            {
                lst.Add(new PositionDTO
                {
                    Tier = tiers[rd.Next(0, tiers.Length)],
                    Rank = "IV",
                    Wins = rd.Next(2, 100),
                    Losses = rd.Next(2, 100),
                    QueueType = "RANKED_SOLO_5x5"
                });
            }
            return lst;
        }
    }
}
