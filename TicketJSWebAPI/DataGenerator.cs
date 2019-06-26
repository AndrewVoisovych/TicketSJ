using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace TicketJSWebAPI
{
    //Class of data generation for Ticket


    //To generate a unique number, it is always copied from the file using serialization.
    [Serializable]
    public class CreateUniqueNumber
    {  
        public int number;
    }

    public class DataGenerator
    {
        private Random rnd;
        private Stream stream; //work with files
        private IFormatter formatter; //serialization
        private CreateUniqueNumber getNumber;

        private List<string> tags { get; set; }
        private List<string> description { get; set; }

        //Information for print on html
        private List<string> logsList { get; set; } //


        public DataGenerator(TicketForJson ticket)
          {
            rnd = new Random();
            formatter = new BinaryFormatter();
            getNumber = new CreateUniqueNumber();
            logsList = new List<string>();

            // Get Data
            ticket.number = NumberGenerator();
            ticket.description = DescriptionGenerator();
            ticket.dateTime = DateTimeGenerator();
            ticket.type = TypeGenerator();
            ticket.tags = TagsGenerator();

            try
            {
                //update serialization file
                stream = new FileStream("number.dat", FileMode.Open, FileAccess.Write);
                formatter.Serialize(stream, getNumber);
                stream.Close();
            }
            catch(FileNotFoundException)
            {
                // If something happens to the serialization file
                logsList.Add("Error writing a unique ticket number");
            }
        }

        private int NumberGenerator()
        {
            //Number generator. If the serialization file exists - reads the value from it.
            //If value = 0 then 1, otherwise simply increments it.
            //If the serialization file does not exist - create it and value = 1
            try
            {
                stream = new FileStream("number.dat", FileMode.Open, FileAccess.Read);
                getNumber = (CreateUniqueNumber)formatter.Deserialize(stream);
                getNumber.number = (getNumber.number == 0) ? 1 : ++getNumber.number;
                stream.Close();

            }
            catch (FileNotFoundException)
            {
                getNumber.number = 1;
                stream = new FileStream("number.dat", FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, getNumber);
                stream.Close();
            }
            return getNumber.number;
        }
        private TicketType TypeGenerator()
        {
            return (TicketType)rnd.Next(0, Enum.GetNames(typeof(TicketType)).Length);
        }
        
        private DateTime DateTimeGenerator()
        {
            //DateTime Generator. Randomly selects a date to a given range.
            DateTime start = DateTime.Now;
            DateTime end = new DateTime(2021,12,31);
            return start.AddDays(rnd.Next((end - DateTime.Today).Days));
        }

        private string DescriptionGenerator()
        {
            //Description Generator. Reads from a file to a list and randomly select.
            ReadToFIle file = new ReadToFIle("descriptionList");
            logsList.AddRange(file.GetLogs());
            description = file.GetResult();
            return description[rnd.Next(0, description.Count())];
        }

        private string[] TagsGenerator()
        {
            //Tag Generator. Reads from a file to a list, shuffle, and randomly selects the number of first tags.
            ReadToFIle file = new ReadToFIle("tagsList");
            logsList.AddRange(file.GetLogs());
            tags = file.GetResult();
            var shuffled = tags.OrderBy(a => Guid.NewGuid()).ToList();
            int quantityTags = rnd.Next(1, tags.Count());
           return shuffled.Take(quantityTags).ToArray();
        }

        public List<string> GetLogs()
        {
            return logsList;
        }

    }
}
