using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ProceXecutioner.Processes {

    /// <summary>Represents the set of all running processes whose name is on a specific list of names, 
    /// in the context of targeting processes to be killed. </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class TargetProcessGroup : INotifyPropertyChanged {

        /// <summary>Initializes a new instance of the <see cref="TargetProcessGroup"/> class. </summary>
        public TargetProcessGroup() {
            name = "";
            ProcessNames = new List<string>();
        }

        /// <summary>Gets or sets the group name. </summary>
        /// <exception cref="System.ArgumentNullException"></exception>
        public string Name {
            get { return name; }
            set {
                if (value == null) throw new ArgumentNullException(nameof(value));
                this.name = value;
                OnPropertyChagned(nameof(Name));
            }
        }
        /// <summary>The group name. </summary>
        private string name;

        /// <summary>Gets the list of process names. </summary>
        public List<string> ProcessNames { get; }

        /// <summary>Gets or sets the list of process names in this group, as a comma-separated list. </summary>
        public string ProcessNameList {
            get { return ProcessNames.ToDelimitedString(", "); }
            set {
                var names = value.Split(",")
                    .Select(word => word.Replace(" ", ""));

                ProcessNames.Clear();
                ProcessNames.AddRange(names);
                OnPropertyChagned(nameof(ProcessNameList));
            }
        }

        /// <summary>Gets or sets a value indicating whether the processes represented by this group are killable. </summary>
        public bool Armed {
            get { return armed; }
            set {
                armed = value;
                OnPropertyChagned(nameof(Armed));
            }
        }
        /// <summary>Value indicating whether the processes represented by this group are killable. </summary>
        private bool armed;

        /// <summary>Returns a <see cref="string" /> that represents this instance. </summary>
        public override string ToString() => $"Name: {Name}, Armed: {Armed}, Processes: {ProcessNameList}";

        #region NotifyPropertyChanged

        /// <summary>Occurs when a property value changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Called when [property chagned]. </summary>
        private void OnPropertyChagned(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
       
        #endregion
    }
}
