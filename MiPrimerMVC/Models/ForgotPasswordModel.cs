using System.ComponentModel.DataAnnotations;

namespace MiPrimerMVC.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email is not valid.")]
        [Display(Name = "Email")]
        public string email { get; set;}
        
    }
}