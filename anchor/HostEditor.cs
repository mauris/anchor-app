using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

/*
 * HostEditor class
 * 
 * Provides functionality for editing Windows hosts file.
 */ 
namespace anchor
{
    class HostEditor
    {

        private string pathName;

        private List<HostEntry> entries = new List<HostEntry>();

        public List<HostEntry> Entries { get { return entries; } }

        private bool changed = false;

        public HostEditor(string file)
        {
            pathName = file;
            this.process();
        }

        ~HostEditor()
        {
            this.update();
        }

        public void update()
        {
            if (changed)
            {
                if (File.Exists(pathName + ".bak"))
                {
                    File.Delete(pathName + ".bak");
                }
                File.Move(pathName, pathName + ".bak");

                using (FileStream stream = File.Open(pathName, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        foreach (HostEntry entry in entries)
                        {
                            writer.WriteLine(entry.IpAddress + " " + entry.HostName + (entry.Comment != null ? " # " + entry.Comment : ""));
                        }
                        writer.Close();
                    }
                }
            }
        }

        private void process()
        {
            using(FileStream stream = File.OpenRead(pathName)){
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine().Trim();
                        if (line.Length > 1 && line.Substring(0, 1) != "#")
                        {
                            Regex regex = new Regex("([^\\s#]+)\\s+([^\\s#]+)");
                            Match match = regex.Match(line);
                            HostEntry entry = new HostEntry { IpAddress = match.Groups[1].Value, HostName = match.Groups[2].Value };
                            Regex commentRegex = new Regex("\\#(.+)$");
                            Match commentMatch = commentRegex.Match(line);
                            if (commentMatch.Length > 0)
                            {
                                entry.Comment = commentMatch.Groups[1].Value.Trim();
                            }
                            entries.Add(entry);
                        }
                    }
                    reader.Close();
                }
            }
            changed = false;
        }

        public void add(string host)
        {
            add(host, "127.0.0.1");
        }

        public void add(string host, string ipAddress)
        {
            add(host, ipAddress, "anchor-host");
        }

        public void add(string host, string ipAddress, string comment)
        {
            this.remove(host, comment);
            entries.Add(new HostEntry { HostName = host, IpAddress = ipAddress, Comment = comment });
        }

        public void remove(string host)
        {
            remove(host, "anchor-host");
        }

        public void remove(string host, string comment)
        {
            entries.RemoveAll(x => x.Comment == comment && x.HostName == host);
            changed = true;
        }

    }

    class HostEntry
    {

        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string Comment { get; set; }

    }
}
