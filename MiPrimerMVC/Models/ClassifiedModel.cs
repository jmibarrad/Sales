using System.ComponentModel.DataAnnotations;
using MiPrimerMVC.ValidationAttributes;

namespace MiPrimerMVC.Models
{
    public class ClassifiedModel
    {
        public string Category { get; set; }

        [Required(ErrorMessage = "Article´s Name is required")]
        [DataType(DataType.Text)]
        [DescriptionValidation(MinimumAmountOfWords = 1, MaximumAmountOfCharacters = 100,
            ErrorMessage = "The description must contains a minimum of 1 word and a maximum of 100 characters.")]
        public string Article { get; set; }
        public string ArticleModel { get; set; }

        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00")]
        [Required(ErrorMessage = "Price is Required")]
        [DataType(DataType.Currency)]
        public float Price { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [DataType(DataType.Text)]
        public string Location { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [DataType(DataType.MultilineText)]
        [DescriptionValidation(MinimumAmountOfWords = 3, MaximumAmountOfCharacters = 255,
            ErrorMessage = "The description must contains a minimum of 3 words and a maximum of 255 characters.")]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl, ErrorMessage = "Url for image is not valid")]
        public string UrlImage { get; set; }


        public string UrlVideo { get; set; }
    }
}