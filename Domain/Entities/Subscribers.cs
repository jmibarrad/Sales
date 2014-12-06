using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Subscribers: IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool Archived { get; protected set; }
        public virtual IEnumerable<AccountLogin> Followers { get; set; }
        public virtual IEnumerable<AccountLogin> Following { get; set; }

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
