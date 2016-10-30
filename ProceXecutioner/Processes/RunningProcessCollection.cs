using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ProceXecutioner.Processes {

    /// <summary>Represents the set of all currently running processes, at a specific instant in time.</summary>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{RunningProcess}" />
    public class RunningProcessCollection : ObservableCollection<RunningProcess> {

        /// <summary>Initializes a new instance of the <see cref="RunningProcessCollection"/> class.</summary>
        public RunningProcessCollection() : base() { }

        /// <summary>Initializes a new instance of the <see cref="RunningProcessCollection"/> class.</summary>
        /// <param name="runningProcesses">The running processes.</param>
        public RunningProcessCollection(IEnumerable<RunningProcess> runningProcesses) : base() {
            foreach (var rp in runningProcesses) Add(rp);
        }

        /// <summary>Gets the total instance count.</summary>
        public int TotalInstanceCount => this.Sum(rp => rp.InstanceCount);

        /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
        public override string ToString() => this.Select(rp => $"{{{rp}}}").ToDelimitedString(", ");

        /// <summary>Sets the armed status of contained processes based on the given target collection.</summary>
        /// <param name="collection">The target collection.</param>
        public void SetArmed(TargetProcessGroupCollection collection) {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            
            foreach (var rp in this) rp.Armed = null; //Reset all 'armed's to null.

            foreach (var target in collection.ArmedTargets) { //Loop through collection groups
                foreach (var p in this.Where(rp => rp.Name.ToUpper() == target.Name)) { //Loop through group targets
                    p.Armed = target.Armed; 
                }
            }
            base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
