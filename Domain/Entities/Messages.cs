using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Messages: IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool Archived { get; protected set; }
        public virtual string Subject { get; set; }
        public virtual string Message { get; set; }
        public virtual string Email { get; set; }
        public virtual string Name { get; set; }
        
        public Messages(string email, string name,string message, string  subject)
        {
            Archived = false;
            Message = message;
            Email = email;
            Name = name;
            Subject = subject;
        }

        public Messages()
        {
            Archived = false;

        }
        public virtual void Archive()
        {
            Archived = true;
        }

        public virtual void Activate()
        {
            Archived = false;
        }
    }
}
