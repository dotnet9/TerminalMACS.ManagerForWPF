using IdentityModel.OidcClient;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Input;

namespace TerminalMACS.Views
{
    public partial class Login : Window
    {
        private OidcClient _oidcClient = null;
        public Login()
        {
            InitializeComponent();
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void LoginWithTerminal_Click(object sender, RoutedEventArgs e)
        {
            var options = new OidcClientOptions()
            {
                Authority = "https://localhost:44377",
                ClientId = "Server_App",
                Scope = "openid",
                ClientSecret = "1q2w3e*",
                RedirectUri = "http://localhost/sample-wpf-app",//"https://localhost:44387/connect/token",
                //ResponseMode = OidcClientOptions.AuthorizeResponseMode.FormPost,
                //Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                Browser = new WpfEmbeddedBrowser()
            };

            _oidcClient = new OidcClient(options);

            LoginResult result;
            try
            {
                result = await _oidcClient.LoginAsync();
            }
            catch (Exception ex)
            {
                //Message.Text = $"Unexpected Error: {ex.Message}";
                return;
            }

            if (result.IsError)
            {
                //Message.Text = result.Error == "UserCancel" ? "The sign-in window was closed before authorization was completed." : result.Error;

            }
            else
            {
                var name = result.User.Identity.Name;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                var apiResult = await client.GetStringAsync("https://localhost:44387/secret");
                //Message.Text = $"Hello {name}";
            }
        }
    }
}
