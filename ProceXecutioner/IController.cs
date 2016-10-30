using System.ComponentModel;
using ProceXecutioner.Processes;

namespace ProceXecutioner {

    /// <summary>Exposes data and commands to UI.</summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public interface IController : INotifyPropertyChanged {

        /// <summary>Gets the target processes.</summary>
        TargetProcessGroupCollection TargetProcesses { get; }

        /// <summary>Gets the running processes.</summary>
        RunningProcessCollection RunningProcesses { get; }

        /// <summary>Gets the number of armed running processes. </summary>
        int ArmedRunningProcessCount { get; }

        /// <summary>Saves current targets.</summary>
        void SaveTargets();

        /// <summary>Loads the last saved targets.</summary>
        void LoadTargets();

        /// <summary>Kills the armed process groups.</summary>
        void KillArmedTargets();
    }
}
