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
        public ObservableCollection<Product> Products { get; set; }
        public ProductsViewModel ViewModel { get; set; } 

        public ProductListPage()
        {
            InitializeComponent();
            Products = new ObservableCollection<Product>
            {
                new Product { Name = "Produkt 1", ImageFileName = "produkt1.jpg" },
                new Product { Name = "Produkt 2", ImageFileName = "produkt2.jpg" },
                new Product { Name = "Produkt 3", ImageFileName = "produkt3.jpg" },
                new Product { Name = "Produkt 4", ImageFileName = "produkt4.jpg" },
                new Product { Name = "Produkt 5", ImageFileName = "produkt5.jpg" }
            };
            ViewModel = new ProductsViewModel(); // Inicjalizuj ViewModel
            Products = ViewModel.Products; // Przypisz produkty z ViewModel do lokalnej kolekcji
            ProductListView.ItemsSource = Products;
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

        public async void HandleProductAssignedToSlot(string productName, int slot)
        {
            var product = Products.FirstOrDefault(p => p.Name == productName);
            if (product != null)
            {
                product.SlotInfo = $"Przypisany do slotu {slot}";
            }
            await Task.CompletedTask;
        }

        private async void OnShowCartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartPage());
        }

        private void OnProductTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }



        private async void OnScanClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Product product = button?.CommandParameter as Product;

            if (product != null)
            {
                var random = new Random();
                var occupiedSlots = ViewModel.CartItems.Where(c => !string.IsNullOrEmpty(c.ProductName)).Select(c => c.Slot).ToList();
                var availableSlots = Enumerable.Range(1, 16).Except(occupiedSlots).ToList();

                if (availableSlots.Count < 2)
                {
                    await DisplayAlert("B³¹d", "Nie ma wystarczaj¹cej liczby dostêpnych miejsc w wózku.", "OK");
                    return;
                }

                int slot1 = availableSlots[random.Next(availableSlots.Count)];
                availableSlots.Remove(slot1);
                int slot2 = availableSlots[random.Next(availableSlots.Count)];

                product.SlotInfo = $"Miejsce {slot1} i {slot2}";

                ViewModel.AddProductToCart(product, new int[] { slot1, slot2 });

                await SendProductSlotToServer(product, new int[] { slot1, slot2 });
            }
        }


        private async Task SendProductSlotToServer(Product product, int[] slots)
        {
            string serverAddress = "127.0.0.1";
            int serverPort = 11000;

            using (UdpClient client = new UdpClient())
            {
                var slotMessage = new Message
                {
                    Command = "AssignSlot",
                    Data = new { ProductName = product.Name, Slots = slots }
                };

                string messageJson = JsonSerializer.Serialize(slotMessage);
                byte[] messageBytes = Encoding.ASCII.GetBytes(messageJson);

                await client.SendAsync(messageBytes, messageBytes.Length, serverAddress, serverPort);
            }
        }
    }
}
