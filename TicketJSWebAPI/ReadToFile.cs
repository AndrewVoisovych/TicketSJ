using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketJSWebAPI
{
    class ReadToFIle
    {
        private List<string> result;
        private List<string> logsList;

        public ReadToFIle(string fileName)
        {
            result = new List<string>();
            logsList = new List<string>();
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Info\\" + fileName + ".txt";
            if (!File.Exists(filepath))
            {
                logsList.Add("The" + fileName + "file does not exist, read error");
            }
            else
            {
                using (StreamReader sr = new StreamReader(filepath, System.Text.Encoding.Default))
                {       
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        result.Add(line.Trim());
                    }
                }
            }
        }

        public List<string> GetResult()
        {
            return this.result;
        }
        public List<string> GetLogs()
        {
            return logsList;
        }
    }
}
