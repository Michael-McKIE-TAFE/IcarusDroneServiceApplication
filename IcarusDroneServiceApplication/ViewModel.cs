namespace IcarusDroneServiceApplication {
    //  This class is used to help bind the Drone data through an observable object located in
    //  the LibrartCoordinator.cs class. The variable names corrispond to the binding names in
    //  MainWindow.xaml
    public class ViewModel {
        public string? DisplayClientName { get; set; }
        public string? DisplayDroneModel { get; set; }
        public string? DisplayServiceProblem { get; set; }
        public double? DisplayServiceCost { get; set; }
        public int? DisplayServiceTag { get; set; }
    }
}