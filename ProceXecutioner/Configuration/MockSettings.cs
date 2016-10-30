using System;
using ProceXecutioner.Processes;

namespace ProceXecutioner.Configuration {

    /// <summary>
    /// Mock class for settings that does not persist changes between application instances.
    /// </summary>
    /// <seealso cref="ISettings" />
    class MockSettings : ISettings {

        /// <summary>Initializes a new instance of the <see cref="MockSettings"/> class.</summary>
        public MockSettings() {
            targets = DefaultTargets;
        }

        /// <summary>Collection of target processes.</summary>
        /// <exception cref="ArgumentNullException"></exception>
        public TargetProcessGroupCollection Targets {
            get { return targets; }
            set {
                if (value == null) throw new ArgumentNullException(nameof(value));
                targets = value;
            }
        }
        private TargetProcessGroupCollection targets;

        /// <summary>(Does nothing)</summary>
        public void Save() { }

        /// <summary>Gets the default targets.</summary>
        private static TargetProcessGroupCollection DefaultTargets {
            get {
                return new TargetProcessGroupCollection(new[] {
                    new TargetProcessGroup {
                        Name = "Misc",
                        Armed = false,
                        ProcessNames = { "Notepad", "Paint" }
                    },
                    new TargetProcessGroup {
                        Name = "Office",
                        Armed = false,
                        ProcessNames = { "Excel", "Outlook" }
                    },
                    new TargetProcessGroup {
                        Name = "Visual Studio",
                        Armed = false,
                        ProcessNames = { "devenv", "vshub", "vbcscompiler" }
                    }
                });
            }
        }
    }
}
