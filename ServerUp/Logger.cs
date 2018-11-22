using System;
using System.Collections.Generic;
using System.IO;

namespace ServerUp
{
    public class Logger : ILogger
    {
        Stack<string> Massiv = new Stack<string>();
        public string FileName { get; set; }
        public bool Enable { get; set; }

        public Logger(string fileName, bool enable)
        {
            this.FileName = fileName;
            this.Enable = enable;
        }

        public void write(string str_log)
        {
            this.Massiv.Push(Time() + " = " + str_log);
            if (Enable)
            {
                File.AppendAllLines(this.FileName, this.Massiv);
                this.Massiv.Clear();
            }
        }

        private static string Time()
        {
            return DateTime.Now.ToString();
        }

    }
}
