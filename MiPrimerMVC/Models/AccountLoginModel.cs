using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiPrimerMVC.Models
{
    public class AccountLoginModel
    {
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}