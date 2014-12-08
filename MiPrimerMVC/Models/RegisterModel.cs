using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimerMVC.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "First name should be between 3 and 50 characters.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessage = "Email should be between 5 and 100 characters.", MinimumLength = 5)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Password should be between 8 and 20 characters.", MinimumLength = 8)]
        [Compare("ConfirmPassword", ErrorMessage = "Password and Confirm Password should be the same")]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "Password can only accept letters and numbers")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Password should be between 8 and 20 characters.", MinimumLength = 8)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password should be the same")]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "Password can only accept letters and numbers")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Accept Terms & Conditions")]
        public bool TermsAndConditionsAccepted { get; set; }

    }


}