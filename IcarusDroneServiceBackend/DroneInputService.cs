namespace IcarusDroneServiceBackend {
    /// <summary>
    /// This class handles the validation and registration of new drones. 
    /// It ensures all input fields are filled, calculates the service cost, and registers the drone with the DroneServiceManager. 
    /// If the input is invalid, it returns an error message.
    /// </summary>
    public class DroneInputService {
        private readonly IDroneValidator _validator;
        private readonly ICostCalculatorService _costCalculatorService;
        private readonly DroneServiceManager _dsManager;

        //  Initializes the service with a DroneServiceManager instance and a new CostCalculator.
        //  It sets up the necessary dependencies for drone registration and cost calculation.
        public DroneInputService (IDroneValidator validator, ICostCalculatorService costCalculatorService, DroneServiceManager dsManager){
            _dsManager = dsManager;
            _validator = validator;
            _costCalculatorService = costCalculatorService;
        }

        //  Attempts to register a drone by validating input fields for completeness and valid cost data.
        //  It calculates the final service cost using the CostCalculator, registers the drone with the
        //  DroneServiceManager, and returns a boolean indicating success. If validation fails, an appropriate
        //  error message is provided.
        public bool TryRegisterDrone(string name, string model, string problem, string costInput, int tagNumber, bool isExpress, out string? errorMessage){
            errorMessage = null;

            try {
                if (!_validator.ValidateInput(name, model, problem, costInput, out errorMessage)){
                    return false;
                }

                double cost = _costCalculatorService.CalculateCost(costInput, isExpress, out errorMessage);

                if (errorMessage != null) {
                    return false;
                }

                _dsManager.RegisterDrone(name.Trim(), model.Trim(), problem.Trim(), cost, tagNumber, isExpress, out errorMessage);
                return true;
            } catch (Exception ex){
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}