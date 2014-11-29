using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Domain.Entities;
using MiPrimerMVC.ValidationAttributes;

namespace MiPrimerMVC.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "The password must be between 3 and 50 characters.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [DataType(DataType.MultilineText)]
        [DescriptionValidation(MinimumAmountOfWords = 3, MaximumAmountOfCharacters = 250,
            ErrorMessage = "The description must contains a minimum of 3 words and a maximum of 250 characters.")]
        public string Message { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email is not valid.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Text)]
        public string Subject { get; set; }
    }
}