using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;

namespace MiPrimerMVC.Models
{
    public class AccountRegisterModel 
    {
        public IEnumerable<AccountLoginModel> LogInIEnumerable { get; set; }
        public IEnumerable<RegisterModel> RegisterEnumerable { get; set; }

    }
}