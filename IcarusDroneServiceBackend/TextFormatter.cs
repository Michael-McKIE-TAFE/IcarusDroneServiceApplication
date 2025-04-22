using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcarusDroneServiceBackend {
    internal class TextFormatter {
        //  This function takes a string and formats it into sentence case.
        //  If the input is null or empty, it returns the input unchanged.
        //  Otherwise, it capitalizes the first character and converts the
        //  rest of the string to lowercase.
        public string FormatSentenceCase (string input){
            if (string.IsNullOrEmpty(input)){
                return input;
            }

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        //  This function converts a string to title case. It first checks if the
        //  input is null or empty and returns it as-is. Otherwise, it converts the
        //  entire string to lowercase, splits it into words, capitalizes the first
        //  letter of each word, and then joins the words back into a single string
        //  with spaces.
        public string FormatTitleCase(string input){
            if (string.IsNullOrEmpty(input)){
                return input;
            }

            var words = input.ToLower().Split(' ');

            for (int i = 0; i < words.Length; i++){
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }

            return string.Join(" ", words);
        }
    }
}
