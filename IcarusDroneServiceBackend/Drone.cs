namespace IcarusDroneServiceBackend {
    //  Programming Requirements: 6.1
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