using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Utils
{
    /// <summary>
    /// Class to read information to generate from files
    /// </summary>
    public sealed class FileReader
    {     
        private List<string> result;

        // Information for print on html    
        private List<string> logsList;

        private  string filePath;
        private  string fileName;

        public FileReader()
        {
            result = new List<string>();
            logsList = new List<string>();   
        }

        /// <summary>
        ///Method for reading a file
        /// </summary>
        public void Reading(string fileName, string filePath = "")
        {
            
            this.filePath = (filePath == "") ? AppDomain.CurrentDomain.BaseDirectory + fileName : AppDomain.CurrentDomain.BaseDirectory + filePath + "\\" + fileName;
            this.fileName = fileName;

            if (!File.Exists(this.filePath))
            {  
                //if file don't Exists
                logsList.Add($"The {fileName} file does not exist, read error");
            }
            else
            {
                result.Clear();

                using (StreamReader sr = new StreamReader(this.filePath, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        result.Add(line.Trim());
                    }
                }
            }
        }

        /// <summary>
        /// Get result reading file
        /// </summary>
        /// <returns></returns>
        public List<string> GetResult()
        {  
            return result;
        }
        public List<string> GetLogs()
        {
            return logsList;
        }
    }
}
