using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.Maui.Graphics;

namespace UdpClientMaui
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Product> products;
        private ObservableCollection<CartItem> cartItems;

        public ObservableCollection<Product> Products
        {
            get => products;
            set
            {
                if (products != value)
                {
                    products = value;
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

        public ObservableCollection<CartItem> CartItems
        {
            get => cartItems;
            set
            {
                if (cartItems != value)
                {
                    cartItems = value;
                    OnPropertyChanged(nameof(CartItems));
                }
            }
        }

        public ProductsViewModel()
        {
            Products = new ObservableCollection<Product>
            {
                new Product { Name = "Produkt 1", BackgroundColor = Colors.White },
                new Product { Name = "Produkt 2", BackgroundColor = Colors.White },
                new Product { Name = "Produkt 3", BackgroundColor = Colors.White },
                new Product { Name = "Produkt 4", BackgroundColor = Colors.White },
                new Product { Name = "Produkt 5", BackgroundColor = Colors.White }
            };

            CartItems = new ObservableCollection<CartItem>(Enumerable.Range(1, 16).Select(i => new CartItem { Slot = i }));
        }

        public void AddProductToCart(Product product, int[] slots)
        {
            if (slots == null || slots.Length == 0)
            {
                Console.WriteLine("Slots are null or empty.");
                return;
            }

            if (product.AssignedSlots != null)
            {
                foreach (var oldSlot in product.AssignedSlots)
                {
                    var oldCartItem = CartItems.FirstOrDefault(c => c.Slot == oldSlot);
                    if (oldCartItem != null)
                    {
                        oldCartItem.ProductName = string.Empty;
                        Console.WriteLine($"Removed product from slot {oldSlot}");
                    }
                }
            }

            foreach (var slot in slots)
            {
                var cartItem = CartItems.FirstOrDefault(c => c.Slot == slot);
                if (cartItem != null)
                {
                    cartItem.ProductName = product.Name;
                    Console.WriteLine($"Assigned product {product.Name} to slot {slot}");
                }
            }

            product.IsConfirmed = true;
            product.AssignedSlots = slots;
            OnPropertyChanged(nameof(CartItems)); 
        }


        public void ConfirmProducts()
        {
            foreach (var product in Products.Where(p => p.IsConfirmed))
            {
                product.BackgroundColor = Colors.Green;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

  

    public class Product : INotifyPropertyChanged
    {
        private string name;
        private string imageFileName;
        private string slotInfo;
        private bool isConfirmed;
        private Color backgroundColor;
        private int[] assignedSlots; // Dodana właściwość

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string ImageFileName
        {
            get => imageFileName;
            set
            {
                if (imageFileName != value)
                {
                    imageFileName = value;
                    OnPropertyChanged(nameof(ImageFileName));
                }
            }
        }

        public string SlotInfo
        {
            get => slotInfo;
            set
            {
                if (slotInfo != value)
                {
                    slotInfo = value;
                    OnPropertyChanged(nameof(SlotInfo));
                }
            }
        }

        public bool IsConfirmed
        {
            get => isConfirmed;
            set
            {
                if (isConfirmed != value)
                {
                    isConfirmed = value;
                    OnPropertyChanged(nameof(IsConfirmed));
                }
            }
        }

        public Color BackgroundColor
        {
            get => backgroundColor;
            set
            {
                if (backgroundColor != value)
                {
                    backgroundColor = value;
                    OnPropertyChanged(nameof(BackgroundColor));
                }
            }
        }

        public int[] AssignedSlots
        {
            get => assignedSlots;
            set
            {
                if (assignedSlots != value)
                {
                    assignedSlots = value;
                    OnPropertyChanged(nameof(AssignedSlots));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
