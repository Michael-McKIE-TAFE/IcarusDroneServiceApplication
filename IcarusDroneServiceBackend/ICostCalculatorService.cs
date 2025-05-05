namespace IcarusDroneServiceBackend {
    public interface ICostCalculatorService {
        double CalculateCost(string costInput, bool isExpress, out string? errorMessage);
    }
}