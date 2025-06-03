namespace IcarusDroneServiceBackend {
    //  Programming Requirements: 6.18
    public class DisplayCoordinator {
        private const double surchargeAmount = 0.15;
        //  Programming Requirements: 6.2
        private readonly List<Drone>? finishedList = [];
        //  Programming Requirements: 6.3
        private readonly Queue<Drone>? regularService = new();
        //  Programming Requirements: 6.4
        private readonly Queue<Drone>? expressService = new();
        
        //  This method attempts to register a drone based on the passed parammeters.
        public bool TryRegisterDrone (string name, string model, string problem, string costInput, int tagNumber, bool isExpress){
            try {
                TextFormatter textFormatter = new();
                string formattedClient = textFormatter.FormatTitle(name);
                string formattedProblem = textFormatter.FormatSentence(problem);
                double cost = ReturnCost(costInput, isExpress);

                if (cost == -1){
                    return false;
                }

                bool success = AddDrone(formattedClient, model, formattedProblem, cost, tagNumber, isExpress);

                return success;
            } catch { 
                return false;    
            }
        }

        //  This method creates a drone object and adds it to either the reguar or express queue.
        private bool AddDrone (string name, string model, string problem, double cost, int tag, bool express){
            try {
                Drone drone = new(name, model, problem, cost, tag);

                if (express){
                    expressService?.Enqueue(drone);
                } else {
                    regularService?.Enqueue(drone);
                }

                return true;
            } catch { 
                return false;    
            }
        }

        //  Programming Requirements: 6.6
        //  Method was made static to fix "CA1822: Member does not access instance data and can be marked static"
        //  warning as per Visual Studio recommendation.
        private static double ReturnCost (string inValue, bool isExpress){
            if (!double.TryParse(inValue, out var cost)){
                return -1;
            }
            
            if (isExpress){
                cost = cost + (cost * surchargeAmount);
            }
            
            return Math.Round(cost, 2);
        }

        //  Programmign Requirements: 6.14 & 6.15
        //  This method dequeues a drone in either the regular or express queue depending on it's
        //  priority. It will return false if an error occurs.
        public bool DequeueDrone (bool priority){
            if (priority && expressService?.Count > 0){
                finishedList?.Add(expressService.Dequeue());
                return true;
            } 
            
            if (!priority && regularService?.Count > 0){
                finishedList?.Add(regularService.Dequeue());
                return true;
            }

            return false;
        }

        //  Programming Requirements: 6.16
        //  This method removes a drone from the finished work list that matches the service tag
        //  number of the desired drone.
        public bool RemoveDroneFromFinishedList (int tagNum){
            var droneToRemove = finishedList?.FirstOrDefault(drone => drone.ServiceTag == tagNum);

            if (droneToRemove != null){
                finishedList?.Remove(droneToRemove);
                return true;
            }

            return false;
        }

        //  This method returns the finished work list.
        public List<(string, string, string, double, int)> ReturnFinishedList(){
            var list = new List<(string, string, string, double, int)>();
            
            if (finishedList != null){
                foreach (var drone in finishedList){
                    var droneTuple = (drone.ClientName, drone.DroneModel, drone.ServiceProblem, drone.ServiceCost, drone.ServiceTag);
                    list.Add(droneTuple);
                }
                return list; 
            }
            
            return list;
        }

        //  Programming Requirements: 6.9
        //  This method returns the express service queue as a queue of Tuples.
        //  This was done to remove all references of Drone from the WPF applicarion.
        //  Tuple breakdown: Item1 = Name, Item2 = Model, Item3 = Problem, Item4 = Cost, Item5 = Tag.
        public Queue<(string, string, string, double, int)> ReturnExpressQueue (){
            var queue = new Queue<(string, string, string, double, int)>();

            if (expressService != null){
                foreach (var drone in expressService){
                    var droneTuple = (drone.ClientName, drone.DroneModel, drone.ServiceProblem, drone.ServiceCost, drone.ServiceTag);
                    queue.Enqueue(droneTuple);
                }

                return queue;
            }

            return queue;
        }

        //  Programming Requirements: 6.8
        //  This method returns the regular service queue as a queue of Tuples.
        //  This was done to remove all references of Drone from the WPF applicarion.
        //  Tuple breakdown: Item1 = Name, Item2 = Model, Item3 = Problem, Item4 = Cost, Item5 = Tag.
        public Queue<(string, string, string, double, int)> ReturnRegularQueue (){
            var queue = new Queue<(string, string, string, double, int)>();

            if (regularService != null){
                foreach (var drone in regularService){
                    var droneTuple = (drone.ClientName, drone.DroneModel, drone.ServiceProblem, drone.ServiceCost, drone.ServiceTag);
                    queue.Enqueue(droneTuple);
                }
                return queue;
            }

            return queue;
        }
    }
}