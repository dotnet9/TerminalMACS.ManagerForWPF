namespace TerminalMACS.TestDemo.Views.LoLGoal.Views.ViewModels;

public class ViewModelProfile
{
    public ViewModelProfile(string summonerName, int icon, long level, string tier, string rank, int wins, int losses)
    {
        SummonerName = summonerName;
        Icon = "http://opgg-static.akamaized.net/images/profile_icons/profileIcon" + icon + ".jpg";
        Level = level;
        Tier = tier;
        Rank = rank;
        Wins = wins;
        Losses = losses;
        Emblem = "/TerminalMACS.TestDemo;component/Views/LoLGoal/Assets/emblems/Emblem_" + tier + ".png";
    }

    public string SummonerName { get; }
    public string Icon { get; }
    public long Level { get; }
    public string Tier { get; }
    public string Rank { get; }
    public string Emblem { get; }
    public int Wins { get; }
    public int Losses { get; }
}