﻿using System;
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
        public virtual bool AdminArchived { get; protected set; }
        public virtual string Category { get; set; }
        public virtual string Article { get; set; }
        public virtual string ArticleModel { get; set; }
        public virtual string Description { get; set; }
        public virtual float Price { get; set; }
        public virtual string Location { get; set; }
        public virtual string UrlVideo { get; set; }
        public virtual string UrlImage { get; set; }
        public virtual string UrlImage1 { get; set; }
        public virtual string UrlImage2 { get; set; }
        public virtual string UrlImage3 { get; set; }
        public virtual string UrlImage4 { get; set; }

        public virtual DateTime PostedDate { get; set; }
        public virtual int Likes { get; set; }
        public virtual int Visited { get; set; }
        public virtual string Email { get; set; }
        public virtual bool TermsAndConditionsAccepted { get; protected set; }

        public Classifieds(string Category, string Article, string ArticleModel, string Location, float Price, string description, string email, string urlimage, string urlimage1, string urlimage2, string urlimage3,string urlimage4, string urlvideo)
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
            AdminArchived = false;
            Likes = 0;
            Visited = 0;
            UrlImage = urlimage;
            UrlVideo=urlvideo;

            UrlImage1 = urlimage1=="" ? "http://fpoimg.com/300x300?text=Advertisement" : urlimage1;
            UrlImage2 = urlimage1 == "" ? "http://fpoimg.com/300x300?text=Advertisement" : urlimage2;
            UrlImage3 = urlimage1 == "" ? "http://fpoimg.com/300x300?text=Advertisement" : urlimage3;
            UrlImage4 = urlimage1 == "" ? "http://fpoimg.com/300x300?text=Advertisement" : urlimage4;
         }

         public Classifieds()
         {
             Archived = false;
             PostedDate = DateTime.Today;
             Likes = 0;
             AdminArchived = false;
         }


        public virtual void Archive()
        {
            Archived = true;
        }

        public virtual void Activate()
        {
            Archived = false;
        }

        public virtual void AdminArchive()
        {
            AdminArchived = true;
        }

        public virtual void AdminActivate()
        {
            AdminArchived = false;
        }
    }
}
