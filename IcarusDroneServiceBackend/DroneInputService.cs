namespace IcarusDroneServiceBackend {
    public class DroneInputService {
        private readonly CostCalculator _calculator;
        private readonly DroneServiceManager _dsManager;

        public DroneInputService (DroneServiceManager dsManager){
            _dsManager = dsManager;
            _calculator = new CostCalculator();
        }

        public bool TryRegisterDrone(string name, string model, string problem, double costInput, int tagNumber, bool isExpress, out string errorMessage){
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(problem) || double.IsNaN(costInput)){
                errorMessage = "Please fill out all fields.";
                return false;
            }

            double calculatedCost = Math.Round(_calculator.CalculateCost(costInput, isExpress), 2);

            _dsManager.RegisterDrone(name.Trim(), model.Trim(), problem.Trim(), calculatedCost, tagNumber, isExpress);
            return true;
        }
    }
}