using System.Windows;

namespace IcarusDroneServiceApplication {
    public partial class DetailWindow : Window {
        public DetailWindow (string clientName, string droneModel, string serviceProblem, string serviceCost, string serviceTag){
            InitializeComponent();
            ClientNameTextBox.Text = clientName;
            DroneModelTextBox.Text = droneModel;
            ServiceProblemTextBox.Text = serviceProblem;
            ServiceCostTextBox.Text = serviceCost;
            ServiceTagTextBox.Text = serviceTag;
        }

        private void CloseButton_Click (object sender, RoutedEventArgs e){
            this.Close();
        }
    }
}