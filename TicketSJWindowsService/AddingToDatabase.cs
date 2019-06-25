using System;
using System.Collections.Generic;
using System.Text;
using TicketSJWindowsService.Models;
using System.Linq;


namespace TicketSJWindowsService
{
   public class AddingToDatabase
    {

        private TicketSoftjournContext myDatabase;

        //

        private int Number { get; set; }
        private int Id { get; set; }
        private string Description { get; set; }
        private TicketType TicketTypeId { get; set; }
        private DateTime ExpiryDateTime { get; set; }
        private List<int> Tags { get; set; }

        public AddingToDatabase()
        {
            myDatabase = new TicketSoftjournContext();
           
        }
        public string Add(TicketForJson t)
        {
            if (NumberCheck(t.number) == false)
            {
                return "Error: A ticket with this number already exists";
            }
            if (TypeCheck(t.type) == false)
            {
                return "Error: There is no such type";
            }

            Number = t.number;
            Id = CreateId();
            TicketTypeId = t.type;
            Description = t.description;
            ExpiryDateTime = t.dateTime;
            Tags = TagsOperation(t.tags);

            if (TicketAdd())
            {

                string TicketAddTagsError = "";
                if (TicketAddTags() == false)
                {
                    TicketAddTagsError = "Error:  There was an error adding tags to the database";
                }

                TicketAddTags();

                return (TicketAddTagsError == "") ? "Ticket number: " + Number + " added to the database" : "Ticket number: " + Number + " added to the database. There was an error adding tags to the database";
            }
            else
            {
                return "Error: There was an error adding ticket to the database";
            }
          
        }

        private bool NumberCheck(int receivedNumber)
        {
            var number = myDatabase.Ticket.Where(n=>n.Number== receivedNumber).FirstOrDefault();
            return number == null ? true : false;
        } 

        private int CreateId()
        {
            var varId = (from x in myDatabase.Ticket select x.Id).FirstOrDefault();
            return (varId == 0) ? 1 : (from x in myDatabase.Ticket select x.Id).ToArray().Max() + 1;
        }

        private bool TypeCheck(TicketType receivedType)
        {
            var type = myDatabase.TicketType.Where(t => t.TypeId == (int)receivedType).FirstOrDefault();
            return type != null ? true : false;
            //розширення щоб можливість додавати
        }

        private List<int> TagsOperation(string[] receivedTags)
        {
            List<int> positionTags = new List<int>();
            for(int i=0; i<receivedTags.Length; i++)
            {
                var tag = myDatabase.TicketTags.Where(t => t.TagTitle.ToLower() == receivedTags[i].ToLower()).FirstOrDefault();
                if (tag != null)
                {
                    positionTags.Add(tag.TagId);
                }
                else
                {
                  positionTags.Add(TagsAdd(receivedTags[i]));   
                }
            }

            positionTags.RemoveAll(l => l == -1);
            return positionTags;
        }

        private int TagsAdd(string addTag)
        {
            var tag = myDatabase.TicketTags.Where(t => t.TagTitle.ToLower() == addTag.ToLower()).FirstOrDefault();
            if (tag != null)
            {
                return tag.TagId;
            }
            else
            {            
                try
                {


                    int newId;
                    var varNewId = (from x in myDatabase.TicketTags select x.TagId).FirstOrDefault();
                    newId = (varNewId == 0) ? 1 : (from x in myDatabase.TicketTags select x.TagId).ToArray().Max() + 1;
                 

                    TicketTags tagDb = new TicketTags
                    {
                        TagId = newId,
                        TagTitle = addTag.ToLower()
                    };
                    myDatabase.TicketTags.Add(tagDb);
                    myDatabase.SaveChanges();
                    return newId;
                }
                catch 
                {
                    return -1;
                }
            }
        }

        private bool TicketAdd()
        {
            try
            {
                Ticket ticketDb = new Ticket
                {
                    Id = this.Id,
                    Number = this.Number,
                    Description = this.Description,
                    TicketTypeId = (int)TicketTypeId,
                    ExpiryDateTime = this.ExpiryDateTime

                };
                myDatabase.Ticket.Add(ticketDb);
                myDatabase.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TicketAddTags()
        {
            try
            {
                for (int i = 0; i < Tags.Count(); i++)
                {
                    var varId = (from x in myDatabase.TicketToTags select x.Id).FirstOrDefault();
                    int newId = (varId == 0) ? 1 : (from x in myDatabase.TicketToTags select x.Id).ToArray().Max() + 1;

                    TicketToTags tagsDb = new TicketToTags
                    {
                        Id = newId,
                        TicketId = this.Id,
                        TagId = this.Tags[i]
                    };
                    myDatabase.TicketToTags.Add(tagsDb);
                    myDatabase.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
