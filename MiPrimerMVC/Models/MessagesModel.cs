using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;

namespace MiPrimerMVC.Models
{
    public class MessagesModel
    {
        public List<Messages> MessagesList { get; set; }
        public Messages SendMessage { get; set; }
        public Classifieds ClassifiedsShown { get; set; }
        public long MyId { get; set; }
    }
}