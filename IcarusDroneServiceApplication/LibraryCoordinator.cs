using IcarusDroneServiceBackend;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace IcarusDroneServiceApplication {
    internal class LibraryCoordinator {
        private readonly ListView _regularQueue;
        private readonly ListView _expressQueue;
        private readonly ListBox _finishedWorkBox;
        private readonly TextBlock _statusMessages;
        private readonly IntegerUpDown _numericControl;
        private readonly DisplayCoordinator library;
        private ObservableCollection<ViewModel> ExpressQueueItems;
        private ObservableCollection<ViewModel> RegularQueueItems;
        private ObservableCollection<ViewModel> FinishedWorkItems;

        public LibraryCoordinator (ListView listView, ListView listView2, ListBox listBox, TextBlock textBlock, IntegerUpDown upDown){
            _regularQueue = listView;
            _expressQueue = listView2;
            _finishedWorkBox = listBox;
            _statusMessages = textBlock;
            _numericControl = upDown;
            library = new DisplayCoordinator ();
            ExpressQueueItems = [];
            RegularQueueItems = [];
            FinishedWorkItems = [];
        }

        //  Programming Requirements: 6.5
        //  This method is used to tell the Class library to create a new Drone. If the Drone was created successfully it will then
        //  call the methods that need to run once the Drone has been created, otherwise it will display a message about the process
        //  having failed.
        public bool CreateRecord (string name, string model, string problem, int tagNumber, bool isExpress, string amount){
            var success = library.TryRegisterDrone(name, model, problem, amount, tagNumber, isExpress);

            if (success){
                AdvanceServiceTag(_numericControl);
                ConfrimItemCreation(isExpress);
                return true;
            } else {
                SetStatusDetails("Unable to create a new record, something went wrong.", false);
                return false;
            }
        }

        //  Programming Requirements: 6.8
        //  This method is used to refresh the content of the Regular Service ListView.
        //  Each aspect is bound to the corrisponding aspect of the Tuple it returns from
        //  the backend.
        private void RefreshRegularQueue (){
            _regularQueue.ItemsSource = null;
            var items = library.ReturnRegularQueue();

            RegularQueueItems = new ObservableCollection<ViewModel> (
                items.Select(d => new ViewModel {
                    DisplayClientName = d.Item1,
                    DisplayDroneModel = d.Item2,
                    DisplayServiceProblem = d.Item3,
                    DisplayServiceCost = d.Item4,
                    DisplayServiceTag = d.Item5
                })
            );

            _regularQueue.ItemsSource = RegularQueueItems;
        }

        //  Programming Requirements: 6.9
        //  This method is used to refresh the content of the Express Service ListView.
        //  Each aspect is bound to the corrisponding aspect of the Tuple it returns from
        //  the backend.
        private void RefreshExpressQueue (){
            _expressQueue.ItemsSource = null;

            var items = library.ReturnExpressQueue();

            ExpressQueueItems= new ObservableCollection<ViewModel> (
                items.Select(d => new ViewModel {
                    DisplayClientName = d.Item1,
                    DisplayDroneModel = d.Item2,
                    DisplayServiceProblem = d.Item3,
                    DisplayServiceCost = d.Item4,
                    DisplayServiceTag = d.Item5
                })
            );

            _expressQueue.ItemsSource= ExpressQueueItems;
        }

        //  Programming Requirements: 6.11
        //  Made static to fix IDE warning CA1822: Member doesn't access instance data and can be marked static.
        //  This method advances the value of the numeric control for the service tag by its incrment amount
        //  found in MainWindow.xaml. Once the controls value becomes greater than its defined maximum value
        //  it resets the value back to the lowest value.
        private static void AdvanceServiceTag (IntegerUpDown control){
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

        //  Programming Reguirements: 6.12 & 6.13
        //  This method creates a Window to display the details of the drone that the user
        //  has selected from either the express or regular service ListView.
        public void DisplayItemDetails (bool priority){
            ListView listView = new();

            if (priority){
                listView = _expressQueue;
            } else {
                listView = _regularQueue;
            }

            if (listView.SelectedItem is ViewModel selectedItem){
                var detailWindow = new DetailWindow(
                    selectedItem.DisplayClientName ?? "",
                    selectedItem.DisplayDroneModel ?? "",
                    selectedItem.DisplayServiceProblem ?? "",
                    selectedItem.DisplayServiceCost.ToString() ?? "",
                    selectedItem.DisplayServiceTag.ToString() ?? ""
                );

                detailWindow.Owner = Application.Current.MainWindow;
                detailWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                detailWindow.Closed += (s, args) => { listView.SelectedItem = null; };

                detailWindow.ShowDialog();
            }
        }

        //  Programming Requirements: 6.14 & 6.15
        //  This method tells the class library to dequeue a drone based on its priority,
        //  then refreshes both of the ListViews to reflect the change.
        public void DeQueueItem (bool priority){
            bool success = library.DequeueDrone(priority);

            if (success){
                SetStatusDetails("Successfully removed a drone from the queue.", true);
                RefreshExpressQueue();
                RefreshRegularQueue();
                DisplayFinishedList();
            } else {
                SetStatusDetails("Failed to remove a drone from the queue.", false);
            }
        }

        //  This method is used for formatting the messages that are outputted on the
        //  status bar.
        public async void SetStatusDetails (string message, bool status){
            if (status){
                _statusMessages.Foreground = (Brush)Application.Current.Resources["ForestGreen"];
            } else {
                _statusMessages.Foreground = (Brush)Application.Current.Resources["Crimson"];
            }

            _statusMessages.Text = message;
            await Task.Delay(3500);
            _statusMessages.Foreground = (Brush)Application.Current.Resources["Platinum"];
            _statusMessages.Text = "Ready...";
        }        

        //  This method is used to update and display the finished work list.
        //  It does this by returning the list of finished work from the backend,
        //  then binds the data to the Listbox on the front end.
        private void DisplayFinishedList (){
            _finishedWorkBox.ItemsSource = null;
            var drones = library.ReturnFinishedList();

            FinishedWorkItems = new ObservableCollection<ViewModel> (
                drones.Select(d => new ViewModel {
                DisplayClientName = d.Item1,
                DisplayDroneModel = d.Item2,
                DisplayServiceProblem = d.Item3,
                DisplayServiceCost = d.Item4,
                DisplayServiceTag = d.Item5
            }));

            _finishedWorkBox.ItemsSource = FinishedWorkItems;
        }

        //  This method is used to tell the DisplayCoordinator to delete a drone from
        //  the finished work list.
        public void RemoveItemFromList (){
            if (_finishedWorkBox.SelectedItem is ViewModel selectedItem){
                var tag = selectedItem.DisplayServiceTag;

                if (tag.HasValue) {
                    library.RemoveDroneFromFinishedList(tag.Value);
                    FinishedWorkItems.Remove(selectedItem);
                    SetStatusDetails("Successfully Deleted the Drone from the Completed Work List.", true);
                    DisplayFinishedList();
                } else {
                    SetStatusDetails("Error: Tag is null, cannot delete the drone.", false);
                }
            }
        }

        //  This method is used to confrim that a new Item was successfully created and updates the
        //  corresponding ListView with the new Drone.
        private void ConfrimItemCreation (bool priority){
            if (priority){
                RefreshExpressQueue();
                SetStatusDetails("Successfully added new customer to the Express Service queue.", true);
            } else {
                RefreshRegularQueue();
                SetStatusDetails("Successfully added new customer to the Regular Service queue.", true);
            }
        }        
    }
}