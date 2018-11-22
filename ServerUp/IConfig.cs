namespace ServerUp
{
    interface IConfig
    {
        string FileLogName { get; set; }
        string DirectoryLog { get; set; }
        string ServerDirectory { get; set; }
        string ServerFileName { get; set; }
        int ServerPort { get; set; }
        int ServerAwait { get; set; }
        string ServerParams { get; set; }
        bool EnableLogger { get; set; }
        bool SetConfig(string config_file_name);
    }
}
