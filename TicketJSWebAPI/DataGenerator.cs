using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using WebAPI.Models;
using WebAPI.Utils;
using WebAPI.Utils.Helpers;

namespace WebAPI
{
    /// <summary>
    /// To generate a unique number, it is always copied from the file using serialization.
    /// </summary>
    [Serializable]
    public sealed class CreateUniqueNumber
    {  
        public int number;
    }

    /// <summary>
    /// Class of data generation for Ticket
    /// </summary>
    public sealed class DataGenerator
    {
        private Random random;  

        private Stream streamSerializable; //work with files
        private IFormatter formatterSerializable; //serialization
        private CreateUniqueNumber getNumber;

       

        private List<string> Tags { get; set; }
        private List<string> Description { get; set; }

        private FileReader file;
        private FileInfoHelper fileInfo;

        /// <summary>
        ///  Information for print on html
        /// </summary>
        private List<string> LogsList { get; set; } 


        public DataGenerator(Ticket ticket)
          {
            random = new Random();
            formatterSerializable = new BinaryFormatter();
            getNumber = new CreateUniqueNumber();
            LogsList = new List<string>();
            file = new FileReader();
            fileInfo = new FileInfoHelper();

            FillingTicket(ticket);
        }


        private void FillingTicket(Ticket ticket)
        {
            ticket.Number = NumberGenerator();
            ticket.Description = DescriptionGenerator();
            ticket.DateTime = DateTimeGenerator();
            ticket.Type = TypeGenerator();
            ticket.Tags = TagsGenerator();
            ValidationSerialization();
        }

        /// <summary>
        /// Generate a unique number
        /// </summary>
        /// <returns>int: unique number</returns>
        private int NumberGenerator()
        {
            
            //Number generator. If the serialization file exists - reads the value from it.
            //If value = 0 then 1, otherwise simply increments it.
            //If the serialization file does not exist - create it and value = 1
            try
            {
                streamSerializable = new FileStream("number.dat", FileMode.Open, FileAccess.Read);
                getNumber = (CreateUniqueNumber)formatterSerializable.Deserialize(streamSerializable);
                getNumber.number = (getNumber.number == 0) ? 1 : ++getNumber.number;
                streamSerializable.Close();
            }
            catch (FileNotFoundException)
            {
                getNumber.number = 1;
                streamSerializable = new FileStream("number.dat", FileMode.Create, FileAccess.Write);
                formatterSerializable.Serialize(streamSerializable, getNumber);
                streamSerializable.Close();
            }

            return getNumber.number;
        }

        /// <summary>
        /// Validation of serialization file for correctness
        /// </summary>
        private void ValidationSerialization()
        {
            try
            {
                //update serialization file
                streamSerializable = new FileStream("number.dat", FileMode.Open, FileAccess.Write);
                formatterSerializable.Serialize(streamSerializable, getNumber);
                streamSerializable.Close();
            }
            catch (FileNotFoundException)
            {
                // If something happens to the serialization file
                LogsList.Add("Error writing a unique ticket number");
            }
        }

        /// <summary>
        /// Randomly select one of the enum listed
        /// </summary>    
        private TicketTypeEnum.TicketType TypeGenerator()
        {
            return (TicketTypeEnum.TicketType)random.Next(0, Enum.GetNames(typeof(TicketTypeEnum.TicketType)).Length);
        }

        /// <summary>
        ///  DateTime Generator. Randomly selects a date to a given range.
        /// </summary>
        /// <returns>Random datetime at a predetermined interval</returns>
        private DateTime DateTimeGenerator()
        {
            DateTime start = DateTime.Now;
            DateTime end = new DateTime(2021,12,31);

            return start.AddDays(random.Next((end - DateTime.Today).Days));
        }

        /// <summary>
        /// Description Generator. Reads from a file to a list and randomly select.
        /// </summary>
        /// <returns>Randomly select string description</returns>
        private string DescriptionGenerator()
        {
            file.Reading(fileInfo.GetFileNameString("DescriptionFile"), fileInfo.GetFilePathString());
            LogsList.AddRange(file.GetLogs());
            Description = file.GetResult();

            return Description[random.Next(0, Description.Count())];
        }

        /// <summary>
        /// Tag Generator. Reads from a file to a list, shuffle, and randomly selects the number of first tags.
        /// </summary>
        /// <returns>Array of string: Randomly selects the number of first tags</returns>
        private string[] TagsGenerator()
        {        
            file.Reading(fileInfo.GetFileNameString("TagsFile"), fileInfo.GetFilePathString());
            LogsList.AddRange(file.GetLogs());
            Tags = file.GetResult();
            var shuffled = Tags.OrderBy(a => Guid.NewGuid()).ToList();
            int quantityTags = random.Next(1, Tags.Count());

            return shuffled.Take(quantityTags).ToArray();
        }

        public List<string> GetLogs()
        {
            return LogsList;
        }

    }
}
