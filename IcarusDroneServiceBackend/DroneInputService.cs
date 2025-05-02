namespace IcarusDroneServiceBackend {
    /// <summary>
    /// This class handles the validation and registration of new drones. 
    /// It ensures all input fields are filled, calculates the service cost, and registers the drone with the DroneServiceManager. 
    /// If the input is invalid, it returns an error message.
    /// </summary>
    public class DroneInputService {
        private readonly DroneServiceManager _dsManager;

        //  Initializes the service with a DroneServiceManager instance and a new CostCalculator.
        //  It sets up the necessary dependencies for drone registration and cost calculation.
        public DroneInputService (DroneServiceManager dsManager){
            _dsManager = dsManager;
        }

        //  Attempts to register a drone by validating input fields for completeness and valid cost data.
        //  It calculates the final service cost using the CostCalculator, registers the drone with the
        //  DroneServiceManager, and returns a boolean indicating success. If validation fails, an appropriate
        //  error message is provided.
        public bool TryRegisterDrone(string name, string model, string problem, double costInput, int tagNumber, bool isExpress, out string? errorMessage){
            errorMessage = null;

            try {
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(problem) || double.IsNaN(costInput)){
                    errorMessage = "Please fill out all fields.";
                    return false;
                }

                _dsManager.RegisterDrone(name.Trim(), model.Trim(), problem.Trim(), costInput, tagNumber, isExpress, out errorMessage);

                return true;
            } catch (Exception ex){
                errorMessage = ex.Message;
                return false;
            }
        }

        //  The `GetCostInput` function parses a string input as a double and calculates a cost based on priority.
        //  It uses the `CostCalculator` class to determine the cost, returning the result or -1 if an error occurs,
        //  and optionally provides an error message.
        public double GetCostInput(string text, bool priority, out string? message) {
            CostCalculator calculator = new CostCalculator();
            double returnValue = 0;
            string? error = null;
            message = null;

            try {
                if (double.TryParse(text, out double value)) {
                    if (priority) {
                        double tempValue = Math.Round(calculator.CalculateCost(value, true, out error), 2);

                        if (error != null) {
                            returnValue = -1;
                        } else {
                            returnValue = tempValue;
                        }
                    } else {
                        double tempValue = Math.Round(calculator.CalculateCost(value, false, out error), 2);

                        if (error != null) {
                            returnValue = -1;
                        } else {
                            returnValue = tempValue;
                        }
                    }
                }

                return returnValue;
            } catch (Exception ex){
                message = error + ex.Message;
                return returnValue;
            }
        }
    }
}