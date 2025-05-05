namespace IcarusDroneServiceBackend {
    public interface IDroneValidator {
        bool ValidateInput(string name, string model, string problem, string costInput, out string? errorMessage);
    }
}