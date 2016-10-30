using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProceXecutioner.Processes {

    /// <summary>Base class for objects that manipulate Windows processes for a specific Windows session.</summary>
   public abstract class ProcessUtilityBase {

        /// <summary>Initializes a new instance of the <see cref="ProcessUtilityBase"/> class. </summary>
        /// <param name="sessionId">ID of session to monitor. Defaults to current session.</param>
        protected ProcessUtilityBase(int? sessionId = null) {
            this.SessionId = sessionId ?? Process.GetCurrentProcess().SessionId;
        }

        /// <summary>Gets the session identifier. </summary>
        public int SessionId { get; private set; }

        /// <summary>Gets a sequence containing a Process object for each process in the monitored session. </summary>
        public IEnumerable<Process> SessionProcesses =>
            Process.GetProcesses().Where(p => p.SessionId == this.SessionId);

    }
}
