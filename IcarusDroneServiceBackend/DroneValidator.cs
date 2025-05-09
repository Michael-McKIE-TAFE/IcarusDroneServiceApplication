namespace IcarusDroneServiceBackend {
    public class DroneValidator : IDroneValidator {
        // Validates that all input fields are filled.
        // Returns false and sets an error message if any are empty or an exception occurs.
        public bool ValidateInput(string name, string model, string problem, string costInput, out string? errorMessage){
            errorMessage = null;

            try {
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(problem) || string.IsNullOrWhiteSpace(costInput)) {
                    errorMessage = "Please fill out all fields.";
                    return false;
                }

                return true;
            } catch (Exception ex){
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}