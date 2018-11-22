using System.Diagnostics;

namespace ServerUp
{
    class ProcessManager
    {
        int ProcessID { get; set; }
        string StartFileName { get; set; }
        string StartArguments { get; set; }
        ProcessStartInfo ProcessInfo { get; set; }
        public Process iProcess { get; set; }


        public ProcessManager() { }
        public ProcessManager(string start_file_name, string start_arguments_string)
        {
            this.StartFileName = start_file_name;
            this.StartArguments = start_arguments_string;
            ProcessInfo = ProcessInfoSet(new ProcessStartInfo());  
        }
        public void Start()
        {
            try
            {
                iProcess = Process.Start(this.ProcessInfo);                    
                //return true;
            }
            catch
            {
                //return false;
            }
        }
        public ProcessStartInfo ProcessInfoSet(ProcessStartInfo process_info)
        {
            process_info.FileName = this.StartFileName;
            process_info.Arguments = this.StartArguments;
            return process_info;
        }

        public bool Stop()
        {
            try
            {
                iProcess.Kill();
                return true;
            }
            catch
            {

                return false;
            }
        }

        public bool Restart()
        {
            try
            {

                return true;
            }
            catch
            {

                return false;
            }
        }

        public bool CrachTest()
        {
            try
            {

                return true;
            }
            catch
            {

                return false;
            }
        }

    }
}
