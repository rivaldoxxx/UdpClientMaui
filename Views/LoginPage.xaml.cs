using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace UdpClientMaui
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                await SendLoggedInMessage();

                await Navigation.PushAsync(new ScanListPage());
            }
            else
            {
                await DisplayAlert("B³¹d", "Nieprawid³owa nazwa u¿ytkownika lub has³o", "OK");
            }
        }

        private async Task SendLoggedInMessage()
        {
            string serverAddress = "127.0.0.1";
            int serverPort = 11001;

            using (UdpClient client = new UdpClient())
            {
                var loggedInMessage = new Message
                {
                    Command = "LoggedIn",
                    Data = null
                };

                string messageJson = JsonSerializer.Serialize(loggedInMessage);
                byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);

                await client.SendAsync(messageBytes, messageBytes.Length, serverAddress, serverPort);
            }
        }
        public class Message
        {
            public string Command { get; set; }
            public object Data { get; set; }
        }

    }
}
