namespace IcarusDroneServiceBackend {
    /// <summary>
    /// The `Drone` class models a drone with attributes such as client name, model, service problem, 
    /// service cost, and service tag. It includes display properties for easy access to these attributes, 
    /// a constructor to initialize them, and a method to return a formatted string with key details like 
    /// the client's name and service cost. The class is primarily used for representing drone service information.
    /// </summary>
    public class Drone {
        private string clientName;
        private string droneModel;
        private string serviceProblem;
        private double serviceCost;
        private int serviceTag;

        public string DisplayClientName => clientName;
        public string DisplayDroneModel => droneModel;
        public string DisplayServiceProblem => serviceProblem;
        public double DisplayServiceCost => serviceCost;
        public int DisplayServiceTag => serviceTag;

        //  This constructor initializes a `Drone` object with the given parameters:
        //  client name, drone model, service problem, service cost, and service tag.
        //  It assigns these values to the respective properties of the `Drone` class
        //  for use in further operations.
        public Drone (string name, string model, string problem, double cost, int tag){
            clientName = name;
            droneModel = model;
            serviceProblem = problem;
            serviceCost = cost;
            serviceTag = tag;
        }

        //  This method returns a formatted string containing the client's name and the service
        //  cost of the drone, with the cost displayed as a currency value. It provides a concise
        //  summary of the drone's details for display purposes.
        //  Given that data binding is used, this function feels irrelevent, and is only here to
        //  meet the programming specs outlined in assessment doc.
        public string DisplayDetails (){
            return $"Client: {clientName}\t\t\tService Cost: {serviceCost:C}";
        }
    }
}