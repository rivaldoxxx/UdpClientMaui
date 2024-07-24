using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Controls;
using UdpClientMaui.Models; // Dodaj w³aœciwy namespace

namespace UdpClientMaui
{
    public partial class SingleProductPage : ContentPage
    {
        private List<Product> products;
        private int currentIndex;

        public SingleProductPage(List<Product> products, int index)
        {
            InitializeComponent();
            this.products = products;
            this.currentIndex = index;
            LoadProduct();
        }

        private void LoadProduct()
        {
            if (currentIndex < products.Count)
            {
                var product = products[currentIndex];
                ProductLabel.Text = product.Name;

                string imagesPath = Path.Combine(AppContext.BaseDirectory, "Resources", "ImagesXAML");

                Console.WriteLine($"Images path: {imagesPath}");

                if (Directory.Exists(imagesPath))
                {
                    string imagePath = Path.Combine(imagesPath, product.ImageFileName);
                    if (File.Exists(imagePath))
                    {
                        Console.WriteLine($"Loading image from: {imagePath}");
                        ProductImage.Source = ImageSource.FromFile(imagePath);
                    }
                    else
                    {
                        Console.WriteLine($"No image found for {product.ImageFileName}");
                    }
                }
                else
                {
                    Console.WriteLine("Images directory does not exist.");
                }
            }
            else
            {
                // Jeœli wszystkie produkty s¹ potwierdzone, wróæ do ScanListPage
                Navigation.PopToRootAsync();
            }
        }




        private async void OnConfirmClicked(object sender, EventArgs e)
        {
            await Task.CompletedTask;

            // PrzejdŸ do nastêpnego produktu
            currentIndex++;
            LoadProduct();
        }
    }
}
