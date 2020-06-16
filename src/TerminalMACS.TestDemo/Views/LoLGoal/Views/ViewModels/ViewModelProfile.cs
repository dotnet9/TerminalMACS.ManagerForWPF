namespace TerminalMACS.TestDemo.Views.LoLGoal.Views.ViewModels
{
    public class ViewModelProfile
    {
        public string SummonerName { get; private set; }
        public string Icon { get; private set; }
        public long Level { get; private set; }
        public string Tier { get; private set; }
        public string Rank { get; private set; }
        public string Emblem { get; private set; }
        public int Wins { get; private set; }
        public int Losses { get; private set; }

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
    }
}
