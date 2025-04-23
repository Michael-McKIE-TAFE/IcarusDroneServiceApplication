namespace IcarusDroneServiceBackend {
    /// <summary>
    /// The `Drone` class models a drone with attributes such as client name, model, service problem, 
    /// service cost, and service tag. It includes display properties for easy access to these attributes, 
    /// a constructor to initialize them, and a method to return a formatted string with key details like 
    /// the client's name and service cost. The class is primarily used for representing drone service information.
    /// </summary>
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

        //  This constructor initializes a `Drone` object with the given parameters:
        //  client name, drone model, service problem, service cost, and service tag.
        //  It assigns these values to the respective properties of the `Drone` class
        //  for use in further operations.
        public Drone (string clientName, string droneModel, string serviceProblem, double serviceCost, int serviceTag){
            ClientName = clientName;
            DroneModel = droneModel;
            ServiceProblem = serviceProblem;
            ServiceCost = serviceCost;
            ServiceTag = serviceTag;
        }

        //  This method returns a formatted string containing the client's name and the service
        //  cost of the drone, with the cost displayed as a currency value. It provides a concise
        //  summary of the drone's details for display purposes.
        public string DisplayDetails (){
            return $"Client: {ClientName}\t\t\tService Cost: {ServiceCost:C}";
        }
    }
}