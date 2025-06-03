namespace IcarusDroneServiceBackend {
    /// <summary>
    /// The `Drone` class models a drone with attributes such as client name, model, service problem, 
    /// service cost, and service tag. It includes display properties for easy access to these attributes, 
    /// a constructor to initialize them, and a method to return a formatted string with key details like 
    /// the client's name and service cost. The class is primarily used for representing drone service information.
    /// </summary>
    public class Drone {
        private readonly string clientName;
        private readonly string droneModel;
        private readonly string serviceProblem;
        private readonly double serviceCost;
        private readonly int serviceTag;

        public string ClientName => clientName;
        public string DroneModel => droneModel;
        public string ServiceProblem => serviceProblem;
        public double ServiceCost => serviceCost;
        public int ServiceTag => serviceTag;

        public Drone (string name, string model, string problem, double cost, int tag){
            clientName = name;
            droneModel = model;
            serviceProblem = problem;
            serviceCost = cost;
            serviceTag = tag;
        }

        public string DisplayDetails (){
            return $"Client: {clientName}\t\t\tService Cost: {serviceCost:C}";
        }
    }
}