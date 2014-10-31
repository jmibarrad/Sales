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
                Email = "jmibarra@gmail.com",
                Name = "Jose Mario Ibarra",
                Password = encrypter.Encrypt("password"),
                Role = "admin"
            };
            _session.Save(account);
        }
    }
}
