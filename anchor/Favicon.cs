using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Net;
using IconImage = System.Drawing.Icon;

namespace anchor
{
    class Favicon
    {
        public static Bitmap fetch(string path, string host)
        {
            string file = Path.Combine(path, "favicon.ico");
            if (File.Exists(file))
            {
                IconImage ico = IconImage.ExtractAssociatedIcon(file);

                MemoryStream strm = new MemoryStream();
                ico.Save(strm);
                return new Bitmap(strm);
            }
            else
            {
                try
                {
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://" + host + "/favicon.ico");
                    // set the method to GET to get the image
                    myRequest.Method = "GET";
                    // get the response from the webpage
                    HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                    // create a bitmap from the stream of the response
                    Bitmap bmp = new Bitmap(myResponse.GetResponseStream());
                    // close off the stream and the response
                    myResponse.Close();
                    // return the Bitmap of the image
                    return bmp;
                }
                catch (Exception ex)
                {
                    Bitmap bitmap = new Bitmap(16, 16);
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.FillRectangle(Brushes.White, new Rectangle(0, 0, 16, 16));
                    }
                    return bitmap;
                }
            }
        }
    }
}
