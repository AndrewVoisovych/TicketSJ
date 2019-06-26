using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TicketSJWindowsService
{
    public partial class Service1 : ServiceBase
    {

        Timer timer = new Timer();
        private WriteToFile writeCommand = new WriteToFile();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            writeCommand.Write("Service is started at " + DateTime.Now);
            ReceiveMessages obj = new ReceiveMessages(); // Receive messages and process them

        }

        protected override void OnStop()
        {
            writeCommand.Write("Service is stopped at " + DateTime.Now);
        }
    }
}
