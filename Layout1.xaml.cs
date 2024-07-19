using Microsoft.Maui.Controls;

namespace UdpClientMaui
{
    public partial class Layout1 : ContentPage
    {
        public Layout1()
        {
            InitializeComponent();
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
