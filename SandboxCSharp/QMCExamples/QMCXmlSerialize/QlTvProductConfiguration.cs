using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
#pragma warning disable 618

namespace SandboxCSharp.QMCExamples.QMCXmlSerialize
{
    [Serializable]
    public class ProductConfiguration
    {
        [XmlAnyElement]
        public List<XmlNode> Elements { get; set; }
    }

    [Serializable]
    [XmlRoot("Qltv")]
    public class QlTvProductConfiguration
    {
        private int _totalDevices;

        public int OttDevices { get; set; }

        public int ConcurrentSessions { get; set; }

        /// <summary>
        /// A demon of the ancient world. Only needed for legacy XML support (backwards compatibility).
        /// Obsolete, use property [TotalDevices] and [IncludedDevices] instead.
        /// </summary>
        [Obsolete("Use property [TotalDevices] and [IncludedDevices] instead.")]
        public int AdditionalDevices { get; set; }
  
        public int IncludedDevices { get; set; }

        [XmlArray("DeviceTypes", IsNullable = true)]
        [XmlArrayItem("DeviceType")]
        public List<DeviceType> DeviceTypes { get; set; }

        public int TotalDevices
        {
            get
            {
                // Legacy fallback:
                if (_totalDevices == 0 && AdditionalDevices > 0)
                {
                    _totalDevices = AdditionalDevices;
                }

                return _totalDevices;
            }
            set { _totalDevices = value; }
        }

        public int NpvrHourQuota { get; set; }

        [XmlArray("DtvBouquets")]
        [XmlArrayItem("DtvBouquet")]
        public List<int> DtvBouquets { get; set; }

        [XmlArray("SvodPackages")]
        [XmlArrayItem("SvodPackage")]
        public List<int> SvodPackages { get; set; }

        [XmlArray("AutoterminatedDtvBouquets")]
        [XmlArrayItem("AutoterminatedDtvBouquet")]
        public List<int> AutoterminatedDtvBouquets { get; set; }

        /// <summary>
        /// Returns [TotalDevices] minus [IncludedDevices].
        /// </summary>
        /// <returns></returns>
        public int GetAmountOfAvailableDevices()
        {
            return TotalDevices - IncludedDevices;
        }
    }

    [Serializable]
    public class DeviceType
    {
        [XmlAttribute]
        public int id { get; set; }

        [XmlAttribute]
        public int billingProductId { get; set; }
    }
}