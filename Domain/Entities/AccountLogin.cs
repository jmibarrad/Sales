﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AccountLogin : IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool Archived { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string Role { get; set; }
        public virtual IEnumerable<Classifieds> AccountClassifieds { get; set;}
        public virtual IEnumerable<Messages> AccountMessages { get; set; } 

        public AccountLogin(string email, string name, string password, string role)
        {
            Role = role;
            Name = name;
            Email = email;
            Password = password;
            Archived = false;
        }

        public AccountLogin()
        {
            // TODO: Complete member initialization
        }

        public virtual void Archive()
        {
            Archived = true;
        }

        public virtual void Activate()
        {
            Archived = false;
        }

        public virtual void AddClassified(Classifieds newClassified)
        {
            if (AccountClassifieds.All(x => x.Id == newClassified.Id))
            {
                ((IList<Classifieds>)AccountClassifieds).Add(newClassified);
            }
        }

        public virtual void ArchiveClassified(long classifiedId)
        {
            var classifiedToBeArchived = AccountClassifieds.FirstOrDefault(x => x.Id == classifiedId);
            if (classifiedToBeArchived != null)
            {
                classifiedToBeArchived.Archive();
            }
        }

        public virtual void AddMessage(Messages newMessage)
        {
            if (AccountMessages.All(x => x.Id == newMessage.Id))
            {
                ((IList<Messages>)AccountMessages).Add(newMessage);
            }
        }

        public virtual void ArchiveMessage(long messageId)
        {
            var classifiedToBeArchived = AccountClassifieds.FirstOrDefault(pred => pred.Id == messageId);
            if (classifiedToBeArchived != null)
            {
                classifiedToBeArchived.Archive();
            }
        }
    }
}
