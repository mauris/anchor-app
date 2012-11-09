using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Navigation;

namespace anchor
{
    class Favicon
    {
        public static Bitmap fetch(string path, string host)
        {
            string file = Path.Combine(path, "favicon.ico");
            if (File.Exists(file))
            {
                Icon ico = Icon.ExtractAssociatedIcon(file);
                return ico.ToBitmap();
            }
            return null;
        }

    }
}
