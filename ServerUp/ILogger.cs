using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerUp
{
    interface ILogger
    {
        string FileName { get; set; }
        bool Enable { get; set; }
        void write(string str_log);
    }
}
