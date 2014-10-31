using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Classifieds : IEntity
    {

        public virtual long Id { get; set; }
        public virtual bool Archived { get; set; }
        public virtual string Category { get; set; }
        public virtual string Article { get; set; }
        public virtual string ArticleModel { get; set; }
        public virtual float Price { get; set; }
        public virtual string Location { get; set; }
        //public virtual DateTime PostedDate { get; set; }

         public Classifieds(string Category,string Article, string ArticleModel, string Location, float Price)
         {
            this.Category = Category;
            this.Article = Article;
            this.ArticleModel = ArticleModel;
            this.Location = Location;
            this.Price = Price;
            //PostedDate= new DateTime();
            Archived = false;
        }

         public Classifieds()
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
    }
}
