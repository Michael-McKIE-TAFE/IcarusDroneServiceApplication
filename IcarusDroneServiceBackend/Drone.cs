namespace IcarusDroneServiceBackend {
    public class Drone {
        public string ClientName { get; }
        public string DroneModel { get; }
        public string ServiceProblem { get; }
        public double ServiceCost { get; }
        public int ServiceTag { get; }

        public string DisplayClientName => ClientName;
        public string DisplayDroneModel => DroneModel;
        public string DisplayServiceProblem => ServiceProblem;
        public double DisplayServiceCost => ServiceCost;
        public int DisplayServiceTag => ServiceTag;

        public Drone (string clientName, string droneModel, string serviceProblem, double serviceCost, int serviceTag){
            ClientName = clientName;
            DroneModel = droneModel;
            ServiceProblem = serviceProblem;
            ServiceCost = serviceCost;
            ServiceTag = serviceTag;
        }

        public string DisplayDetails (){
            return $"Client: {ClientName}\t\t\tService Cost: {ServiceCost:C}";
        }
    }
}