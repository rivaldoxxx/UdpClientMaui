﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace UdpClientMaui
{
    public partial class MainPage : ContentPage
    {
        private UdpClient client;
        private IPEndPoint remoteEP;

        public MainPage()
        {
            InitializeComponent();
            // StartUdpListener();
        }

        /*private void StartUdpListener()
        {
            int port = 12000; // Nowy port nasłuchu klienta
            client = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            remoteEP = new IPEndPoint(IPAddress.Any, port);

            try
            {
                Console.WriteLine($"Klient nasłuchuje na porcie {port}");
                ReceiveMessages();
            }
            catch (Exception e)
            {
                Console.WriteLine("Wystąpił błąd podczas uruchamiania nasłuchiwania: " + e.ToString());
            }
        }*/


        /*private void ReceiveMessages()
        {
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        Console.WriteLine("Oczekiwanie na wiadomość...");
                        var result = client.Receive(ref remoteEP);
                        byte[] receivedBytes = result;
                        string receivedData = Encoding.ASCII.GetString(receivedBytes);

                        Console.WriteLine($"Odebrano dane: {receivedData}");

                        var message = JsonSerializer.Deserialize<Message>(receivedData);
                        Console.WriteLine($"Otrzymano komunikat: {message.Command}");

                        switch (message.Command)
                        {
                            case "AllProductsConfirmed":
                                Dispatcher.Dispatch(() => HandleAllProductsConfirmed());
                                break;
                            default:
                                Console.WriteLine($"Nieznany komunikat: {message.Command}");
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Wystąpił błąd podczas odbierania wiadomości: " + e.ToString());
                }
            });
        }*/

        private async void HandleAllProductsConfirmed()
        {
            await DisplayAlert("Informacja", "Wszystkie produkty zostały potwierdzone.", "OK");
            await Navigation.PopToRootAsync(); // Powrót do strony głównej
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

        public async Task ShowHelloWorld()
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

        public async Task SendXamlFile(string filePath)
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

        public async Task ShowDialog(string dialogMessage)
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

        public async Task ShowError(string errorMessage)
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
