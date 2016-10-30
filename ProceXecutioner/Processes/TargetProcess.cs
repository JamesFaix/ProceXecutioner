using System;

namespace ProceXecutioner.Processes {

    /// <summary>Represents the set of all running processes with a specific name, 
    /// in the context of targeting processes to be killed.</summary>
    public class TargetProcess {

        /// <summary>Initializes a new instance of the <see cref="TargetProcess"/> class. </summary>
        /// <param name="name">The process name.</param>
        /// <param name="armed">Determines whether the represented processes should be killable.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public TargetProcess(string name, bool armed) {
            if (name == null) throw new ArgumentNullException(nameof(name));
            this.Name = name;
            this.Armed = armed;
        }

        /// <summary>Gets the process name. </summary>
        public string Name { get; }

        /// <summary>Gets a value indicating whether the represented processes should be killable. </summary>
        public bool Armed { get; }

        /// <summary>Returns a <see cref="string" /> that represents this instance. </summary>
        public override string ToString() => $"Name: {Name}, Armed:{Armed}";
    }
}
