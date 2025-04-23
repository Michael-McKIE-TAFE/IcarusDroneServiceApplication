using IcarusDroneServiceBackend;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IcarusDroneServiceApplication {
    public partial class MainWindow : Window {
        private DroneServiceManager dsManager;
        private ObservableCollection<Drone> finishedDrones;
        private DroneInputService inputService;

        //  This constructor initializes the components of the MainWindow by calling InitializeComponent().
        //  It then creates a new instance of the Drone class.
        public MainWindow (){
            InitializeComponent();
            dsManager = new DroneServiceManager();
            inputService = new DroneInputService(dsManager);    
            finishedDrones = new ObservableCollection<Drone>(dsManager.GetFinishedList());
            FinishedWorkList.ItemsSource = finishedDrones;
        }

        //  This method is triggered when a TextBox receives focus.
        //  It clears the content of the TextBox if the sender is a valid TextBox object.
        //  The TextBox is cast from the sender, and if the cast is successful,
        //  it calls Clear() to remove any text inside.
        private void TextBox_GotFocus (object sender, RoutedEventArgs e){
            TextBox? textBox = sender as TextBox;

            if (textBox != null){
                textBox.Clear();
            }
        }

        //  This method updates the StatusDetailsText element based on the provided status.
        //  If the status is true, it sets the text color to green (using a resource from the application),
        //  and if false, it sets the text color to red.
        //  It also updates the text content of StatusDetailsText with the provided message.
        public async void SetStatusDetails (string message, bool status) {
            if (status){
                StatusDetailsText.Foreground = (Brush)Application.Current.Resources["greenText"];
            } else {
                StatusDetailsText.Foreground = (Brush)Application.Current.Resources["redText"];
            }

            StatusDetailsText.Text = message;

            await Task.Delay(2000);
            StatusDetailsText.Foreground = (Brush)Application.Current.Resources["whiteText"];
            StatusDetailsText.Text = "Ready...";
        }

        //  This method handles the click event for adding a new item.
        //  It first checks if all required fields are filled and if a service type has been selected.
        //  If any conditions are not met, it displays an error message with red text. If all conditions are satisfied,
        //  it creates a new record and clears the input fields.
        private void AddNewItem_Click (object sender, RoutedEventArgs e){
            bool serviceTypeSelected = GetServicePriority();

            if (NameField.Text == "Name" || DetailsField.Text == "Details" || ProblemField.Text == "Problem" || ServiceCostTextBox.Text == "Cost" || !serviceTypeSelected){
                SetStatusDetails("Error: Unable to add the new item. Please enter all of the fields.", false);
            } else {                
                CreateRecord();
                ClearInputFields();
            }
        }

        //  This method creates a new Drone record based on the input fields.
        //  It calculates the service cost using a CostCalculator, with adjustments
        //  based on whether the express service is selected. It then creates a new
        //  Drone object with the provided details and assigns it to droneRef.
        //  The service tag is advanced, and the new drone is added to the appropriate
        //  service queue.
        private void CreateRecord (){
            string name = NameField.Text.Trim();
            string model = DetailsField.Text.Trim();
            string problem = ProblemField.Text.Trim();
            double amount = ReturnServiceValue();
            int tagNumber = (TagNumber.Value ?? 0);
            bool isExpress = ExpressSelected.IsChecked == true;

            if (inputService.TryRegisterDrone(name, model, problem, amount, tagNumber, isExpress, out string errorMsg)){
                AdvanceServiceTag(TagNumber);
                AddToQueue();
            } else {
                SetStatusDetails(errorMsg, false);
            }
        }

        //  This function, calculates the cost of a service based on user input.
        //  It retrieves a value from a text box (ServiceCostTextBox), checks if it's a valid number,
        //  and then calculates the cost using the CostCalculator class. If the "Express" option is
        //  selected (ExpressSelected.IsChecked), it calculates with express pricing; otherwise, it
        //  calculates with standard pricing. The result is rounded to two decimal places and returned.
        //  If the input is invalid, it displays an error message.
        private double ReturnServiceValue (){
            CostCalculator calculator = new CostCalculator();
            double returnValue = 0;

            if (double.TryParse(ServiceCostTextBox.Text, out double value)){
                if (ExpressSelected.IsChecked == true){
                    returnValue = calculator.CalculateCost(value, true);
                    returnValue = Math.Round(returnValue, 2);
                } else {
                    returnValue = calculator.CalculateCost(value, false);
                    returnValue = Math.Round(returnValue, 2);
                }
            } else {
                SetStatusDetails("Please enter a valid service cost.", false);
            }

            return returnValue;
        }

        //  This method advances the value of the provided IntegerUpDown control by its increment value.
        //  If the new value exceeds the maximum allowed value, it resets the value to the minimum, or
        //  defaults to 100 if no minimum is specified. The method ensures that the control's value stays
        //  within the defined range.
        private void AdvanceServiceTag (Xceed.Wpf.Toolkit.IntegerUpDown control){
            if (control.Value.HasValue && control.Increment.HasValue){
                int currentValue = control.Value.Value;
                int increment = control.Increment.Value;
                int newValue = currentValue + increment;

                if (control.Maximum.HasValue && newValue > control.Maximum.Value){
                    newValue = control.Minimum ?? 100;
                }

                control.Value = newValue;
            }
        }

        //  This method adds the newly created drone to either the Express Service or Regular Service queue
        //  based on the service selection.
        //  It updates the status message to confirm the addition, clears the current item list in the UI,
        //  and then binds the updated queue to the appropriate list control
        //  (ExpressServiceList or RegularServiceList).
        private void AddToQueue (){
            if (ExpressSelected.IsChecked == true){
                RefreshQueue(true);
                SetStatusDetails("Successfully added new customer to the Express Service queue.", true);
            } else {
                RefreshQueue(false);
                SetStatusDetails("Successfully added new customer to the Regular Service queue.", true);
            }
        }

        //  This method resets the input fields to their default placeholder values
        //  ("Name", "Details", "Cost", "Problem") and unchecks both the Express and
        //  Regular service options. It prepares the form for the next input.
        private void ClearInputFields (){
            NameField.Text = "Name";
            DetailsField.Text = "Details";
            ServiceCostTextBox.Text = "Cost";
            ProblemField.Text = "Problem";
            ExpressSelected.IsChecked = false;
            RegularSelected.IsChecked = false;
        }

        //  This method checks whether either the Express or Regular service option is selected.
        //  If at least one is selected, it returns true, indicating a valid service priority;
        //  otherwise, it returns false.
        private bool GetServicePriority (){
            if (ExpressSelected.IsChecked == true || RegularSelected.IsChecked == true){
                return true;
            } else {
                return false;
            }
        }

        //  This function ensures that only valid currency-style input is allowed in a TextBox.
        //  It uses a regular expression to restrict user input to numbers with up to two decimal
        //  places (e.g., 123, 123.45). If the input doesn't match, the keystroke is blocked.
        private void ServiceCostTextBox_PreviewTextInput (object sender, TextCompositionEventArgs e){
            var regex = new Regex(@"^\d*(\.\d{0,2})?$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text + e.Text);
        }

        //  This function opens a DetailWindow showing information about the selected drone from a list.
        //  When the window is closed, the selection in the ListView is cleared, allowing the user to
        //  reselect the same item to view its details again.
        private void ExpressServiceList_SelectionChanged (object sender, SelectionChangedEventArgs e){
            if (ExpressServiceList.SelectedItem is Drone selectedItem){
                SetStatusDetails(selectedItem.DisplayDetails(), true);

                var detailWindow = new DetailWindow(
                    selectedItem.DisplayClientName,
                    selectedItem.DisplayDroneModel,
                    selectedItem.DisplayServiceProblem,
                    selectedItem.DisplayServiceCost.ToString());
                
                detailWindow.Owner = this;
                detailWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                detailWindow.Closed += (s, args) => {
                    ExpressServiceList.SelectedItem = null;
                };

                detailWindow.ShowDialog();
            }
        }

        //  This function opens a DetailWindow displaying details about the selected drone from the RegularServiceList.
        //  When the window is closed, the selection in the RegularServiceList is cleared, allowing the user to reselect
        //  the same item to view its details again.
        private void RegularServiceList_SelectionChanged (object sender, SelectionChangedEventArgs e){
            if (RegularServiceList.SelectedItem is Drone selectedItem){
                SetStatusDetails(selectedItem.DisplayDetails(), true);

                var detailWindow = new DetailWindow(
                    selectedItem.DisplayClientName,
                    selectedItem.DisplayDroneModel,
                    selectedItem.DisplayServiceProblem,
                    selectedItem.DisplayServiceCost.ToString());
                
                detailWindow.Owner = this;
                detailWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                detailWindow.Closed += (s, args) => {
                    RegularServiceList.SelectedItem = null;
                };

                detailWindow.ShowDialog();
            }
        }

        private void DequeueExpress_Click (object sender, RoutedEventArgs e){
            if (ExpressServiceList.Items.Count > 0){
                dsManager.DequeueDrone(true);
                RefreshQueue(true);
                DisplayFinishedList();
                SetStatusDetails("Successfully removed the Drone from the Express Service Queue.", true);
            } else {
                SetStatusDetails("Unable to remove the Drone from the Express Service Queue, no Drone was found", false);
            }
        }

        private void DequeueRegular_Click (object sender, RoutedEventArgs e){
            if (RegularServiceList.Items.Count > 0){
                dsManager.DequeueDrone(false);
                RefreshQueue(false);
                DisplayFinishedList();
                SetStatusDetails("Successfully removed the Drone from the Regular Service Queue.", true);
            } else {
                SetStatusDetails("Unable to remove the Drone from the Regular Service Queue, no Drone was found", false);
            }
        }

        private void RefreshQueue (bool priority){
            if (priority){
                ExpressServiceList.ItemsSource = null;
                ExpressServiceList.ItemsSource = dsManager.GetExpressQueue().ToList();
            } else {
                RegularServiceList.ItemsSource = null;
                RegularServiceList.ItemsSource = dsManager.GetRegularQueue().ToList();
            }
        }

        private void DisplayFinishedList (){
            FinishedWorkList.ItemsSource = null;
            FinishedWorkList.ItemsSource = dsManager.GetFinishedList();
        }

        private void CompletedServiceList_SelectionChanged (object sender, SelectionChangedEventArgs e){
            if (FinishedWorkList.SelectedItem is Drone selectedItem){
                var detailWindow = new DetailWindow(
                    selectedItem.DisplayClientName,
                    selectedItem.DisplayDroneModel,
                    selectedItem.DisplayServiceProblem,
                    selectedItem.DisplayServiceCost.ToString());
                
                detailWindow.Owner = this;
                detailWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                detailWindow.Closed += (s, args) => {
                    FinishedWorkList.SelectedItem = null;
                };

                detailWindow.ShowDialog();
            }
        }

        private void FinishedWorkList_MouseDoubleClick (object sender, MouseButtonEventArgs e){
            if (FinishedWorkList.SelectedItem is Drone selectedItem){
                dsManager.RemoveDroneFromFinishedList(selectedItem);
                finishedDrones.Remove(selectedItem);
                DisplayFinishedList();
                SetStatusDetails("Successfully Deleted the Drone from the Completed Work List.", true);
            }
        }
    }
}