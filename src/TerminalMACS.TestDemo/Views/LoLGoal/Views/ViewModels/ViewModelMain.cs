using System.ComponentModel;
using TerminalMACS.TestDemo.Views.LoLGoal.Utils;

namespace TerminalMACS.TestDemo.Views.LoLGoal.Views.ViewModels
{
    public class ViewModelMain : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        string region;
        public string Region
        {
            get { return region; }
            set { region = value; Constants.Region = value; NotifyPropertyChanged("Region"); }
        }

        string summonerName;
        public string SummonerName
        {
            get { return summonerName; }
            set { summonerName = value; NotifyPropertyChanged("SummonerName"); }
        }

    }
}
