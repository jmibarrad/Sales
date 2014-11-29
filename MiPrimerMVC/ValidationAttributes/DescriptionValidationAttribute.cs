using System.ComponentModel.DataAnnotations;

namespace MiPrimerMVC.ValidationAttributes
{
    public class DescriptionValidationAttribute : ValidationAttribute
    {
         public int MinimumAmountOfWords { get; set; }
        public int MaximumAmountOfCharacters { get; set; }

        public DescriptionValidationAttribute()
        {
            MinimumAmountOfWords = 1;
            MaximumAmountOfCharacters = 50;
        }
        public override bool IsValid(object value)
        {
            var strValue = (string) value;
            var wordsInStrValue = strValue.Split(' ');
            return wordsInStrValue.Length >= MinimumAmountOfWords && strValue.Length <= MaximumAmountOfCharacters;
        }
    }
    
}