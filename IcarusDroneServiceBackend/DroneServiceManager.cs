namespace IcarusDroneServiceBackend {
    /// <summary>
    /// This class manages drone registration, service queues, and finished drone records. 
    /// It supports registering drones, dequeuing them from service queues, and removing finished drones. 
    /// The class uses a TextFormatter to format input fields and maintains separate queues for express 
    /// and regular services, along with a list of finished drones.
    /// </summary>
    public class DroneServiceManager {
        private readonly List<Drone>? finishedList = new List<Drone>();
        private readonly Queue<Drone>? regularService = new Queue<Drone>();
        private readonly Queue<Drone>? expressService = new Queue<Drone>();
        private readonly TextFormatter _textFormatter;

        //  Initializes the instance of `TextFormatter`, which is used to format client names and
        //  service problems when registering drones. This ensures consistent formatting for the
        //  data handled by the service manager.
        public DroneServiceManager (){
            _textFormatter = new TextFormatter();
        }

        //  This method formats the provided client name and service problem using a TextFormatter instance.
        //  It then creates a new Drone object with the formatted data and other provided information (drone
        //  model, cost, tag, and priority). Depending on whether the drone is marked as "express" or not, it
        //  adds the drone to either the expressService or regularService queue.
        public bool RegisterDrone (string clientName, string droneModel, string serviceProblem, double cost, int tag, bool priority, out string? message){
            message = null;

            try {
                string? eMessage = null;
                var formattedClient = _textFormatter.FormatTitleCase(clientName, out eMessage);

                if (eMessage != null){
                    message = eMessage;
                    return false;
                } else {
                    var formattedProblem = _textFormatter.FormatSentenceCase(serviceProblem, out eMessage);

                    if (eMessage != null){
                        message = eMessage;
                        return false;
                    } else {
                        Drone drone = new Drone(formattedClient, droneModel, formattedProblem, cost, tag);

                        if (priority){
                            expressService?.Enqueue(drone);
                        } else {
                            regularService?.Enqueue(drone);
                        }

                        return true;
                    }
                }
            } catch (Exception ex){
                message = ex.Message;
                return false;
            }
        }

        //  This method removes a drone from either the `expressService` or `regularService` queue based on
        //  the `priority` parameter. If the priority is true (express), it dequeues a drone from the
        //  `expressService` queue and adds it to the `finishedList`. If the priority is false (regular),
        //  it does the same with the `regularService` queue. The method ensures that it only dequeues from
        //  the respective queues if they contain at least one item.
        public void DequeueDrone (bool priority, out string? message){
            message = null;

            try {
                if (priority && expressService?.Count > 0){
                    finishedList?.Add(expressService.Dequeue());
                } 
                else if (!priority && regularService?.Count > 0){
                    finishedList?.Add(regularService.Dequeue());
                }
            } catch (Exception ex){
                message = ex.Message;
            }
        }

        //  This method removes a specified `drone` from the `finishedList`. It uses the null-conditional operator
        //  (`?.`) to ensure that the `finishedList` is not null before attempting to remove the drone. If `finishedList`
        //  is not null, the specified drone is removed from the list.
        public void RemoveDroneFromFinishedList (Drone drone, out string? message){
            message = null;
            try {
                finishedList?.Remove(drone);
            } catch (Exception ex){
                message = ex.Message;
            }
        }

        //  Returns the finished work list
        public List<Drone> GetFinishedList (out string? message){
            message = null;

            try {
                if (finishedList == null){
                    return new List<Drone>();
                }

                return finishedList;
            } catch (Exception ex){
                message = ex.Message;
                return new List<Drone>();
            }
        }

        //  Returns the express service queue
        //  Returns an empty queue to avoid a possible crash if the
        //  express service queue happens to be null.
        public Queue<Drone> GetExpressQueue (out string? message){
            message = null;

            try {
                if (expressService == null){
                    return new Queue<Drone>();
                }

                return expressService;
            } catch (Exception ex){
                message = ex.Message;
                return new Queue<Drone>();
            }
        }

        //  Returns the regular service queue.
        public Queue<Drone> GetRegularQueue (out string? message){
            message = null;
            try {
                if (regularService == null){
                    return new Queue<Drone>();
                }

                return regularService;
            } catch (Exception ex){
                message = ex.Message;
                return new Queue<Drone>();
            }
        }
    }
}