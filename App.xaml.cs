using Microsoft.Maui.Controls;

namespace UdpClientMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
