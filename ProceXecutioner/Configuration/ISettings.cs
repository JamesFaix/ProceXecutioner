using ProceXecutioner.Processes;

namespace ProceXecutioner.Configuration {

    /// <summary>Encapsulates persistent application settings.</summary>
    public interface ISettings {

        /// <summary>Collection of target processes.</summary>
        TargetProcessGroupCollection Targets { get; set; }

        /// <summary>Saves the current settings.</summary>
        void Save();        
    }
}
