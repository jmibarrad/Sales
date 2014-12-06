using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Domain.Entities;
using MiPrimerMVC.ValidationAttributes;

namespace MiPrimerMVC.Models
{
    public class ManageProfileModel
    {
        public AccountLogin CurrentUser { get; set; } 

        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        [DescriptionValidation(MinimumAmountOfWords = 1, MaximumAmountOfCharacters = 50,
            ErrorMessage = "The Name must contains a minimum of 1 word and a maximum of 50 characters.")]
        public string Name { get; set; }
        public string Gender { get; set; }

        public string Country { get; set; }

        [DataType(DataType.Url)]
        [DescriptionValidation(MinimumAmountOfWords = 1, MaximumAmountOfCharacters = 100,
            ErrorMessage = "The BlogUrl must contains a minimum of 1 word and a maximum of 100 characters.")]
        public string Blog { get; set; }
        public int AmountOfClassifieds { get; set; }

        [Required(ErrorMessage = "Cellphone Number is required")]
        [DataType(DataType.Text)]
        public string Cellphone { get; set; }

        [DataType(DataType.Text)]
        public string OfficePhone { get; set; }

        [DataType(DataType.ImageUrl, ErrorMessage = "Url for image is not valid")]
        public string ProfileImg { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [DataType(DataType.MultilineText)]
        [DescriptionValidation(MinimumAmountOfWords = 3, MaximumAmountOfCharacters = 255,
            ErrorMessage = "The description must contains a minimum of 3 words and a maximum of 255 characters.")]
        public string Description { get; set; }
        public int TotalSold { get; set; }

    }
}