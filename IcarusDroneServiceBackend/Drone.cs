namespace IcarusDroneServiceBackend {
    //  Programming Requirements: 6.1
    public class Drone (string name, string model, string problem, double cost, int tag){
        private readonly string clientName = name;
        private readonly string droneModel = model;
        private readonly string serviceProblem = problem;
        private readonly double serviceCost = cost;
        private readonly int serviceTag = tag;

        // Read-only property that returns the value of the private field clientName.
        public string ClientName => clientName;

        // Read-only property that returns the value of the private field droneModel.
        public string DroneModel => droneModel;

        // Read-only property that returns the value of the private field serviceProblem.
        public string ServiceProblem => serviceProblem;

        // Read-only property that returns the value of the private field serviceCost.
        public double ServiceCost => serviceCost;

        // Read-only property that returns the value of the private field serviceTag.
        public int ServiceTag => serviceTag;

        //  This is here to meet programming requirements, its unused as the data is retreived
        //  through the readonly properties.
        //  This method returns a string that contains the Drones clients name and cost.
        public string DisplayDetails (){
            return $"Client: {clientName}\t\t\tService Cost: {serviceCost:C}";
        }
    }
}