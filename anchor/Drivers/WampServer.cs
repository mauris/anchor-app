using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IniParser;

namespace anchor.Drivers
{
    class WampServer : Driver
    {
        public WampServer(string path)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.LoadFile(Path.Combine(path, "wampmanager.conf"));
            string version = data["apache"]["apacheVersion"];
            version = version.Trim('"', '\'');
            this.ApacheRestartCommand = Path.Combine(path, "bin\\apache\\apache" + version + "\\bin\\httpd.exe") + " -k graceful";
            this.ApacheConfigPath = Path.Combine(path, "conf\\");
        }
    }
}
