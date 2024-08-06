using System.ComponentModel;
using Microsoft.Maui.Graphics;

namespace UdpClientMaui.Models
{
    public class Product : INotifyPropertyChanged
    {
        private string name;
        private string imageFileName;
        private string slotInfo;
        private Color backgroundColor;

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
