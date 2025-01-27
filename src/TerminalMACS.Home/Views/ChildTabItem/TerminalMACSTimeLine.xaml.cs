using System.Collections.ObjectModel;
using System.Windows.Controls;
using WPFXmlTranslator;

namespace TerminalMACS.Home.Views.ChildTabItem;

public partial class TerminalMACSTimeLine : UserControl
{
    public TerminalMACSTimeLine()
    {
        InitializeComponent();

        #region timeline

        var listTimeLine = new ObservableCollection<Tuple<int, string, string>>();
        listTimeLine.Add(new Tuple<int, string, string>(7, "2020-04-25",
            I18nManager.Instance.GetResource(Localization.TimeLine.AddNotifyIcon).ToString()));
        listTimeLine.Add(new Tuple<int, string, string>(7, "2020-04-20",
            I18nManager.Instance.GetResource(Localization.TimeLine.WPFManagerStructureOver).ToString()));
        listTimeLine.Add(new Tuple<int, string, string>(6, "2020-04-14",
            I18nManager.Instance.GetResource(Localization.TimeLine.AddWPFManagerProject).ToString()));
        listTimeLine.Add(new Tuple<int, string, string>(5, "2020-04-13",
            I18nManager.Instance.GetResource(Localization.TimeLine.AddAbpVNextSeverProject).ToString()));
        listTimeLine.Add(new Tuple<int, string, string>(4, "2020-04-03",
            I18nManager.Instance.GetResource(Localization.TimeLine.ClientAppVersion1).ToString()));
        listTimeLine.Add(new Tuple<int, string, string>(3, "2020-03-31",
            I18nManager.Instance.GetResource(Localization.TimeLine.ClientAppBaseInfo).ToString()));
        listTimeLine.Add(new Tuple<int, string, string>(2, "2020-03-28",
            I18nManager.Instance.GetResource(Localization.TimeLine.ClientAppCompleteContactRead).ToString()));
        listTimeLine.Add(new Tuple<int, string, string>(1, "2020-03-21",
            I18nManager.Instance.GetResource(Localization.TimeLine.AddEmptyClientApp).ToString()));
        listTimeLine.Add(new Tuple<int, string, string>(0, "2020-03-16",
            I18nManager.Instance.GetResource(Localization.TimeLine.InitializeProject).ToString()));
        AduTimeLine.ItemsSource = listTimeLine;

        #endregion
    }
}