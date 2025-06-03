namespace IcarusDroneServiceBackend {
    internal class TextFormatter {
        public string FormatSentence (string input){
            if (string.IsNullOrEmpty(input)){
                return input;
            }

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

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
