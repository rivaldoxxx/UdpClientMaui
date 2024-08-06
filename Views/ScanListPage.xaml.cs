using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using UdpClientMaui.Models;

namespace UdpClientMaui
{
    public partial class ScanListPage : ContentPage
    {
        private ObservableCollection<Product> Products { get; set; }
        private ObservableCollection<string> xamlFiles;
        private UdpClient client;
        private IPEndPoint remoteEP;

        public ScanListPage()
        {
            InitializeComponent();
            LoadXamlFiles();
            StartUdpListener();
        }

        private void StartUdpListener()
        {
            int port = 12001; 
            client = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            remoteEP = new IPEndPoint(IPAddress.Any, port);

            try
            {
                Console.WriteLine($"Klient nas³uchuje na porcie {port}");
                ReceiveMessages();
            }
            catch (Exception e)
            {
                Console.WriteLine("Wyst¹pi³ b³¹d podczas uruchamiania nas³uchiwania: " + e.ToString());
            }
        }

        private void ReceiveMessages()
        {
            Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        Console.WriteLine("Oczekiwanie na wiadomoœæ...");
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
                            case "ProductsPlaced":
                                Dispatcher.Dispatch(() => HandleProductsPlaced());
                                break;
                            default:
                                Console.WriteLine($"Nieznany komunikat: {message.Command}");
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Wyst¹pi³ b³¹d podczas odbierania wiadomoœci: " + e.ToString());
                }
            });
        }

        private async void HandleAllProductsConfirmed()
        {
            await Navigation.PopAsync();
        }

        private async void HandleProductsPlaced()
        {
            await DisplayAlert("Potwierdzenie", "Magazynier potwierdzi³ umieszczenie produktów w wózku.", "OK");
            // trzeba dodac jakas logike???

        }

        private async void OnOption1Clicked(object sender, EventArgs e)
        {
            await SendOptionOneMessage();
            await Navigation.PushAsync(new ProductListPage());
        }

        private async void OnOption2Clicked(object sender, EventArgs e)
        {
            var products = new List<Product>
            {
                new Product { Name = "Produkt 1", ImageFileName = "produkt1.jpg" },
                new Product { Name = "Produkt 2", ImageFileName = "produkt2.jpg" },
                new Product { Name = "Produkt 3", ImageFileName = "produkt3.jpg" },
                new Product { Name = "Produkt 4", ImageFileName = "produkt4.jpg" },
                new Product { Name = "Produkt 5", ImageFileName = "produkt5.jpg" }
            };

            await Navigation.PushAsync(new SingleProductPage(products, 0));
        }

        private async void OnShowHelloWorldClicked(object sender, EventArgs e)
        {
            await ShowHelloWorld();
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

                    Console.WriteLine("Wys³ano wiadomoœæ: " + messageJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("B³¹d wysy³ania wiadomoœci: " + ex.Message);
                }
            }
        }

        private void LoadXamlFiles()
        {
            string baseDirectory = AppContext.BaseDirectory; 
            string xamlDirectory = Path.Combine(baseDirectory, "XamlFiles");


            if (Directory.Exists(xamlDirectory))
            {
                var files = Directory.GetFiles(xamlDirectory, "*.xaml");
                xamlFiles = new ObservableCollection<string>(files.Select(Path.GetFileName));
                XamlFilesListView.ItemsSource = xamlFiles;
            }
            else
            {
                DisplayAlert("B³¹d", "Katalog z plikami XAML nie istnieje.", "OK");
            }
        }

        private async void OnSendXamlClicked(object sender, EventArgs e)
        {
            XamlFilesListView.IsVisible = true;
        }

        private async void OnXamlFileSelected(object sender, SelectedItemChangedEventArgs e)
        {
            string selectedFile = e.SelectedItem as string;
            if (selectedFile != null)
            {
                try
                {
                    string fileContent = ReadXamlFileContent(selectedFile);
                    await SendXamlFileContent(fileContent);
                }
                catch (FileNotFoundException ex)
                {
                    await DisplayAlert("B³¹d", ex.Message, "OK");
                }

                XamlFilesListView.SelectedItem = null;
                XamlFilesListView.IsVisible = false;
            }
        }

        private string ReadXamlFileContent(string fileName)
        {
          
            string baseDirectory = AppContext.BaseDirectory;
            string xamlDirectory = Path.Combine(baseDirectory, "XamlFiles");
            string filePath = Path.Combine(xamlDirectory, fileName);

            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                throw new FileNotFoundException("Nie znaleziono pliku: " + fileName);
            }
        }

        private async Task SendXamlFileContent(string fileContent)
        {
            string serverAddress = "127.0.0.1"; // adres IP serwera
            int serverPort = 11000;

            using (UdpClient client = new UdpClient())
            {
                try
                {
                    var xamlMessage = new Message
                    {
                        Command = "SendXaml",
                        Data = fileContent
                    };

                    string messageJson = JsonSerializer.Serialize(xamlMessage);
                    byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);

                    await client.SendAsync(messageBytes, messageBytes.Length, serverAddress, serverPort);

                    Console.WriteLine("Wys³ano plik XAML: " + messageJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("B³¹d wysy³ania pliku XAML: " + ex.Message);
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

                    Console.WriteLine("Wys³ano wiadomoœæ: " + messageJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("B³¹d wysy³ania wiadomoœci: " + ex.Message);
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

                    Console.WriteLine("Wys³ano wiadomoœæ: " + messageJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("B³¹d wysy³ania wiadomoœci: " + ex.Message);
                }
            }
        }

        public async Task SendOptionOneMessage()
        {
            string serverAddress = "127.0.0.1"; // adres IP serwera
            int serverPort = 11000;

            using (UdpClient client = new UdpClient())
            {
                try
                {
                    var optionOneMessage = new Message
                    {
                        Command = "OptionOne",
                        Data = "OpenProductListView"
                    };

                    string messageJson = JsonSerializer.Serialize(optionOneMessage);
                    byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);

                    await client.SendAsync(messageBytes, messageBytes.Length, serverAddress, serverPort);

                    Console.WriteLine("Wys³ano wiadomoœæ: " + messageJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("B³¹d wysy³ania wiadomoœci: " + ex.Message);
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
