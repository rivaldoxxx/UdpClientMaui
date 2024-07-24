using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using UdpClientMaui.Models;

namespace UdpClientMaui
{
    public partial class ProductListPage : ContentPage
    {
        private ObservableCollection<Product> products;

        public ProductListPage()
        {
            InitializeComponent();
            products = new ObservableCollection<Product>
            {
                new Product { Name = "Produkt 1", ImageFileName = "produkt1.jpg" },
                new Product { Name = "Produkt 2", ImageFileName = "produkt2.jpg" },
                new Product { Name = "Produkt 3", ImageFileName = "produkt3.jpg" },
                new Product { Name = "Produkt 4", ImageFileName = "produkt4.jpg" },
                new Product { Name = "Produkt 5", ImageFileName = "produkt5.jpg" }
            };
            ProductListView.ItemsSource = products;
        }

        private async void OnConfirmClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Product product = button?.CommandParameter as Product;

            if (product != null)
            {
                // Wysy³anie informacji o potwierdzeniu produktu do serwera
                await SendProductConfirmationToServer(product);
            }
        }

        private async Task SendProductConfirmationToServer(Product product)
        {
            string serverAddress = "127.0.0.1";
            int serverPort = 11000;

            using (UdpClient client = new UdpClient())
            {
                var confirmMessage = new Message
                {
                    Command = "RequestProductConfirmation",
                    Data = product.Name
                };

                string messageJson = JsonSerializer.Serialize(confirmMessage);
                byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);

                await client.SendAsync(messageBytes, messageBytes.Length, serverAddress, serverPort);
            }
        }

        private void OnProductTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        public class Message
        {
            public string Command { get; set; }
            public object Data { get; set; }
        }
    }
}
