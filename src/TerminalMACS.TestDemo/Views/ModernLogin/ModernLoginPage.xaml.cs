using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TerminalMACS.TestDemo.Views.ModernLogin;

public partial class ModernLoginPage : Window
{
    public ModernLoginPage()
    {
        InitializeComponent();
    }

    private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
    {
        txtEmail.Focus();
    }

    private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
        {
            textEmail.Visibility = Visibility.Collapsed;
        }
        else
        {
            textEmail.Visibility = Visibility.Visible;
        }
    }

    private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
    {
        txtPassword.Focus();
    }

    private void txtPassword_TextChanged(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
        {
            textPassword.Visibility = Visibility.Collapsed;
        }
        else
        {
            textPassword.Visibility = Visibility.Visible;
        }
    }

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }

    private void login_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(txtPassword.Password))
        {
            MessageBox.Show("登录成功");
        }
    }

    private void close_MouseUp(object sender, MouseButtonEventArgs e)
    {
        this.Close();
    }
}