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
        public bool TryRegisterDrone(string name, string model, string problem, double costInput, int tagNumber, bool isExpress, out string errorMessage){
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(problem) || double.IsNaN(costInput)){
                errorMessage = "Please fill out all fields.";
                return false;
            }

            _dsManager.RegisterDrone(name.Trim(), model.Trim(), problem.Trim(), costInput, tagNumber, isExpress);
            return true;
        }

        public double GetCostInput(string text, bool priority){
            CostCalculator calculator = new CostCalculator();
            double returnValue = 0;

            if (double.TryParse(text, out double value)){
                if (priority){
                    returnValue = Math.Round(calculator.CalculateCost(value, true), 2);
                } else {
                    returnValue = Math.Round(calculator.CalculateCost(value, false), 2);
                }

                return returnValue;
            } else {
                return -1;
            }
        }
    }
}