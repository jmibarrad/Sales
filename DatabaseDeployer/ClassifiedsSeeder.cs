using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainDrivenDatabaseDeployer;
using NHibernate;
using Domain.Entities;


namespace DatabaseDeployer
{
    class ClassifiedsSeeder :IDataSeeder
    {
         readonly ISession _session;

        public ClassifiedsSeeder(ISession session)
        {
            _session = session;
        }

        public void Seed()
        {
            var classi = new Classifieds
            {
                Category = "Automovile",
                Article = "Mercedes Benz",
                ArticleModel = "XL705",
                Location = "San Diego, Califonia",
                Price = 7896,
                
            };
            _session.Save(classi);
        }
    }
}
