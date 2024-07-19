using Microsoft.Maui.Controls;

namespace UdpClientMaui
{
    public partial class Layout2 : ContentPage
    {
        public Layout2()
        {
            InitializeComponent();
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
