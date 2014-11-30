using System.Collections.Generic;
using Domain.Entities;
using Domain.Services;
using DomainDrivenDatabaseDeployer;
using NHibernate;

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
                 AccountMessages = new List<Messages>
                 {
                     new Messages
                     {
                         Email = "jmibarra@unitec.edu",
                         Name = "Jose Ibarra",
                         Message = "Queria consultar donde estan ubicados? Gracias",
                         Subject = "Importante"
                     }
                 }
               
            };
            _session.Save(account);
            _session.Flush();
            _session.Clear();
        }
    }
}
