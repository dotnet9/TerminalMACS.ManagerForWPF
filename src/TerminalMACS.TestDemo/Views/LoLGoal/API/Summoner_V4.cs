using System;
using TerminalMACS.TestDemo.Views.LoLGoal.Model;

namespace TerminalMACS.TestDemo.Views.LoLGoal.API;

public class Summoner_V4 : Api
{
    public Summoner_V4(string region) : base(region)
    {
    }

    public SummonerDTO GetSummonerByName(string SummonerName)
    {
        //1、这是正常的API访问
        //string path = "summoner/v4/summoners/by-name/" + SummonerName;
        //var response = GET(GetURI(path));
        //string content = response.Content.ReadAsStringAsync().Result;
        //if(response.StatusCode == System.Net.HttpStatusCode.OK)
        //{
        //    return JsonConvert.DeserializeObject<SummonerDTO>(content);
        //}
        //else
        //{
        //    return null;
        //}

        //2、这是模拟数据，正常访问LOL服务器，需要注册Key
        return new SummonerDTO
        {
            ProfileIconId = DateTime.Now.Second,
            Name = SummonerName,
            SummonerLevel = new Random(DateTime.Now.Millisecond).Next(50, 200),
            Id = DateTime.Now.Second.ToString()
        };
    }
}