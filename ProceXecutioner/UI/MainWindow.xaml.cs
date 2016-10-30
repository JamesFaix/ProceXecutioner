using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace ProceXecutioner.UI {
    //BUG: Target processes not turning correct color after checkbox clicked

    /// <summary>Interaction logic for MainWindow.xaml </summary>
    partial class MainWindow : Window {

        /// <summary>Initializes a new instance of the <see cref="MainWindow"/> class. </summary>
        public MainWindow(IController controller) {
            InitializeComponent();

            this.notifyIcon = new NotifyIconController(this) {
                Text = "ProceXecutioner",
                Icon = Properties.Resources.Axe
            };

            this.controller = controller;
            this.controller.PropertyChanged += this.Model_Changed;
            this.controller.LoadTargets();
            RefreshView();
        }

        /// <summary>The notify icon control.</summary>
        private readonly NotifyIconController notifyIcon;
        
        /// <summary>Gets the view model. </summary>
        private IController controller { get; }

        /// <summary>Refreshes the model when the model changes. 
        /// (Must use Dispatcher to update model on UI thread.)</summary>
        private void Model_Changed(object sender, PropertyChangedEventArgs e) => Dispatcher.Invoke(RefreshView);

        /// <summary>Updates the view based on the view model. </summary>
        private void RefreshView() {
            TargetProcessesGrid.DataContext = controller.TargetProcesses;
            RunningProcessesGrid.DataContext = controller.RunningProcesses;
            RunningProcessCountLabel.DataContext = controller.RunningProcesses;
            TargetProcessCountLabel.DataContext = controller;
            //   this.TargetProcessesGrid.Items.Refresh();
            //   this.RunningProcessesGrid.Items.Refresh();
        }

        /// <summary>Asks the user for approval to kill armed target processes. </summary>
        /// <returns> <c>true</c>, if user approves; otherwise <c>false</c>.</returns>
        private bool GetKillApproval() {
            var count = controller.ArmedRunningProcessCount;

            if (count == 0) {
                Alerts.Notify(
                    caption: "Kill processes",
                    message: "No processes to kill.");
                return false;
            }
            else {
                return Alerts.GetApproval(
                    caption: "Kill processes",
                    message: $"Are you sure want to kill {count} processes?");
            }
        }

        #region Button event handlers

        /// <summary>Saves the current targets when Save is clicked. </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e) => controller.SaveTargets();

        /// <summary>Loads saved targets when Revert is clicked. </summary>
        private void RevertButton_Click(object sender, RoutedEventArgs e) => controller.LoadTargets();

        /// <summary>Checks for user approval, then kills target processes when Kill is clicked. </summary>
        private void KillButton_Click(object sender, RoutedEventArgs e) {
            if (GetKillApproval())
                controller.KillArmedTargets();
        }

        #endregion

        #region Target Processes grid event handlers

        /// <summary>Starts editing a cell when it gets focus. </summary>
        private void TargetProcessesGrid_GotFocus(object sender, RoutedEventArgs e) {
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
                ((DataGrid)sender).BeginEdit(e);
        }

        /// <summary>Refreshes the model when the user types in the cell. </summary>
        private void TargetProcessesGrid_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) => RefreshView();

        /// <summary>Stores the last edited cell when editing is about to end. </summary>
        private void TargetProcessesGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            targetProcessRowBeingEdited = e.Row.Item as DataRowView;
        }

        /// <summary>Ends editing and refreshes the model when a cell is entered or exited. </summary>
        private void TargetProcessesGrid_CurrentCellChanged(object sender, EventArgs e) {
            if (targetProcessRowBeingEdited != null) {
                targetProcessRowBeingEdited.EndEdit();
                RefreshView();
            }
        }

        /// <summary>The grid row being edited </summary>
        private DataRowView targetProcessRowBeingEdited = null;

        #endregion
    }
}
