namespace IcarusDroneServiceBackend {
    /// <summary>
    /// This class includes two methods: `FormatSentenceCase` and `FormatTitleCase`. 
    /// The `FormatSentenceCase` method capitalizes the first letter of a string and converts the rest 
    /// to lowercase. Meanwhile, the `FormatTitleCase` method ensures that the first letter of each word
    /// in the string is capitalized, and the remaining letters are lowercase. Both methods check for 
    /// null or empty strings, returning the input unchanged if it's null or empty.
    /// </summary>
    internal class TextFormatter {
        //  This method takes a string input and ensures that the first character is
        //  capitalized while the rest of the string is converted to lowercase.
        //  If the input string is null or empty, it returns the string as is. Otherwise,
        //  it returns the input with the first letter in uppercase and the remaining
        //  letters in lowercase.
        public string FormatSentenceCase (string input){
            if (string.IsNullOrEmpty(input)){
                return input;
            }

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        //  The `FormatTitleCase` method takes a string input and converts it to title case,
        //  where each word's first letter is capitalized and the rest are lowercase. It first
        //  converts the entire input to lowercase, splits the string into individual words,
        //  and then capitalizes the first letter of each word. Finally, it joins the words back
        //  together with spaces in between. If the input is null or empty, it simply returns the
        //  input as is.
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
