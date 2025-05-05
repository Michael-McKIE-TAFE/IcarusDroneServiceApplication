namespace IcarusDroneServiceBackend {
    public class DroneValidator : IDroneValidator {
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