using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anchor.Drivers
{
    abstract class Driver
    {
        public string ApacheRestartCommand { get; set; }

        public string ApacheConfigPath { get; set; }
    }
}
