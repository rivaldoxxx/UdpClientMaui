using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace UdpClientMaui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnShowHelloWorldClicked(object sender, EventArgs e)
        {
            await ShowHelloWorld();
        }

        private async void OnSendXamlClicked(object sender, EventArgs e)
        {
            string filePath = "C:\\Users\\fizyk\\source\\repos\\UdpClientMaui\\ExampleLayout.xaml";
            await SendXamlFile(filePath);
        }

        private async void OnShowDialogClicked(object sender, EventArgs e)
        {
            await ShowDialog("This is a dialog message.");
        }

        private async void OnShowErrorClicked(object sender, EventArgs e)
        {
            await ShowError("This is an error message.");
        }

        private async Task ShowHelloWorld()
        {
            string serverAddress = "127.0.0.1"; // adres IP serwera
            int serverPort = 11000;

            using (UdpClient client = new UdpClient())
            {
                try
                {
                    var showHelloWorldMessage = new Message
                    {
                        Command = "ShowHelloWorld",
                        Data = "Hello, World!"
                    };

                    string messageJson = JsonSerializer.Serialize(showHelloWorldMessage);
                    byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);

                    await client.SendAsync(messageBytes, messageBytes.Length, serverAddress, serverPort);

                    ResponseLabel.Text = "Wysłano wiadomość Hello, World! do serwera";
                    Console.WriteLine("Wysłano wiadomość: " + messageJson);
                }
                catch (Exception ex)
                {
                    ResponseLabel.Text = "Błąd: " + ex.Message;
                    Console.WriteLine("Błąd wysyłania wiadomości: " + ex.Message);
                }
            }
        }

        private async Task SendXamlFile(string filePath)
        {
            string serverAddress = "127.0.0.1"; // adres IP serwera
            int serverPort = 11000;

            using (UdpClient client = new UdpClient())
            {
                try
                {
                    string fileContent = File.ReadAllText(filePath);
                    var xamlMessage = new Message
                    {
                        Command = "SendXaml",
                        Data = fileContent
                    };

                    string messageJson = JsonSerializer.Serialize(xamlMessage);
                    byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);

                    await client.SendAsync(messageBytes, messageBytes.Length, serverAddress, serverPort);

                    ResponseLabel.Text = "Wysłano plik XAML do serwera";
                    Console.WriteLine("Wysłano plik XAML: " + messageJson);
                }
                catch (Exception ex)
                {
                    ResponseLabel.Text = "Błąd: " + ex.Message;
                    Console.WriteLine("Błąd wysyłania pliku XAML: " + ex.Message);
                }
            }
        }

        private async Task ShowDialog(string dialogMessage)
        {
            string serverAddress = "127.0.0.1"; // adres IP serwera
            int serverPort = 11000;

            using (UdpClient client = new UdpClient())
            {
                try
                {
                    var dialogMessageObj = new Message
                    {
                        Command = "ShowDialog",
                        Data = dialogMessage
                    };

                    string messageJson = JsonSerializer.Serialize(dialogMessageObj);
                    byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);

                    await client.SendAsync(messageBytes, messageBytes.Length, serverAddress, serverPort);

                    ResponseLabel.Text = "Wysłano wiadomość ShowDialog do serwera";
                    Console.WriteLine("Wysłano wiadomość: " + messageJson);
                }
                catch (Exception ex)
                {
                    ResponseLabel.Text = "Błąd: " + ex.Message;

                    Console.WriteLine("Błąd wysyłania wiadomości: " + ex.Message);
                }
            }
        }

        private async Task ShowError(string errorMessage)
        {
            string serverAddress = "127.0.0.1"; // adres IP serwera
            int serverPort = 11000;

            using (UdpClient client = new UdpClient())
            {
                try
                {
                    var errorMessageObj = new Message
                    {
                        Command = "ShowError",
                        Data = errorMessage
                    };

                    string messageJson = JsonSerializer.Serialize(errorMessageObj);
                    byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);

                    await client.SendAsync(messageBytes, messageBytes.Length, serverAddress, serverPort);

                    ResponseLabel.Text = "Wysłano wiadomość ShowError do serwera";
                    Console.WriteLine("Wysłano wiadomość: " + messageJson);
                }
                catch (Exception ex)
                {
                    ResponseLabel.Text = "Błąd: " + ex.Message;
                    Console.WriteLine("Błąd wysyłania wiadomości: " + ex.Message);
                }
            }
        }

        public class Message
        {
            public string Command { get; set; }
            public object Data { get; set; }
        }
    }
}
