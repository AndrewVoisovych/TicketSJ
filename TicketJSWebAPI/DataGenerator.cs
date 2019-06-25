using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace TicketJSWebAPI
{

    [Serializable]
    public class CreateUniqueNumber
    {
        public int number;
    }

    public class DataGenerator
    {
        private Random rnd;
        private Stream stream;
        private IFormatter formatter;
        private CreateUniqueNumber getNumber;

        private List<string> tags { get; set; }
        private List<string> description { get; set; }
          
          public DataGenerator(TicketForJson ticket)
          {
            rnd = new Random();
            formatter = new BinaryFormatter();
            getNumber = new CreateUniqueNumber();


            ticket.number = NumberGenerator();
            ticket.description = DescriptionGenerator();
            ticket.dateTime = DateTimeGenerator();
            ticket.type = TypeGenerator();
            ticket.tags = TagsGenerator();

            try
            {
                stream = new FileStream("number.dat", FileMode.Open, FileAccess.Write);
                formatter.Serialize(stream, getNumber);
                stream.Close();
            }
            catch(FileNotFoundException)
            {
                return;
            }
        }

        private int NumberGenerator()
        {
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
            DateTime start = DateTime.Now;
            DateTime end = new DateTime(2021,12,31);
            return start.AddDays(rnd.Next((end - DateTime.Today).Days));
        }

        private string DescriptionGenerator()
        {
            ReadToFIle file = new ReadToFIle("descriptionList");
            description = file.GetResult();
            return description[rnd.Next(0, description.Count())];
        }

        private string[] TagsGenerator()
        {
            ReadToFIle file = new ReadToFIle("tagsList");
            tags = file.GetResult();
            var shuffled = tags.OrderBy(a => Guid.NewGuid()).ToList();
            int quantityTags = rnd.Next(1, tags.Count());
           return shuffled.Take(quantityTags).ToArray();
        }
    }
}
