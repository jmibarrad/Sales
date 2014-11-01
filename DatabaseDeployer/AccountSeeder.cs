using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Services;
using DomainDrivenDatabaseDeployer;
using FizzWare.NBuilder;
using NHibernate;
using NHibernate.Mapping;

namespace DatabaseDeployer
{
    class AccountSeeder : IDataSeeder
    {
        readonly ISession _session;

        public AccountSeeder(ISession session)
        {
            _session = session;
        }

        public void Seed()
        {
            ICrypter encrypter = new Hasher();
            var account = new AccountLogin
            {
                Archived = false,
                Email = "jmibarrad@gmail.com",
                Name = "Jose Mario Ibarra",
                Password = encrypter.Encrypt("password"),
                Role = "admin",
                 AccountMessages = new List<Messages>()
                 {
                     new Messages()
                     {
                         Email = "jmibarra@unitec.edu",
                         Name = "Jose Ibarra",
                         Message = "Queria consultar donde estan ubicados? Gracias"
                         
                     }
                 },
                AccountClassifieds = new List<Classifieds>()
                {
                    new Classifieds()
                    {
                        Article = "Mercedes Benz",
                        ArticleModel = "XRLT 720",
                        Category = "Automoviles",
                        Location = "San Diego California",
                        Price = 848
                    }
                }
            };
            _session.Save(account);
            _session.Flush();
            _session.Clear();
        }
    }
}
