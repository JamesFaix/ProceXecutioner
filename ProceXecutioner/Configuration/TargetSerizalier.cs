using System;
using System.Linq;
using System.Xml.Linq;
using ProceXecutioner.Processes;

namespace ProceXecutioner.Configuration {

    /// <summary>Serializes and deserializes target processes to XML.</summary>
    class TargetSerializer {

        #region XML tag constants

        /// <summary>XML tag for TargetProcessGroupCollection object.</summary>
        private const string COLLECTION_TAG = "ProcessGroupCollection";

        /// <summary>XML tag for TargetProcessGroup object.</summary>
        private const string GROUP_TAG = "ProcessGroup";

        /// <summary>XML tag for collection of TargetProcesses in a group.</summary>
        private const string PROCESSES_TAG = "Processes";

        /// <summary>XML tag for TargetProcess object.</summary>
        private const string PROCESS_TAG = "Process";

        /// <summary>XML tag for a Name property.</summary>
        private const string NAME_TAG = "Name";

        /// <summary>XML tag for an Armed property.</summary>
        private const string ARMED_TAG = "Armed";

        #endregion

        #region TargetProcessGroupCollection

        /// <summary>Serializes a TargetProcessGroupCollection to XML. </summary>
        /// <param name="collection">The collection.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public string CollectionToXml(TargetProcessGroupCollection collection) {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return new XElement(nameof(TargetProcessGroupCollection),
                collection.Select(group => GroupToXml(group))).ToString();
        }

        /// <summary>Deserializes a TargetProcessGroupCollection from XML. </summary>
        /// <param name="xml">The XML.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public TargetProcessGroupCollection CollectionFromXml(string xml) {
            if (xml == null) throw new ArgumentNullException(nameof(xml));
            var el = XElement.Parse(xml);
            var targets = el.Elements().Select(grp => GroupFromXml(grp));
            return new TargetProcessGroupCollection(targets);
        }

        #endregion

        #region TargetProcessGroup

        /// <summary>Serializes a TargetProcessGroup to XML. </summary>
        /// <param name="group">The group.</param>
        /// <exception cref="ArgumentNullException"></exception>
        private XElement GroupToXml(TargetProcessGroup group) {
            if (group == null) throw new ArgumentNullException(nameof(group));

            return new XElement(GROUP_TAG,
                new XElement(NAME_TAG, group.Name),
                new XElement(ARMED_TAG, group.Armed),
                new XElement(PROCESSES_TAG,
                    group.ProcessNames.Select(p => new XElement(PROCESS_TAG, p))));
        }

        /// <summary>Deserializes a TargetProcessGroup from XML. </summary>
        /// <param name="xml">The XML.</param>
        /// <exception cref="ArgumentNullException"></exception>
        private TargetProcessGroup GroupFromXml(XElement xml) {
            if (xml == null) throw new ArgumentNullException(nameof(xml));

            var result = new TargetProcessGroup();
            result.Name = xml.Element(NAME_TAG).Value;
            result.Armed = bool.Parse(xml.Element(ARMED_TAG).Value);

            var processNames = xml.Element(PROCESSES_TAG)
                .Elements(PROCESS_TAG)
                .Select(p => p.Value);

            result.ProcessNames.AddRange(processNames);
            return result;
        }

        #endregion
    }
}
