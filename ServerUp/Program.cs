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
using static System.Net.Mime.MediaTypeNames;

namespace ServerUp
{
    class Program
    {
        static int id { get; set; }
        static void Main(string[] args)
        {

            //Dayz2.ErrorDialog = true;
            //Dayz2.RedirectStandardError = true;
            // Dayz2.RedirectStandardOutput = true;




            //Config cfg = Config.SetConfig("ServerUp.config.json");
            //Console.WriteLine(cfg.ServerParams);
            
            

            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            ILogger Log = new Logger("server_status.log", true);
            int port = 2302;
            Start();
            Thread.Sleep(30000);
            while (true)
            {
                Thread.Sleep(1000);                          
                Console.WriteLine(DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + " " + PortState(port));
                Console.WriteLine(Process.GetProcessById(id));
                Log.write(PortState(port).ToString());

                if (!PortState(port) || !Process.GetProcessById(id).Responding)
                {
                    Console.WriteLine("ERROR");
                    try
                    {
                        Process.GetProcessById(id).Kill();
                        Start();
                        Console.WriteLine("Wait start server...");
                        Log.write("Wait start server...");
                        Thread.Sleep(30000);
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
            bool alreadyinuse = (from p in System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners() where p.Port == port select p).Count() == 1;
            return alreadyinuse;
        }
        public static void Start()
        {
            new Thread(new ThreadStart(Test)).Start();
        }
        public static void Test()
        {
            ProcessStartInfo Dayz2 = new ProcessStartInfo();
            Dayz2.Arguments = "-profiles=D:\\SteamLibrary\\steamapps\\common\\DayZServer\\battleye -adminlog -dologs -config=serverDZ.cfg -port=2302";
            Dayz2.FileName = "D:\\SteamLibrary\\steamapps\\common\\DayZServer\\DayZServer_x64.exe";
            Dayz2.ErrorDialog = false;
            try
            {
                using (Process exeProcess = Process.Start(Dayz2))
                {
                    id = exeProcess.Id;
                    Console.WriteLine(exeProcess.Id);
                    Console.WriteLine();
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


    }


    public interface ILogger
    {
        string FileName { get; set; }
        bool Enable { get; set; }

        void write(string str_log);

    }

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



    public interface IConfig
    {
        string FileLogName { get; set; }
        string DirectoryLog { get; set; }
        string ServerDirectory { get; set; }
        int ServerPort { get; set; }
        int ServerAwait { get; set; }
        string[] ServerParams { get; set; }
        bool EnableLogger { get; set; }
    }

    
    public class Config : IConfig
    {
        public Config() { }

        [JsonProperty("FileLogName")]
        public string FileLogName { get; set; }

        [JsonProperty("DirectoryLog")]
        public string DirectoryLog { get; set; }

        [JsonProperty("ServerDirectory")]
        public string ServerDirectory { get; set; }

        [JsonProperty("ServerPort")]
        public int ServerPort { get; set; }

        [JsonProperty("ServerAwait")]
        public int ServerAwait { get; set; }

        [JsonProperty("ServerParams")]
        public string[] ServerParams { get; set; }

        [JsonProperty("EnableLogger")]
        public bool EnableLogger { get; set; }        

        public bool GetConfig()
        {
            return GetConfig("ServerUp.config");
        }
        public bool GetConfig(string config_file_name)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();

                using (StreamWriter sw = new StreamWriter(config_file_name))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, this);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public static Config SetConfig(string config_file_name)
        {
            JsonSerializer serializer = new JsonSerializer();

            using (StreamReader sr = new StreamReader(config_file_name))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JObject json = (JObject)serializer.Deserialize(reader, Type.GetType("JObject"));

                

                Console.WriteLine(serializer.Deserialize<Config>(reader));
                Console.ReadKey();
                return null;

                //return (Config)serializer.Deserialize(reader, Type.GetType("Config"));
            }
        }

    }
}
