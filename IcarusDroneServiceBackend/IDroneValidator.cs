namespace IcarusDroneServiceBackend {
    // Defines a method for validating drone input fields and returning an error message if any field is invalid.
    public interface IDroneValidator {
        bool ValidateInput(string name, string model, string problem, string costInput, out string? errorMessage);
    }
}