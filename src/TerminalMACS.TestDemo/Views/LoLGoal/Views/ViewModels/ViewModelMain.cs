using System.ComponentModel;
using TerminalMACS.TestDemo.Views.LoLGoal.Utils;

namespace TerminalMACS.TestDemo.Views.LoLGoal.Views.ViewModels;

public class ViewModelMain : INotifyPropertyChanged
{
    private string region;

    private string summonerName;

    public string Region
    {
        get => region;
        set
        {
            region = value;
            Constants.Region = value;
            NotifyPropertyChanged("Region");
        }
    }

    public string SummonerName
    {
        get => summonerName;
        set
        {
            summonerName = value;
            NotifyPropertyChanged("SummonerName");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string info)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(info));
    }
}