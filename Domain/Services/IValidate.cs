using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class IValidate
    {
        public Boolean ValidateTextLength(string word, int minimun, int maximum)
        {
            Boolean correct = false;
            if (word.Length >= minimun && word.Length <= maximum ? correct = true : correct=false) ;
            return correct;
        }

        public Boolean ValidateTextWords(string text, int wordsToHave)
        {
            var wordCounter = text.Split(' ');

            if (wordCounter.Count() < wordsToHave)
            {
                return false;
            }
            return true;
            
        }

        public Boolean ValidateOnlyLetters(string message)
        {
            Boolean correct = true;
            foreach (char c in message)
            {
                if (char.IsDigit(c)) correct = false;
                else if (char.IsSymbol(c)) correct = false;
            }
            return correct;
        }

        public Boolean ValidateUrl(string url)
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                if (request != null)
                {
                    request.Method = "HEAD";
                    var response = request.GetResponse() as HttpWebResponse;
                    return response != null && (response.StatusCode == HttpStatusCode.OK);
                }
            }
            catch
            {
                return false;
            }
            return false;
        }


        public String Embed(string urlVideo)
        {
            const string pattern = @"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)";
            const string replacement = "http://www.youtube.com/embed/$1";

            var rgx = new Regex(pattern);
            var result = rgx.Replace(urlVideo, replacement);
            return result;
        }
    }
}
