using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IcarusDroneServiceApplication {
    //  Programming Requirements: 6.18
    public partial class MainWindow : Window {
        private readonly LibraryCoordinator coordinator;

        public MainWindow (){
            InitializeComponent();
            coordinator = new LibraryCoordinator(RegularServiceList, ExpressServiceList, FinishedWorkList, StatusDetailsText, TagNumber);
        }

        //  Programming Requirements: 6.5
        //  This method is used to determine what happens when the user presses the
        //  Submit button when adding a new user.
        private void AddNewItem_Click (object sender, RoutedEventArgs e){ 
            bool serviceTypeSelected = GetServicePriority();
            bool isExpress = ExpressSelected.IsChecked == true;
            int tagNumber = (TagNumber.Value ?? 0);

            if (string.IsNullOrWhiteSpace(NameField.Text) || string.IsNullOrWhiteSpace(DetailsField.Text) || string.IsNullOrWhiteSpace(ProblemField.Text) || string.IsNullOrWhiteSpace(ServiceCostTextBox.Text) || !serviceTypeSelected){
                coordinator.SetStatusDetails("Error: Unable to add the new item. Please enter all of the fields.", false);
            } else {
                bool success = coordinator.CreateRecord(NameField.Text.Trim(), DetailsField.Text.Trim(), ProblemField.Text.Trim(), tagNumber, isExpress, ServiceCostTextBox.Text.Trim());
                
                if (success){
                    ClearInputFields();
                } else {
                    coordinator.SetStatusDetails("Unable to create record, something went wrong.", false);
                }
            }
        }

        //  Programming Requirements: 6.7
        //  This method checks that the user has selected either the regular or express radio button,
        //  And returns true if either of them are, otherwise returns false.
        private bool GetServicePriority (){
            if (ExpressSelected.IsChecked == true || RegularSelected.IsChecked == true){
                return true;
            }

            return false;
        }

        //  Programming Requirements: 6.10
        //  This method restricts the input of a TextBox to only allow for numbers to be entered.
        private void ServiceCostTextBox_PreviewTextInput (object sender, TextCompositionEventArgs e){
            if (sender is TextBox textBox){
                var regex = new Regex(@"^\d*(\.\d{0,2})?$");
                e.Handled = !regex.IsMatch(textBox.Text + e.Text);
            }
        }

        //  Programming Requirements: 6.12
        //  This method tells the LibraryCoordinator to display the details for the selected regular item.
        private void RegularServiceList_SelectionChanged (object sender, SelectionChangedEventArgs e){
            coordinator.DisplayItemDetails(false);
        }

        //  Programming Requirements: 6.13
        //  This method tells the LibraryCoordinator to display the details for the selected express item.
        private void ExpressServiceList_SelectionChanged (object sender, SelectionChangedEventArgs e){
            coordinator.DisplayItemDetails(true);
        }

        //  Programming Requirements: 6.14
        //  This method tells the LibraryCoordingator.cs to dequeue an item from teh regular queue.
        private void DequeueRegular_Click (object sender, RoutedEventArgs e){ 
            coordinator.DeQueueItem(false);
        }

        //  Programming Requirements: 6.15
        //  This method tells LibraryCoordinator.cs to dequeue an item from the express queue.
        private void DequeueExpress_Click (object sender, RoutedEventArgs e){
            coordinator.DeQueueItem(true);
        }

        //  Programming Requirements: 6.16
        //  This method tells the LibraryCoordinator to remove an Item from the finished work
        //  list when the user double clicks on an Item in the list.
        private void FinishedWorkList_MouseDoubleClick (object sender, MouseButtonEventArgs e){
            coordinator.RemoveItemFromList();
        }

        //  Programming Requirements: 6.17
        //  This method clears the input fields once a record has been added.
        private void ClearInputFields (){
            NameField.Clear();
            DetailsField.Clear();
            ProblemField.Clear();
            ServiceCostTextBox.Clear();
            ExpressSelected.IsChecked = false;
            RegularSelected.IsChecked = false;
        }

        //  This method tells the LibraryCoordinator to remove an Item from the finished work
        //  list when the user presses the finalise button.
        private void FinaliseButton_Click (object sender, RoutedEventArgs e){
            coordinator.RemoveItemFromList();
        }

        //  This method allows the Window to be moved around the screen.
        private void Window_MouseDown (object sender, MouseButtonEventArgs e){
            if (e.LeftButton == MouseButtonState.Pressed){
                DragMove();
            }
        }

        //  This method minimises the Window when pressed.
        private void BtnMinimise_Click (object sender, RoutedEventArgs e){
            WindowState = WindowState.Minimized;
        }

        //  This method Closes the Application when pressed.
        private void BtnClose_Click (object sender, RoutedEventArgs e){
            Application.Current.Shutdown();
        }

        //  This method clears the contents of a TextBox when the user clicks into it.
        private void TextBox_GotFocus (object sender, RoutedEventArgs e){
            if (sender is TextBox textBox){
                textBox.Clear();
            }
        }
    }
}