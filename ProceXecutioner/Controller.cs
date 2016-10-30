using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using ProceXecutioner.Configuration;
using ProceXecutioner.Processes;

namespace ProceXecutioner {

    //BUG: Running processes not being colored sometimes

    /// <summary>Exposes data and commands to UI.</summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class Controller : IController {

        /// <summary>Initializes a new instance of the <see cref="Controller"/> class.</summary>
        public Controller(ISettings settings) {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            this.monitor = new RunningProcessMonitor();
            this.killer = new TargetProcessKiller();
            this.settings = settings;

            monitor.PropertyChanged += RunningProcesses_Changed;
        }

        #region Properties/Fields

        /// <summary>Encapsulates persisted settings.</summary>
        private readonly ISettings settings;

        /// <summary>Monitors running processes.</summary>
        private readonly RunningProcessMonitor monitor;

        /// <summary>Kills targeted processes.</summary>
        private readonly TargetProcessKiller killer;

        /// <summary>Synchronization lock for the collection of running processes.</summary>
        private object runningProcessesLock;

        /// <summary>Gets the target processes.</summary>
        public TargetProcessGroupCollection TargetProcesses { get; private set; }

        /// <summary>Gets the running processes.</summary>
        public RunningProcessCollection RunningProcesses { get; private set; }

        /// <summary>Gets the number of armed running processes. </summary>
        public int ArmedRunningProcessCount {
            get {
                var targetNames = TargetProcesses.DistinctArmedNames.ToArray();
                return killer.GetSessionProcessesWithName(targetNames).Count();
            }
        }

        #endregion

        #region Model event handlers

        /// <summary>Refreshes the running processes.</summary>
        private void RunningProcesses_Changed(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(RunningProcessMonitor.Processes)) {
                this.RunningProcesses = monitor.Processes;
                if (TargetProcesses != null)
                    this.RunningProcesses.SetArmed(TargetProcesses);

                OnPropertyChanged(nameof(RunningProcesses));
                OnPropertyChanged(nameof(ArmedRunningProcessCount));

                this.runningProcessesLock = new object();
                BindingOperations.EnableCollectionSynchronization(this.RunningProcesses, this.runningProcessesLock);
            }
        }

        /// <summary>Updates armed statuses when target processes are changed.</summary>
        private void TargetProcesses_Changed(object sender, NotifyCollectionChangedEventArgs e) {
            RunningProcesses.SetArmed(TargetProcesses);
            OnPropertyChanged(nameof(TargetProcesses));
            OnPropertyChanged(nameof(ArmedRunningProcessCount));
        }

        #endregion

        #region NotifyPropertyChanged

        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Called when [property changed].</summary>
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region User commands

        /// <summary>Saves the current targets.</summary>
        public void SaveTargets() {
            settings.Targets = this.TargetProcesses;
            settings.Save();
        }

        /// <summary>Loads the last saved targets.</summary>
        public void LoadTargets() {
            TargetProcesses = settings.Targets;
            TargetProcesses.CollectionChanged += TargetProcesses_Changed;
            RunningProcesses.SetArmed(TargetProcesses);
            OnPropertyChanged(nameof(TargetProcesses));
        }

        /// <summary>Kills the armed process groups.</summary>
        public void KillArmedTargets() => killer.Kill(TargetProcesses);

        #endregion
    }
}
