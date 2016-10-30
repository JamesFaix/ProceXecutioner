using System;
using System.Diagnostics;
using ProceXecutioner.Processes;

namespace ProceXecutioner.Configuration {

    /// <summary>Encapsulates application settings persisted through a *.settings file.</summary>
    class SystemSettings : ISettings {

        /// <summary>Initializes a new instance of the <see cref="SystemSettings" /> class.</summary>
        internal SystemSettings() {
            this.inner = Settings.Default;
            this.serializer = new TargetSerializer();
        }

        /// <summary>Serializes and deserializes targets.</summary>
        private readonly TargetSerializer serializer;

        /// <summary>*.settings file object.</summary>
        private readonly Settings inner;

        /// <summary>Gets or sets the targets.</summary>
        public TargetProcessGroupCollection Targets {
            get {
                var xml = (string)inner[TARGETS_SETTING_NAME];
                try {
                    return serializer.CollectionFromXml(xml);
                }
                catch (Exception e) {
                    Trace.WriteLine($"{e.GetType()}: {e.Message} @ {e.StackTrace}");
                    return DefaultTargets;
                }
            }
            set {
                if (value == null) throw new ArgumentNullException(nameof(value));
                inner[TARGETS_SETTING_NAME] = serializer.CollectionToXml(value);
            }
        }

        /// <summary>Name of the setting containing target processes.</summary>
        private const string TARGETS_SETTING_NAME = "TargetProcessesXml";

        /// <summary>Default target processes.</summary>
        private TargetProcessGroupCollection DefaultTargets {
            get {
                return new TargetProcessGroupCollection(
                    new[] {
                        new TargetProcessGroup {
                            Name = "Office",
                            Armed = false,
                            ProcessNameList = "Excel, WinWord, Outlook"
                        },
                        new TargetProcessGroup {
                            Name = "Browsers",
                            Armed = false,
                            ProcessNameList = "Firefox, Chrome, IExplorer"
                        },
                        new TargetProcessGroup {
                            Name = "Visual Studio",
                            Armed = false,
                            ProcessNameList = "devenv, msvsmon, scriptedsandbox64"
                        }
                    });
            }
        }

        /// <summary>Saves the current settings.</summary>
        public void Save() {
            inner.Save();
        }
    }
}
