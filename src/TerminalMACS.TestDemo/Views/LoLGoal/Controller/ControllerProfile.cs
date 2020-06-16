using System;
using System.Linq;
using TerminalMACS.TestDemo.Views.LoLGoal.API;
using TerminalMACS.TestDemo.Views.LoLGoal.Model;
using TerminalMACS.TestDemo.Views.LoLGoal.Utils;
using TerminalMACS.TestDemo.Views.LoLGoal.Views;
using TerminalMACS.TestDemo.Views.LoLGoal.Views.ViewModels;

namespace TerminalMACS.TestDemo.Views.LoLGoal.Controller
{
    public class ControllerProfile
    {
        public object GetContext()
        {
            var summoner = Constants.Summoner;
            var position = GetPosition(summoner);

            return new ViewModelProfile(summoner.Name, summoner.ProfileIconId, summoner.SummonerLevel, position.Tier, position.Rank,
                position.Wins, position.Losses);
        }

        private PositionDTO GetPosition(SummonerDTO summoner)
        {
            League_V4 league = new League_V4(Constants.Region);

            var position = league.GetPositions(summoner.Id).Where(p => p.QueueType.Equals("RANKED_SOLO_5x5")).FirstOrDefault();

            return position ?? new PositionDTO();
        }

        public void OpenMain()
        {
            MainWindow profile = new MainWindow();
            profile.Show();
        }
    }
}
