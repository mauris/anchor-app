using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace anchor.Drivers
{
    abstract class Driver
    {
        public string ApacheConfigPath { get; set; }

        public string ServerRootPath { get; set; }

        public abstract void restartServer();
    }
}
