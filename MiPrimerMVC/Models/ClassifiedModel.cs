using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiPrimerMVC.Models
{
    public class ClassifiedModel
    {
        public string Category { get; set; }
        public string Article { get; set; }
        public string ArticleModel { get; set; }
        public float Price { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public string UrlVideo { get; set; }
    }
}