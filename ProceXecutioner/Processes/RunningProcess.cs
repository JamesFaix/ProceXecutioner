using System;
using System.ComponentModel;

namespace ProceXecutioner.Processes {

    /// <summary>Represents the set of all currently running processes with a specific name at a specific instant in time. </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    /// <seealso cref="System.IEquatable{RunningProcess}" />
    public class RunningProcess : INotifyPropertyChanged, IEquatable<RunningProcess> {

        /// <summary>Initializes a new instance of the <see cref="RunningProcess"/> class. </summary>
        /// <param name="name">The process name.</param>
        /// <param name="instances">The number of instances.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RunningProcess(string name, int instances) {
            if (name == null) throw new ArgumentNullException(nameof(name));
            this.Name = name;
            this.InstanceCount = instances;
        }

        /// <summary>Gets the process name.</summary>
        public string Name { get; }

        /// <summary>Gets the instance count. </summary>
        public int InstanceCount { get; }

        /// <summary>Gets or sets a value indicating whether this process is armed (true), 
        /// targeted but not armed (false) or not targeted (null).</summary>
        public bool? Armed {
            get { return armed; }
            set {
                armed = value;
                OnPropertyChanged(nameof(Armed));
            }
        }
        /// <summary>Value indicating armed status of this process.</summary>
        private bool? armed;

        /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
        public override string ToString() => $"Name: {Name}, Count: {InstanceCount}, Armed: {Armed}";

        #region NotifyPropertyChanged

        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Called when [property changed].</summary>
        /// <param name="propertyName">Name of the property.</param>
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region IEquatable

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        public bool Equals(RunningProcess other) {
            return other != null
                && Name == other.Name
                && InstanceCount == other.InstanceCount;
        }

        /// <summary>Determines whether the specified <see cref="System.Object" />, is equal to this instance.</summary>
        public override bool Equals(object obj) => Equals(obj as RunningProcess);

        /// <summary>Returns a hash code for this instance.</summary>
        public override int GetHashCode() => Name.GetHashCode();

        /// <summary>Implements the operator ==.</summary>
        public static bool operator ==(RunningProcess a, RunningProcess b) =>
            (Equals(a, null) && Equals(b, null))
            || (!Equals(a, null) && a.Equals(b));

        /// <summary>Implements the operator !=.</summary>
        public static bool operator !=(RunningProcess a, RunningProcess b) => !(a == b);

        #endregion
    }
}
