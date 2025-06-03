namespace IcarusDroneServiceBackend {
    //  Programming Requirements: 6.1
    internal class TextFormatter {
        //  This method formats the inputed string into sentance format.
        //  It does this by changing the first character to upper case
        //  and the remaining characters to lower case.
        public string FormatSentence (string input){
            if (string.IsNullOrEmpty(input)){
                return input;
            }

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        //  This method formats the inputed string into a title.
        //  It achieves this by changing the first character of each word to
        //  uppercase and the reamining characters in each work to be lower case.
        public string FormatTitle (string input){
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
