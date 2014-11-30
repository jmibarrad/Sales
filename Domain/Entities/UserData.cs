using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserData
    {
        public virtual string Gender { get; set; }
        public virtual string Country { get; set; }
        public virtual int AmountOfClassifieds { get; set; }
        public virtual string Cellphone { get; set; }
        public virtual string OfficePhone { get; set; } 
        public virtual string ProfileImg { get; set; }
        public virtual string Description { get; set; }
        public virtual float TotalSold { get; set; }

    }
}
