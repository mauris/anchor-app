using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IniParser;
using System.Diagnostics;

namespace anchor.Drivers
{
    class WampServer : Driver
    {
        private string rootPath;

        private string apacheVersion;

        public WampServer(string path)
        {
            rootPath = path;
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.LoadFile(Path.Combine(path, "wampmanager.conf"));
            string version = data["apache"]["apacheVersion"];
            apacheVersion = version.Trim('"', '\'');
            this.ApacheConfigPath = Path.Combine(path, "alias\\");
            this.ServerRootPath = Path.Combine(path, "www");
        }

        public override void restartServer()
        {
            ProcessStartInfo info = new ProcessStartInfo(Path.Combine(rootPath, "bin\\apache\\apache" + apacheVersion + "\\bin\\httpd.exe"), "-n wampapache -k restart");
            info.CreateNoWindow = true;
            Process.Start(info);
        }
    }
}
