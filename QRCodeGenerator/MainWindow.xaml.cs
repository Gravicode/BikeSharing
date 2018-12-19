using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing;
using ZXing.QrCode;

namespace QRCodeGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Bitmap GeneratedQR { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            BtnGenerate.Click += BtnGenerate_Click;
            BtnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (GeneratedQR != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Jpeg (*.jpg)|*.jpg";
                if (saveFileDialog.ShowDialog() == true)
                    GeneratedQR.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
            }
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            QrCodeEncodingOptions options = new QrCodeEncodingOptions();
            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = 400,
                Height = 400,
            };
            var writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            if (String.IsNullOrWhiteSpace(TxtQrCode.Text) || String.IsNullOrEmpty(TxtQrCode.Text))
            {
                GeneratedQR = null;
                ImgQR.Source = null;
                MessageBox.Show("Text not found", "Oops!",  MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else
            {
                var qr = new ZXing.BarcodeWriter();
                qr.Options = options;
                qr.Format = ZXing.BarcodeFormat.QR_CODE;
                var result = new Bitmap(qr.Write(TxtQrCode.Text.Trim()));
                GeneratedQR = result;
                ImgQR.Source = BitmapToImageSource(result);
                //TxtQrCode.Clear();
            }
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
