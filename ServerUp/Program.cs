using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServerUp
{
    class Program
    {

        //IConfig Conf { get { return this.Conf; } set { this.Conf = Config.GetConfig("ServerUp.config.json"); } }
        static IConfig Conf { get; set; }
        static Logger Log { get; set; }
        static int ProcessID { get; set; }
        static void Main(string[] args)
        {
            Conf = Config.GetConfig("ServerUp.config.json");
            Log = new Logger(Conf.FileLogName, Conf.EnableLogger);


            bool _start = false;
            //{ For debug
            //    Console.WriteLine(Conf.ServerParams);
            //    Console.ReadKey();
            //    Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            //    //ILogger Log = new Logger("server_status.log", true);
            //}


            Start();
            Console.Write("Wait...");
            while (!_start)
            {
                if (PortState(Conf.ServerPort))
                    _start = true;
                Console.Write(".");
                Thread.Sleep(1000);
                
            }


            while (true)
            {
                Thread.Sleep(1000);                          
                Console.WriteLine(DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + " " + PortState(Conf.ServerPort));
                Log.write(PortState(Conf.ServerPort).ToString());

                if (!PortState(Conf.ServerPort) || !Process.GetProcessById(ProcessID).Responding)
                {
                    Console.WriteLine("ERROR");
                    try
                    {
                        Process.GetProcessById(ProcessID).Kill();
                        Start();
                        Console.WriteLine("Wait start server...");
                        Log.write("Wait start server...");
                        Thread.Sleep(Conf.ServerAwait);
                        Console.WriteLine("Server started!");
                        Log.write("Server started!");
                    }
                    catch (Exception e )
                    {
                        Console.WriteLine(e);
                    }
                }
                
            }

        }
        public static bool PortState(int port)
        {
            bool alreadyinuse = (from p in System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners() where p.Port == Conf.ServerPort select p).Count() == 1;
            return alreadyinuse;
        }
        public static void Start()
        {
            new Thread(new ThreadStart(new ProcessManager("D:\\SteamLibrary\\steamapps\\common\\DayZServer\\DayZServer_x64.exe", Conf.ServerParams).Start)).Start();
        }


    }


   

    



  

    
    
}
