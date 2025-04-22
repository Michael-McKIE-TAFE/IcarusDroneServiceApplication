namespace IcarusDroneServiceBackend {
    public class Drone {
        private string clientName;
        private string droneModel;
        private string serviceProblem;
        private double serviceCost;
        private int serviceTag;
        private static List<Drone>? FinishedList = new List<Drone>();
        private static Queue<Drone>? RegularService = new Queue<Drone>();
        private static Queue<Drone>? ExpressService = new Queue<Drone>();

        //  This group of properties provides access to specific internal data of an object.
        //  They are read-only, meaning their values can be retrieved but cannot be modified
        //  from outside the class. (As each property only has the getter syntax =>)
        //  These values are set during object initialization and remain unchanged thereafter,
        //  ensuring controlled access to the object's state.
        public string DisplayClientName => clientName;
        public string DisplayDroneModel => droneModel;
        public string DisplayServiceProblem => serviceProblem;
        public double DisplayServiceCost => serviceCost;
        public int DisplayServiceTag => serviceTag;

        //  Default constructor for empty Drone objects
        public Drone (){ }

        //  Initializes a new Drone object with provided details such as client name, drone model, service problem,
        //  service cost, service tag, and priority status.
        //  Formats the clientName to title case and serviceProblem to sentence case using a TextFormatter.
        //  Based on the priority flag, it enqueues the drone into either the ExpressService or RegularService queue.
        public Drone (string clientName, string droneModel, string serviceProblem, double serviceCost, int serviceTag, bool priority){
            TextFormatter formatter = new TextFormatter();
            
            this.clientName = formatter.FormatTitleCase(clientName);
            this.droneModel = droneModel;
            this.serviceProblem = formatter.FormatSentenceCase(serviceProblem);
            this.serviceCost = serviceCost;
            this.serviceTag = serviceTag;

            if (priority){
                ExpressService.Enqueue(this);
            } else {
                RegularService.Enqueue(this);
            }
        }

        //  Returns a formatted string that includes the client's display name and the service cost.
        //  The service cost is formatted as currency using the :C format specifier (e.g., $150.00).
        public string DisplayDetails (){
            return $"{DisplayClientName} - Service Cost: {DisplayServiceCost:C}";
        }

        //  Returns the ExpressService queue, which contains the drones that are prioritized for express service.
        public Queue<Drone> GetExpressQueue (){
            return ExpressService;
        }

        //  Returns the RegularService queue, which holds the drones that are scheduled for regular service.
        public Queue<Drone> GetRegularQueue (){
            return RegularService;
        }

        //  Returns the FinishedList, which contains the drones that have completed their service.
        public List<Drone> GetFinishedList (){
            return FinishedList;
        }

        //  This method dequeues a drone from either the ExpressService or RegularService queue based on the priority flag:
        //  If priority is true and the ExpressService queue has drones, it dequeues a drone and adds it to the FinishedList.
        //  If priority is false and the RegularService queue has drones, it dequeues a drone and adds it to the FinishedList.
        public void DequeueDrone (bool priority){
            if (priority && ExpressService.Count > 0){
                FinishedList.Add(ExpressService.Dequeue());
            } else if (!priority && RegularService.Count > 0){
                FinishedList.Add(RegularService.Dequeue());
            }
        }
    }
}