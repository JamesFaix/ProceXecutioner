using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ProceXecutioner.Processes {

    /// <summary>Represents a collection of all targeted processes. </summary>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{TargetProcessGroup}" />
    public class TargetProcessGroupCollection : ObservableCollection<TargetProcessGroup> {

        /// <summary>Initializes a new instance of the <see cref="TargetProcessGroupCollection"/> class. </summary>
        /// <param name="targetProcessGroups">Collection of TargetProcessGroups.</param>
        public TargetProcessGroupCollection(IEnumerable<TargetProcessGroup> targetProcessGroups = null)
            : base() {

            if (targetProcessGroups != null) {
                foreach (var group in targetProcessGroups) {
                    if (group == null) throw new ArgumentNullException("Collection cannot contain null.");
                    Add(group);
                }
            }
        }

        /// <summary>Returns a <see cref="string" /> that represents this instance. </summary>
        public override string ToString() => this.Select(tpg => $"{{{tpg}}}").ToDelimitedString(", ");

        /// <summary>Gets a sequence of distinct names found in target groups. </summary>
        public IEnumerable<string> DistinctArmedNames =>
                    this.Where(group => group.Armed)
                        .SelectMany(group => group.ProcessNames.Select(name => name.ToUpper()))
                        .Distinct();

        /// <summary>Gets a sequence of all armed targets. </summary>
        public IEnumerable<TargetProcess> ArmedTargets =>
                    this.SelectMany(tpg => tpg.ProcessNames
                        .Select(name => new TargetProcess(name.ToUpper(), tpg.Armed)))                //Get all targets
                        .GroupBy(tp => tp.Name)            //For any duplicate names, use Armed if any are
                        .Select(group => new TargetProcess(group.Key, group.Any(tp => tp.Armed)));

        /// <summary>Adds the specified target group to the collection.</summary>
        /// <param name="group">The group.</param>
        public new void Add(TargetProcessGroup group) {
            base.Add(group);
            group.PropertyChanged += ForwardEvent;
        }

        /// <summary>Removes the specified target group from the collection.</summary>
        /// <param name="group">The group.</param>
        public new void Remove(TargetProcessGroup group) {
            base.Remove(group);
            group.PropertyChanged -= ForwardEvent;
        }

        /// <summary>Forwards a property changed event to the base class.</summary>
        private void ForwardEvent(object sender, PropertyChangedEventArgs e) =>
            base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}
