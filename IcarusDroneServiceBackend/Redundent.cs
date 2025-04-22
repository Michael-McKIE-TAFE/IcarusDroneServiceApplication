using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcarusDroneServiceBackend {
    internal class Redundent {
        /*private string clientName;
        private string droneModel;
        private string serviceProblem;
        private float serviceCost;
        private int serviceTag;
        private static List<Drone>? FinishedList = new List<Drone>();
        private static Queue<Drone>? RegularService = new Queue<Drone>();
        private static Queue<Drone>? ExpressService = new Queue<Drone>();

        public string DisplayClientName => GetClientName();
        public string DisplayDroneModel => GetDroneModel();
        public string DisplayServiceProblem => GetServiceProblem();
        public float DisplayServiceCost => GetServiceCost();
        public int DisplayServiceTag => GetServiceTag();

        //  Constructor with parameters
        public Drone (string clientName, string droneModel, string serviceProblem, float serviceCost, int serviceTag, bool priority){
            this.clientName = FormatTitleCase(clientName);
            this.droneModel = droneModel;
            this.serviceProblem = FormatSentenceCase(serviceProblem);
            this.serviceCost = serviceCost;
            this.serviceTag = serviceTag;

            if (priority){
                ExpressService.Enqueue(this);
            } else {
                RegularService.Enqueue(this);
            }
        }

        //  Default constructor for empty Drone objects
        public Drone (){ }

        // Getter and Setter for ClientName with Title Case formatting
        public string GetClientName (){
            return clientName;
        }

        public void SetClientName (string name){
            clientName = FormatTitleCase(name);
        }

        // Getter and Setter for DroneModel
        public string GetDroneModel (){
            return droneModel;
        }

        public void SetDroneModel (string model){
            droneModel = model;
        }

        // Getter and Setter for ServiceProblem with Sentence Case formatting
        public string GetServiceProblem (){
            return serviceProblem;
        }

        public void SetServiceProblem (string description){
            serviceProblem = FormatSentenceCase(description);
        }

        // Getter and Setter for ServiceCost
        public float GetServiceCost (){
            return serviceCost;
        }

        public void SetServiceCost (float cost){
            serviceCost = cost;
        }

        // Getter and Setter for ServiceTag
        public int GetServiceTag (){
            return serviceTag;
        }

        public void SetServiceTag (int tag){
            serviceTag = tag;
        }

        // Display method returning ClientName and ServiceCost
        public string DisplayDetails (){
            return $"{GetClientName()} - Service Cost: {GetServiceCost():C}";
        }

        //  Used for getting the express queue
        public Queue<Drone> GetExpressQueue (){
            return ExpressService;
        }

        //  Used for getting the regular queue
        public Queue<Drone> GetRegularQueue (){
            return RegularService;
        }

        //  Used for getting the list of completed repairs
        public List<Drone> GetFinishedList (){
            return FinishedList;
        }

        //  Used to dequeue the current drone and move it to the finished drones list
        public void DequeueDrone (bool priority){
            if (priority && ExpressService.Count > 0){
                FinishedList.Add(ExpressService.Dequeue());
            } else if (!priority && RegularService.Count > 0){
                FinishedList.Add(RegularService.Dequeue());
            }
        }

        //  Formats text to Title Case
        private string FormatTitleCase(string input){
            if (string.IsNullOrEmpty(input)){
                return input;
            }

            var words = input.ToLower().Split(' ');

            for (int i = 0; i < words.Length; i++){
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }

            return string.Join(" ", words);
        }

        //  Formats text to Sentence Case
        private string FormatSentenceCase (string input){
            if (string.IsNullOrEmpty(input)){
                return input;
            }

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }*/
    }
}
