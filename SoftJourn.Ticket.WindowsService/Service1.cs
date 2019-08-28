using System;
using System.ServiceProcess;
using System.Timers;
using WindowsService.Utils.Helpers;
using WindowsService.AzureReceive;


namespace WindowsService
{
    public partial class Service1 : ServiceBase
    {

        private Timer timer = new Timer();
        /// <summary>
        ///The operation of writing to a file
        /// </summary>
        private WrittenFileHelper file = new WrittenFileHelper();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            file.Write($"Service is started at {DateTime.Now}");
           
           
            // Receive messages and process them
            ReceiveMessages obj = new ReceiveMessages(); 

        }

        protected override void OnStop()
        {
            file.Write($"Service is stopped at {DateTime.Now}");
        }
    }
}
