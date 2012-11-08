using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace anchor
{
    class ConfigWriter
    {
        private string path;

        public ConfigWriter(string path)
        {
            this.path = path;
        }

        public void write(string root, List<Entry> entries)
        {
            FileStream file = File.Open(this.path, FileMode.Create);
            StreamWriter writer = new StreamWriter(file);
            writer.WriteLine("NameVirtualHost 127.0.0.1");
            writer.WriteLine("<VirtualHost 127.0.0.1>");
            writer.WriteLine("    ServerName localhost");
            writer.WriteLine("    DocumentRoot '" + root + "'");
            writer.WriteLine("</VirtualHost>");
            foreach(Entry entry in entries)
            {
                writer.WriteLine("<VirtualHost 127.0.0.1>");
                writer.WriteLine("    ServerName " + entry.Name + ".dev");
                writer.WriteLine("    DocumentRoot '" + entry.Path + "'");
                writer.WriteLine("</VirtualHost>");
                writer.WriteLine("<Directory \"" + entry.Path + "\">");
                writer.WriteLine("    Deny from all");
                writer.WriteLine("    Allow from 127.0.0.1");
                writer.WriteLine("    Order Deny,Allow");
                writer.WriteLine("</Directory>");
            }
            writer.Close();
        }

    }
}
