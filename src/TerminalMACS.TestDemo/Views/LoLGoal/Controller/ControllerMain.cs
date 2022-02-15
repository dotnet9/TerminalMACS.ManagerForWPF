using TerminalMACS.TestDemo.Views.LoLGoal.API;
using TerminalMACS.TestDemo.Views.LoLGoal.Utils;

namespace TerminalMACS.TestDemo.Views.LoLGoal.Controller;

public class ControllerMain
{
    public bool GetSummoner(string summonerName)
    {
        var summoner_V4 = new Summoner_V4(Constants.Region);

        var summoner = summoner_V4.GetSummonerByName(summonerName);

        Constants.Summoner = summoner;

        return summoner != null;
    }
}