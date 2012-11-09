using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using System.Windows;

namespace anchor
{
    [Serializable]
    class Entry
    {

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr value);

        public string Name{get; set;}

        public string Path { get; set; }

        public ImageSource GetFavicon
        {
            get
            {
                var bitmap = Favicon.fetch(this.Path, this.Name + ".dev");
                if (bitmap != null)
                {
                    IntPtr bmpPt = bitmap.GetHbitmap();
                    BitmapSource bitmapSource =
                     System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                           bmpPt,
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());

                    //freeze bitmapSource and clear memory to avoid memory leaks
                    bitmapSource.Freeze();
                    DeleteObject(bmpPt);

                    return bitmapSource;
                }
                else
                {
                    return new BitmapImage(new Uri(@"pack://application:,,,/anchor;component/Assets/application.png"));
                }
            }
        }

    }
}
