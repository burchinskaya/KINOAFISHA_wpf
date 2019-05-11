﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace KINOwpf
{
    /// <summary>
    /// Логика взаимодействия для Ticket.xaml
    /// </summary>
    public partial class Ticket : Window
    {
        public User user;
        public string dir, number;
        public string path;
        public Ticket(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap(498, 424, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(this);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using (KinoContext db = new KinoContext())
            {
                using (Stream stm = File.Create(path + "\\" + dir + "\\Билет №" + number + ".png"))
                    encoder.Save(stm);
            }
            this.Close();
        }

        public static void SaveToPNG(FrameworkElement frameworkElement, Size size, string fileName)
        {
            using (FileStream stream = new FileStream(string.Format("{0}.png", "KINOAFISHA tickets " + DateTime.Now.ToString("dd/mm/yyyy hh:mm")), FileMode.Create))
            {
                SaveToPNG(frameworkElement, size, stream);
            }
        }

        public static void SaveToPNG(FrameworkElement frameworkElement, Size size, Stream stream)
        {
            Transform transform = frameworkElement.LayoutTransform;
            frameworkElement.LayoutTransform = null;
            Thickness margin = frameworkElement.Margin;
            frameworkElement.Margin = new Thickness(0, 0, margin.Right - margin.Left, margin.Bottom - margin.Top);
            frameworkElement.Measure(size);
            frameworkElement.Arrange(new Rect(size));
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(frameworkElement);
            frameworkElement.LayoutTransform = transform;
            frameworkElement.Margin = margin;
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Interlace = PngInterlaceOption.On;
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(stream);
        }
    }
}
