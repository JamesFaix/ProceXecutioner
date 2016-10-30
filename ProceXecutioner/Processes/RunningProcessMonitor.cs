using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProceXecutioner.Processes {

    /// <summary>Watches for process starts and stops in a specific session. </summary>
    /// <seealso cref="ProceXecutioner.Processes.ProcessUtilityBase" />
    public class RunningProcessMonitor : ProcessUtilityBase {

        /// <summary>Initializes a new instance of the <see cref="RunningProcessMonitor"/> class. </summary>
        /// <param name="sessionId">ID of session to monitor. Defaults to current session.</param>
        public RunningProcessMonitor(int? sessionId = null)
            : base(sessionId) {

            this.Processes = new RunningProcessCollection();
            this.refreshMilliseconds = 500;
            Task.Run((Action)CheckForProcessChanges);
        }

        /// <summary>Gets or sets the number of milliseconds between checking for process changes. </summary>
        /// <exception cref="ArgumentOutOfRangeException">RefreshMilliseconds must be greater than 0.</exception>
        public int RefreshMilliseconds {
            get { return refreshMilliseconds; }
            set {
                if (value <= 0) throw new ArgumentOutOfRangeException("RefreshMilliseconds must be greater than 0.");
                refreshMilliseconds = value;
            }
        }
        /// <summary>The number of milliseconds between checking for process changes. </summary>
        private int refreshMilliseconds;

        /// <summary>Gets a collection of objects representing the processes running in the monitored session. </summary>
        public RunningProcessCollection Processes { get; private set; }

        #region INotifyPropertyChanged

        /// <summary>Occurs when a property value changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>Called when [property changed]. </summary>
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        /// <summary>Checks for process changes, then sleeps the thread. </summary>
        private void CheckForProcessChanges() {
            while (true) {
                //Check current processes
                var currentProcesses = new RunningProcessCollection(
                    SessionProcesses
                    .GroupBy(p => p.ProcessName)
                    .Select(g => new RunningProcess(g.Key, g.Count()))
                    .OrderBy(rp => rp.Name)); //sorting allows sequence comparison

                //If different than last check, publish update
                if (!currentProcesses.SequenceEqual(this.Processes)) {
                    this.Processes = currentProcesses;
                    OnPropertyChanged(nameof(this.Processes));
                }

                //Wait to refresh
                Thread.Sleep(refreshMilliseconds);
            }
        }
    }
}
