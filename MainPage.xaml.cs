using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace UdpClientMaui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSendHelloClicked(object sender, EventArgs e)
        {
            string serverAddress = "127.0.0.1"; // adres IP serwera
            int serverPort = 11000;

            using (UdpClient client = new UdpClient())
            {
                try
                {
                    // Wysyłanie wiadomości do serwera
                    var message = new Message { Content = "Hello from client", Timestamp = DateTime.Now };
                    string messageJson = JsonSerializer.Serialize(message);
                    byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);
                    await client.SendAsync(messageBytes, messageBytes.Length, serverAddress, serverPort);

                    // Odbieranie odpowiedzi od serwera
                    UdpReceiveResult result = await client.ReceiveAsync();
                    string responseJson = Encoding.ASCII.GetString(result.Buffer);

                    // Deserializacja odpowiedzi
                    var responseMessage = JsonSerializer.Deserialize<Message>(responseJson);
                    ResponseLabel.Text = $"Otrzymano odpowiedź: {responseMessage.Content}, Timestamp: {responseMessage.Timestamp}";
                }
                catch (Exception ex)
                {
                    ResponseLabel.Text = "Błąd: " + ex.Message;
                }
            }
        }
    }

    public class Message
    {
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
