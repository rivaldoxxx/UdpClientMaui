/*using Microsoft.Maui.Controls;
using System.Text.Json;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System;

namespace UdpClientMaui
{
    public partial class FrankeLogin1 : ContentPage
    {
        public FrankeLogin1()
        {
            InitializeComponent();
        }

        private async void OnStartButtonClicked(object sender, EventArgs e)
        {
            var message = new ButtonClickedMessage { Cmd = "buttonClicked", ButtonName = "Start" };
            string messageJson = JsonSerializer.Serialize(message);
            byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);
            await MainPage.Instance.client.SendAsync(messageBytes, messageBytes.Length, MainPage.Instance.remoteEndPoint);
        }
    }
}
*/