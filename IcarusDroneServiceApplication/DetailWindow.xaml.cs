using System.Windows;

namespace IcarusDroneServiceApplication {
    //  Programming Requirements: 6.18
    public partial class DetailWindow : Window {
        public DetailWindow (string clientName, string droneModel, string serviceProblem, string serviceCost, string serviceTag){
            InitializeComponent();
            ClientNameTextBox.Text = clientName;
            DroneModelTextBox.Text = droneModel;
            ServiceProblemTextBox.Text = serviceProblem;
            ServiceCostTextBox.Text = serviceCost;
            ServiceTagTextBox.Text = serviceTag;
        }

        //  This method closes the Window when pressed.
        private void CloseButton_Click (object sender, RoutedEventArgs e){
            this.Close();
        }
    }
}