namespace IcarusDroneServiceBackend {
    public class DroneServiceManager {
        public List<Drone> GetFinishedList() => new(finishedList);
        public Queue<Drone> GetExpressQueue() => new(expressService);
        public Queue<Drone> GetRegularQueue() => new(regularService);
        
        private readonly List<Drone>? finishedList = new List<Drone>();
        private readonly Queue<Drone>? regularService = new Queue<Drone>();
        private readonly Queue<Drone>? expressService = new Queue<Drone>();
        private readonly TextFormatter _textFormatter;

        public DroneServiceManager (){
            _textFormatter = new TextFormatter();
        }

        public void RegisterDrone (string clientName, string droneModel, string serviceProblem, double cost, int tag, bool priority){
            var formattedClient = _textFormatter.FormatTitleCase(clientName);
            var formattedProblem = _textFormatter.FormatSentenceCase(serviceProblem);

            Drone drone = new Drone(formattedClient, droneModel, formattedProblem, cost, tag);

            if (priority){
                expressService?.Enqueue(drone);
            } else {
                regularService?.Enqueue(drone);
            }
        }

        public void DequeueDrone (bool priority){
            if (priority && expressService?.Count > 0){
                finishedList?.Add(expressService.Dequeue());
            }
            else if (!priority && regularService?.Count > 0){
                finishedList?.Add(regularService.Dequeue());
            }
        }

        public void RemoveDroneFromFinishedList (Drone drone){
            finishedList?.Remove(drone);
        }
    }
}
