
namespace ServerUp
{
    interface ILogger
    {
        string FileName { get; set; }
        bool Enable { get; set; }
        void write(string str_log);
    }
}
