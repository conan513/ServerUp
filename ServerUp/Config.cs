using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerUp
{
    public class Config : IConfig
    {
        public Config() { }        

        public Config(string file_name, string directory_log, string server_directory, int server_port, int server_await, string server_params, bool enable_logger)
        {
            this.FileLogName = file_name;
            this.DirectoryLog = directory_log;
            this.ServerDirectory = server_directory;
            this.ServerPort = server_port;
            this.ServerAwait = server_await * 1000;         /////////////////////// This is a feature but not a bug =)))
            this.ServerParams = server_params;
            this.EnableLogger = enable_logger;
        }

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
        public string ServerParams { get; set; }

        [JsonProperty("EnableLogger")]
        public bool EnableLogger { get; set; }

        public bool SetConfig()
        {
            return SetConfig("ServerUp.config");
        }
        public bool SetConfig(string config_file_name)
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
        public static Config GetConfig(string config_file_name)
        {
            JsonSerializer serializer = new JsonSerializer();

            using (StreamReader sr = new StreamReader(config_file_name))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JObject json = (JObject)serializer.Deserialize(reader, Type.GetType("JObject"));

                return new Config(
                    (string)json.GetValue("FileLogName"),
                    (string)json.GetValue("DirectoryLog"),
                    (string)json.GetValue("ServerDirectory"),
                    (int)json.GetValue("ServerPort"),
                    (int)json.GetValue("ServerAwait"),
                    (string)json.GetValue("ServerParams"),
                    (bool)json.GetValue("EnableLogger")
                    );
                
            }
        }

    }
}
