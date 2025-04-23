using IcarusDroneServiceBackend;
using System.Windows;

namespace IcarusDroneServiceApplication {
    /// <summary>
    /// This class defines the `DetailWindow` used to display the details of a drone. 
    /// The constructor accepts four string parameters (`clientName`, `droneModel`, `serviceProblem`, 
    /// and `serviceCost`) and sets the corresponding values in the text boxes within the window.
    /// The `CloseButton_Click` method handles the event when the close button is clicked, simply closing the window.
    /// This setup is for a simple detail view where you can see the drone's information, and the window can be closed with a button.
    /// </summary>
    public partial class DetailWindow : Window {
        //  The constructor for `DetailWindow` takes in four parameters—`clientName`,
        //  `droneModel`, `serviceProblem`, and `serviceCost`— and assigns them to the corresponding
        //  `TextBox` controls (`ClientNameTextBox`, `DroneModelTextBox`, `ServiceProblemTextBox`, and
        //  `ServiceCostTextBox`). This allows the `DetailWindow` to display the details of a drone when
        //  it is instantiated. The `TextBox` values are populated with the provided data when the window
        //  is created.
        public DetailWindow (string clientName, string droneModel, string serviceProblem, string serviceCost){
            InitializeComponent();
            ClientNameTextBox.Text = clientName;
            DroneModelTextBox.Text = droneModel;
            ServiceProblemTextBox.Text = serviceProblem;
            ServiceCostTextBox.Text = serviceCost;
        }

        //  The `CloseButton_Click` method is an event handler for the `Click` event of the
        //  `CloseButton` in the `DetailWindow`. When the button is clicked, the method simply
        //  calls `this.Close()`, which closes the window. This is typically used for closing
        //  modal windows in a UI, such as the `DetailWindow` in your case.
        private void CloseButton_Click (object sender, RoutedEventArgs e){
            this.Close();
        }
    }
}