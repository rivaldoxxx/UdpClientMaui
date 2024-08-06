using System.Collections.ObjectModel;

namespace UdpClientMaui
{
    public partial class CartPage : ContentPage
    {
        public ObservableCollection<CartItem> CartItems { get; set; }

        public CartPage()
        {
            InitializeComponent();
            CartItems = new ObservableCollection<CartItem>(Enumerable.Range(1, 16).Select(i => new CartItem { Slot = i }));
            BindingContext = this;
        }

        public void UpdateCart(string productName, int slot)
        {
            var cartItem = CartItems.FirstOrDefault(c => c.Slot == slot);
            if (cartItem != null)
            {
                cartItem.ProductName = productName;
            }
        }
    }

    public class CartItem
    {
        public int Slot { get; set; }
        public string ProductName { get; set; }
    }
}
