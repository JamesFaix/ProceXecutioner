using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace ProceXecutioner.Processes {

    /// <summary>Kills targeted processes in a specific session. </summary>
    /// <seealso cref="ProcessUtilityBase" />
    public class TargetProcessKiller : ProcessUtilityBase {

        /// <summary> Initializes a new instance of the <see cref="TargetProcessKiller"/> class. </summary>
        /// <param name="sessionId">ID of session to monitor. Defaults to current session.</param>
        public TargetProcessKiller(int? sessionId = null)
            : base(sessionId) {
        }

        /// <summary>Kills the specified groups. </summary>
        /// <param name="groups">The groups.</param>
        /// <exception cref="ArgumentNullException">groups cannot be null.</exception>
        public void Kill(TargetProcessGroupCollection groups) {
            if (groups == null) throw new ArgumentNullException(nameof(groups));

            var names = groups.DistinctArmedNames.ToArray();
            var processes = GetSessionProcessesWithName(names);
            foreach (var p in processes) p.Kill();
        }

        /// <summary>Gets a sequence containing a Process object for each process with 
        /// the one of the given names in the monitored session. </summary>
        /// <param name="names">The names.</param>
        /// <exception cref="ArgumentNullException">names cannot be null.</exception>
        public IEnumerable<Process> GetSessionProcessesWithName(IEnumerable<string> names) {
            if (names == null) throw new ArgumentNullException(nameof(names));

            return SessionProcesses.Where(p => names.Contains(p.ProcessName.ToUpper()));
        }
    }
}
