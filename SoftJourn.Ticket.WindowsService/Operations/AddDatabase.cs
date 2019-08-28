using System;
using System.Collections.Generic;
using WindowsService.Entity;
using System.Linq;
using WindowsService.Models;

namespace WindowsService
{

    /// <summary>
    ///  Class to add a ticket data to Azure Sql
    /// </summary>
    public sealed class AddDatabase
    {
     
        private int Number { get; set; }
        private int Id { get; set; }
        private string Description { get; set; }
        private TicketTypeEnum.TicketType TicketTypeId { get; set; }
     
        private DateTime ExpiryDateTime { get; set; }
        private List<int> Tags { get; set; }

        public string Add(Models.Ticket t)
        {
            // Error
            if (NumberCheck(t.Number) == false)
            {
                return "Error: A ticket with this number already exists";
            }
            if (TypeCheck(t.Type) == false)
            {

                return "Error: There is no such type";
            }
            //.

            Number = t.Number;
            Id = CreateId();
            TicketTypeId = t.Type;
            Description = t.Description;
            ExpiryDateTime = t.DateTime;
            Tags = TagsOperation(t.Tags);

            //Add Ticket to the database
            if (TicketAdd())
            {
                string TicketAddTagsError = "";
                //Add Tags's Ticket to the database
                if (TicketAddTags() == false)
                {
                    TicketAddTagsError = "Error:  There was an error adding tags to the database";
                }
                TicketAddTags();

                return (TicketAddTagsError == "") ? $"Ticket number: {Number} added to the database" : $"Ticket number: {Number} added to the database. There was an error adding tags to the database";
            }
            else
            {
                return "Error: There was an error adding ticket to the database";
            }
          
        }


        /// <summary>
        /// Checking the unique flower number
        /// </summary>
        /// <param name="receivedNumber">Number Ticket</param>
        /// <returns>true or false</returns>
        private bool NumberCheck(int receivedNumber)
        {
            using(TicketSoftjournContext myDatabase = new TicketSoftjournContext())
            {
                var number = myDatabase.Ticket.Where(n => n.Number == receivedNumber).FirstOrDefault();

                return number == null ? true : false;
            }
          
        }


        /// <summary>
        /// Create new id for ticket
        /// </summary>
        /// <returns>int:new id for ticket</returns>
        private int CreateId()
        {
            using (TicketSoftjournContext myDatabase = new TicketSoftjournContext())
            {
                var selectId = myDatabase.Ticket.Select(x => x.Id).FirstOrDefault();

                return (selectId == 0) ? 1 : myDatabase.Ticket.OrderByDescending(x => x.Id).Select(x => x.Id).First() + 1;
            }
        }


        /// <summary>
        /// Checking the existence of the  Type 
        /// </summary>
        /// <param name="receivedType">received Ticket Type</param>
        /// <returns>true or false</returns>
        private bool TypeCheck(TicketTypeEnum.TicketType receivedType)
        {
            using (TicketSoftjournContext myDatabase = new TicketSoftjournContext())
            {
                var type = myDatabase.TicketType.Where(t => t.TypeId == (int)receivedType).FirstOrDefault();

                return type != null ? true : false;
            }
        }


        /// <summary>
        /// Checking the existence of the   tags and tagging
        /// </summary>
        /// <param name="receivedTags">Array of strings received Ticket Type</param>
        /// <returns>List<int>: Id position tags on database </returns>
        private List<int> TagsOperation(string[] receivedTags)
        {
            //list for position tags in db
            List<int> positionTags = new List<int>();

            using (TicketSoftjournContext myDatabase = new TicketSoftjournContext())
            {
                for (int i = 0; i < receivedTags.Length; i++)
                {
                    var tag = myDatabase.TicketTags.Where(t => t.TagTitle.ToLower() == receivedTags[i].ToLower()).FirstOrDefault();
                    if (tag != null)
                    {   
                        //if exists
                        positionTags.Add(tag.TagId);
                    }
                    else
                    {
                        // if no  exists  - Add tag
                        positionTags.Add(TagsAdd(receivedTags[i]));
                    }
                }
            }

            //Remove all result: -1
            positionTags.RemoveAll(l => l == -1);


            return positionTags;
        }


        /// <summary>
        /// Tag's add
        /// </summary>
        /// <param name="addTag">String Tag</param>
        /// <returns>int: new id tag or error -1</returns>
        private int TagsAdd(string addTag)
        {
            using (TicketSoftjournContext myDatabase = new TicketSoftjournContext())
            {
                var tag = myDatabase.TicketTags.Where(t => t.TagTitle.ToLower() == addTag.ToLower()).FirstOrDefault();
                if (tag != null)
                {   
                    //if exists
                    return tag.TagId;
                }
                else
                {
                    try
                    {
                        int newId;
                        var selectId = myDatabase.TicketTags.Select(x => x.TagId).FirstOrDefault();
                      
                        //if exists = db query+1, else 1
                        newId = (selectId == 0) ? 1 : myDatabase.TicketTags.OrderByDescending(x => x.TagId).Select(x => x.TagId).First() + 1;


                        //add tag
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
                        //error returns -1
                        return -1;
                    }
                }
            }
        }


        /// <summary>
        /// Add Ticket with receive (class)message 
        /// </summary>
        /// <returns>true or false</returns>
        private bool TicketAdd()
        {
            using (TicketSoftjournContext myDatabase = new TicketSoftjournContext())
            {
                try
                {
                    Entity.Ticket ticketDb = new Entity.Ticket
                    {
                        Id = Id,
                        Number = Number,
                        Description = Description,
                        TicketTypeId = (int)TicketTypeId,
                        ExpiryDateTime = ExpiryDateTime

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
        }


        /// <summary>
        /// Add tags's ticket
        /// </summary>
        /// <returns>true or false</returns>
        private bool TicketAddTags()
        {
            using (TicketSoftjournContext myDatabase = new TicketSoftjournContext())
            {
                try
                {
                    for (int i = 0; i < Tags.Count(); i++)
                    {
                        var selectId = myDatabase.TicketToTags.Select(x => x.Id).FirstOrDefault();
                        int newId = (selectId == 0) ? 1 : myDatabase.TicketToTags.OrderByDescending(x => x.Id).Select(x => x.Id).First() + 1; ;
                        

                        TicketToTags tagsDb = new TicketToTags
                        {
                            Id = newId,
                            TicketId = Id,
                            TagId = Tags[i]
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
}
