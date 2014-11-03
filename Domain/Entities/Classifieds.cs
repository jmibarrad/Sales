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
        public virtual bool Archived { get; protected set; }
        public virtual string Category { get; set; }
        public virtual string Article { get; set; }
        public virtual string ArticleModel { get; set; }
        public virtual string Description { get; set; }
        public virtual float Price { get; set; }
        public virtual string Location { get; set; }
        public virtual string UrlVideo { get; set; }
        public virtual string UrlImage { get; set; }
        public virtual DateTime PostedDate { get; set; }
        public virtual int Likes { get; set; }
        public virtual string Email { get; set; }

         public Classifieds(string Category, string Article, string ArticleModel, string Location, float Price, string description, string email, string urlimage, string urlvideo)
         {
            this.Category = Category;
            this.Article = Article;
            this.ArticleModel = ArticleModel;
            this.Location = Location;
            Email = email;
            this.Price = Price;
            Description = description;
            PostedDate= DateTime.Today;
            Archived = false;
            Likes = 0;
            UrlImage = urlimage;
            UrlVideo=urlvideo;
         }

         public Classifieds()
         {
             Archived = false;
             PostedDate = DateTime.Today;
             Likes = 0;
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
