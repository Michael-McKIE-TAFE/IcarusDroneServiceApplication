namespace IcarusDroneServiceBackend {
    // Defines a method for calculating the cost based on input and whether the
    // service is express, returning an error message if applicable.
    public interface ICostCalculatorService {
        double CalculateCost(string costInput, bool isExpress, out string? errorMessage);
    }
}