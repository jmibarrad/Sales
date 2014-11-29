using System.Collections.Generic;

namespace MiPrimerMVC.Models
{
    public class AccountRegisterModel 
    {
        public IEnumerable<AccountLoginModel> LogInIEnumerable { get; set; }
        public IEnumerable<RegisterModel> RegisterEnumerable { get; set; }

    }
}