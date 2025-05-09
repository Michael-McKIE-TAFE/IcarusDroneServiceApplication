using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Channels;

namespace IcarusDroneServiceBackend {
    /// <summary>
    /// This code defines a cost calculation system using a strategy pattern via the ICostAdjustment interface.
    /// ICostAdjustment: Interface with a method to apply a cost adjustment and return an optional error message.
    /// SurchargeAdjustment: Applies a 15% surcharge to the input cost value.
    /// NoAdjustment: Returns the input cost without any changes.
    /// CostCalculatorService: Parses a string input for cost, determines if an express surcharge is needed, and applies
    /// the appropriate adjustment.Returns the adjusted and rounded cost or an error if the input is invalid.
    /// </summary>

    //  Applies a cost adjustment to the given value and returns the result.
    //  Outputs an error message if an exception occurs.
    public interface ICostAdjustment {
        double ApplyAdjustment(double value, out string? exception);
    }

    //  Applies a 15% surcharge to the input value.
    //  Returns -1 and sets an error message if an exception occurs
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

    //  Returns the input value without any adjustment.
    //  Returns -1 and sets an error message if an exception occurs.
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

    //  Parses the cost input and applies a surcharge if express is selected.
    //  Returns the adjusted, rounded cost or -1 on error.
    public class CostCalculatorService : ICostCalculatorService {
        public double CalculateCost(string costInput, bool isExpress, out string? errorMessage){
            errorMessage = null;

            try {
                if (!double.TryParse(costInput, out double value)){
                    errorMessage = "Invalid cost input.";
                    return -1;
                }

                ICostAdjustment costAdjustment = isExpress ? new SurchargeAdjustment() : new NoAdjustment();
                double cost = costAdjustment.ApplyAdjustment(value, out errorMessage);

                return errorMessage == null ? Math.Round(cost, 2) : -1;
            } catch (Exception ex){
                errorMessage = ex.Message;
                return -1;
            }
        }
    }
}