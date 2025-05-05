namespace IcarusDroneServiceBackend {
    public interface ICostAdjustment {
        double ApplyAdjustment(double value, out string? exception);
    }

    public class SurchargeAdjustment : ICostAdjustment {
        private const double SurchargePercentage = 0.15;

        public double ApplyAdjustment(double value, out string? exception){
            exception = null;
            try{
                return value + (value * SurchargePercentage);
            } catch (Exception ex){
                exception = ex.Message;
                return -1;
            }
        }
    }

    public class NoAdjustment : ICostAdjustment {
        public double ApplyAdjustment(double value, out string? exception){
            exception = null;
            try {
                return value;
            } catch (Exception ex){
                exception = ex.Message;
                return -1;
            }
        }
    }
    
    public class CostCalculatorService : ICostCalculatorService {
        public double CalculateCost(string costInput, bool isExpress, out string? errorMessage){
            errorMessage = null;

            if (!double.TryParse(costInput, out double value)){
                errorMessage = "Invalid cost input.";
                return -1;
            }

            ICostAdjustment costAdjustment = isExpress ? new SurchargeAdjustment() : new NoAdjustment();
            double cost = costAdjustment.ApplyAdjustment(value, out errorMessage);

            return errorMessage == null ? Math.Round(cost, 2) : -1;
        }
    }
}