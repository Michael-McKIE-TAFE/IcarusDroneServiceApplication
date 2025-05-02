using IcarusDroneServiceBackend;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IcarusDroneServiceApplication {
    /// <summary>
    /// This class manages the UI and interactions for a drone service application. 
    /// It handles drone registration, manages drone service queues (express and regular), and 
    /// updates the status of service requests. It includes methods to add drones to service queues,
    /// dequeue completed drones, and display details of drone service records in a new window. 
    /// Additionally, it manages input validation, status messaging, and the updating of lists for 
    /// ongoing and completed services.
    /// </summary>
    public partial class MainWindow : Window {
        private DroneServiceManager dsManager;
        private ObservableCollection<Drone> finishedDrones;
        private DroneInputService inputService;

        //  Initializes the `MainWindow` by setting up the `DroneServiceManager` and `DroneInputService` instances,
        //  populating the `finishedDrones` collection with drones from the service manager's finished list, and binding
        //  the `FinishedWorkList` to this collection to display the completed drone services in the UI.
        public MainWindow (){
            InitializeComponent();
            dsManager = new DroneServiceManager();
            inputService = new DroneInputService(dsManager);    
            finishedDrones = new ObservableCollection<Drone>(dsManager.GetFinishedList());
            FinishedWorkList.ItemsSource = finishedDrones;
        }

        //  This method handles the `GotFocus` event for a `TextBox`, clearing its content when it gains focus.
        //  It checks if the sender is a `TextBox` and clears its text if true.
        private void TextBox_GotFocus (object sender, RoutedEventArgs e){
            TextBox? textBox = sender as TextBox;

            if (textBox != null){
                textBox.Clear();
            }
        }

        //  This method sets the status message and its corresponding text color based on a `status` flag.
        //  It displays the message with a green or red color, waits for 2 seconds using `Task.Delay`, and
        //  then resets the text to "Ready..." with a default color.
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

        //  This method is triggered when the "Add New Item" button is clicked. It checks if all required fields
        //  are filled and a service type is selected. If any of the conditions are not met, it displays an error
        //  message. If everything is valid, it creates a new record and clears the input fields.
        private void AddNewItem_Click (object sender, RoutedEventArgs e){
            bool serviceTypeSelected = GetServicePriority();

            if (NameField.Text == "Name" || DetailsField.Text == "Details" || ProblemField.Text == "Problem" || ServiceCostTextBox.Text == "Cost" || !serviceTypeSelected){
                SetStatusDetails("Error: Unable to add the new item. Please enter all of the fields.", false);
            } else {                
                CreateRecord();
                ClearInputFields();
            }
        }

        //  This method creates a new drone service record by extracting and trimming the values from the input fields.
        //  It checks if the registration is successful using the `TryRegisterDrone` method. If successful, it advances
        //  the service tag and adds the drone to the appropriate queue (express or regular). If there is an error, it
        //  displays an error message.
        private void CreateRecord (){
            string name = NameField.Text.Trim();
            string model = DetailsField.Text.Trim();
            string problem = ProblemField.Text.Trim();
            int tagNumber = (TagNumber.Value ?? 0);
            bool isExpress = ExpressSelected.IsChecked == true;
            double amount = inputService.GetCostInput(ServiceCostTextBox.Text, isExpress);

            if (amount == -1){
                SetStatusDetails("Please enter a valid service cost.", false);
            }

            if (inputService.TryRegisterDrone(name, model, problem, amount, tagNumber, isExpress, out string errorMsg)){
                AdvanceServiceTag(TagNumber);
                AddToQueue();
            } else {
                SetStatusDetails(errorMsg, false);
            }
        }

        //  This method increments the value of an `IntegerUpDown` control.
        //  It retrieves the current value, adds the increment, and checks if the new value exceeds the maximum.
        //  If it does, the value is reset to the minimum value (or 100 by default). The updated value is then
        //  assigned to the control.
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

        //  This method adds a new customer to either the Express or Regular service queue based
        //  on the user's selection. It then refreshes the corresponding queue and displays a
        //  success message indicating which queue the customer was added to.
        private void AddToQueue (){
            if (ExpressSelected.IsChecked == true){
                RefreshQueue(true);
                SetStatusDetails("Successfully added new customer to the Express Service queue.", true);
            } else {
                RefreshQueue(false);
                SetStatusDetails("Successfully added new customer to the Regular Service queue.", true);
            }
        }

        //  This method clears the input fields in the UI, resetting the text fields for name, details,
        //  cost, and problem to their placeholder values ("Name", "Details", "Cost", and "Problem").
        //  It also unchecks both the "Express" and "Regular" service type options.
        private void ClearInputFields (){
            NameField.Text = "Name";
            DetailsField.Text = "Details";
            ServiceCostTextBox.Text = "Cost";
            ProblemField.Text = "Problem";
            ExpressSelected.IsChecked = false;
            RegularSelected.IsChecked = false;
        }

        //  This method checks whether either the "Express" or "Regular" service type is selected.
        //  It returns `true` if one of the options is selected, indicating that a valid service
        //  priority has been chosen; otherwise, it returns `false`.
        private bool GetServicePriority (){
            if (ExpressSelected.IsChecked == true || RegularSelected.IsChecked == true){
                return true;
            } else {
                return false;
            }
        }

        //  This method restricts user input in the `ServiceCostTextBox` to only allow valid numeric
        //  values with up to two decimal places. It uses a regular expression to check that the input
        //  matches the pattern for a number, ensuring the input is properly formatted before it is processed.
        //  If the input doesn't match the pattern, the `e.Handled` flag is set to `true`, preventing the
        //  invalid input from being entered.
        private void ServiceCostTextBox_PreviewTextInput (object sender, TextCompositionEventArgs e){
            if (sender is TextBox textBox){
                var regex = new Regex(@"^\d*(\.\d{0,2})?$");
                e.Handled = !regex.IsMatch(textBox.Text + e.Text);
            }
        }

        //  This method handles the event when an item in the `ExpressServiceList` is selected.
        //  It retrieves the selected `Drone` item and displays its details in a `DetailWindow`.
        //  The details include the client's name, the drone model, the service problem, and the service cost.
        //  After displaying the details, the window is centered relative to the main window. When the `DetailWindow`
        //  is closed, the selected item in the `ExpressServiceList` is cleared.
        private void ExpressServiceList_SelectionChanged (object sender, SelectionChangedEventArgs e){
            if (ExpressServiceList.SelectedItem is Drone selectedItem){
                var detailWindow = new DetailWindow(
                    selectedItem.DisplayClientName,
                    selectedItem.DisplayDroneModel,
                    selectedItem.DisplayServiceProblem,
                    selectedItem.DisplayServiceCost.ToString(),
                    selectedItem.DisplayServiceTag.ToString());

                
                detailWindow.Owner = this;
                detailWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                detailWindow.Closed += (s, args) => {
                    ExpressServiceList.SelectedItem = null;
                };

                detailWindow.ShowDialog();
            }
        }

        //  This method handles the event when an item in the `RegularServiceList` is selected.
        //  It retrieves the selected `Drone` and displays its details in a `DetailWindow`,
        //  showing the client's name, the drone model, the service problem, and the service cost.
        //  The window is positioned in the center of the main window. After the `DetailWindow` is
        //  closed, the selected item in the `RegularServiceList` is cleared.
        private void RegularServiceList_SelectionChanged (object sender, SelectionChangedEventArgs e){
            if (RegularServiceList.SelectedItem is Drone selectedItem){
                var detailWindow = new DetailWindow(
                    selectedItem.DisplayClientName,
                    selectedItem.DisplayDroneModel,
                    selectedItem.DisplayServiceProblem,
                    selectedItem.DisplayServiceCost.ToString(),
                    selectedItem.DisplayServiceTag.ToString());
                
                detailWindow.Owner = this;
                detailWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                detailWindow.Closed += (s, args) => {
                    RegularServiceList.SelectedItem = null;
                };

                detailWindow.ShowDialog();
            }
        }

        //  This method handles the event when the "Dequeue Express" button is clicked.
        //  It checks if there are any items in the ExpressServiceList. If there are, it dequeues
        //  the first drone from the express service queue, refreshes the queue, displays the finished
        //  list, and updates the status message indicating the drone was successfully removed. If the
        //  queue is empty, it sets an error message stating no drones were found to remove.
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

        //  This method handles the event when the "Dequeue Regular" button is clicked.
        //  It checks if there are any items in the `RegularServiceList`. If there are,
        //  it dequeues the first drone from the regular service queue, refreshes the queue,
        //  updates the finished list, and displays a success message. If the queue is empty,
        //  it shows an error message indicating no drones were found to remove.
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

        //  This method refreshes the queue based on the `priority` parameter.
        //  If `priority` is `true`, it updates the `ExpressServiceList` by setting its
        //  `ItemsSource` to the current express service queue. If `priority` is `false`,
        //  it updates the `RegularServiceList` with the current regular service queue.
        //  It first clears the existing items to ensure the list is refreshed with the latest data.
        private void RefreshQueue (bool priority){
            if (priority){
                ExpressServiceList.ItemsSource = null;
                ExpressServiceList.ItemsSource = dsManager.GetExpressQueue().ToList();
            } else {
                RegularServiceList.ItemsSource = null;
                RegularServiceList.ItemsSource = dsManager.GetRegularQueue().ToList();
            }
        }

        //  This method updates the `FinishedWorkList` by first clearing its current items
        //  (`ItemsSource = null`) and then setting its `ItemsSource` to the current list of
        //  finished drones from `dsManager.GetFinishedList()`, ensuring that the list displays
        //  the latest completed drones.
        private void DisplayFinishedList (){
            FinishedWorkList.ItemsSource = null;
            FinishedWorkList.ItemsSource = dsManager.GetFinishedList();
        }

        //  This method handles the mouse double-click event on the FinishedWorkList.
        private void FinishedWorkList_MouseDoubleClick (object sender, MouseButtonEventArgs e){
            RemoveFromList();
        }

        //  This method handles the button pressed event for the Finalise Button
        private void FinaliseButton_Click (object sender, RoutedEventArgs e){
            RemoveFromList();
        }

        //  When a drone is double-clicked or the Finalise button is pressed, it removes
        //  the selected drone from both the finishedDrones collection and the DroneServiceManager's
        //  finished list. Afterward, it updates the display of the finished list and shows a success
        //  message indicating that the drone was successfully deleted from the completed work list.
        private void RemoveFromList (){
            if (FinishedWorkList.SelectedItem is Drone selectedItem){
                dsManager.RemoveDroneFromFinishedList(selectedItem);
                finishedDrones.Remove(selectedItem);
                DisplayFinishedList();
                SetStatusDetails("Successfully Deleted the Drone from the Completed Work List.", true);
            }
        }
    }
}