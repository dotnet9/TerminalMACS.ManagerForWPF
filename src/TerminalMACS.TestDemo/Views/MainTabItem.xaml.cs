﻿using TerminalMACS.TestDemo.Views.DrawdownMenu3;

namespace TerminalMACS.TestDemo.Views;

public partial class MainTabItem
{
    public MainTabItem(ITestService service)
    {
        InitializeComponent();
        var currentTime = service.GetCurrentTime();
        Console.WriteLine(currentTime);
    }

    private void ShowFoodLoginUI_Click(object sender, RoutedEventArgs e)
    {
        var view = new FoodAppLoginView();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowAppUseageDashboard_Click(object sender, RoutedEventArgs e)
    {
        var view = new AppUsageDashboard();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowLoginView1_Click(object sender, RoutedEventArgs e)
    {
        var view = new LoginView1();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowLoginView2_Click(object sender, RoutedEventArgs e)
    {
        var view = new LoginView2();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowMusicPalyer1_Click(object sender, RoutedEventArgs e)
    {
        var view = new MusicPlayer1();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowDashboard2_Click(object sender, RoutedEventArgs e)
    {
        var view = new Dashboard2();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowMenuChange_Click(object sender, RoutedEventArgs e)
    {
        var view = new MenuChange.MenuChange();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowAnimatedMenu_Click(object sender, RoutedEventArgs e)
    {
        var view = new AnimatedMenuView();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowInstagramRedesign_Click(object sender, RoutedEventArgs e)
    {
        var view = new InstagramRedesignView();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowLoLGoal_Click(object sender, RoutedEventArgs e)
    {
        var view = new MainWindow();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowDrawerMenu_Click(object sender, RoutedEventArgs e)
    {
        var view = new DrawerMenu();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowDrawerMenu2_Click(object sender, RoutedEventArgs e)
    {
        var view = new MenuView();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowBaiduMap_Click(object sender, RoutedEventArgs e)
    {
        var view = new BaiduMapView();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowChatView_Click(object sender, RoutedEventArgs e)
    {
        var view = new MainChatView();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowCalculator_Click(object sender, RoutedEventArgs e)
    {
        var view = new CalculatorView();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowWriteExcel_Click(object sender, RoutedEventArgs e)
    {
        var view = new TestReadWriteExcelView();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowCoreBehaviors_Click(object sender, RoutedEventArgs e)
    {
        var view = new TestBehaviorsView();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowTreeView_Click(object sender, RoutedEventArgs e)
    {
        var view = new TreeViewDemo();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowDriveStorageView_Click(object sender, RoutedEventArgs e)
    {
        var view = new DriveStorageView();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowLoginView3_Click(object sender, RoutedEventArgs e)
    {
        var view = new LoginView3();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowModernLogin_Click(object sender, RoutedEventArgs e)
    {
        var view = new ModernLoginPage();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowDashboard3_Click(object sender, RoutedEventArgs e)
    {
        var view = new Dashboard3.MainWindow();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowDashboard4_Click(object sender, RoutedEventArgs e)
    {
        var view = new WalletPayment.MainWindow();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowLogin5_Click(object sender, RoutedEventArgs e)
    {
        var view = new Login5.MainWindow();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowInstagramRedesign2_Click(object sender, RoutedEventArgs e)
    {
        var view = new ReDesignInstagramView();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }

    private void ShowDrawerMenu3_Click(object sender, RoutedEventArgs e)
    {
        var view = new DrawdownMenu3Window();
        view.Owner = Application.Current.MainWindow;
        view.Show();
    }
}