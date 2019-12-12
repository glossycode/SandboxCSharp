using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Unity;

namespace SandboxCSharp.QMCExamples.QMCXmlSerialize
{
    class QMCXmlSerializeFactory : IFactory
    {
        public void DoRegister(IUnityContainer Container)
        {

        }

        public void Run(IUnityContainer Container)
        {
            SaveToFile();

            string theDataWithDeviceTypes = "<ProductConfiguration xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">"
            + "  <Qltv>"
            + "    <ConcurrentSessions>4</ConcurrentSessions>"
            + "    <OttDevices>10</OttDevices>"
            + "    <BouquetBoxGroupsWithDevice>"
            + "      <BouquetBoxGroup id=\"7\" />"
            + "    </BouquetBoxGroupsWithDevice>"
            + "    <IncludedDevices>3</IncludedDevices>"
            + "    <DeviceTypes>"
            + "	   <DeviceType id=\"27\" billingProductId =\"977\" />	   "
            + "	</DeviceTypes>"
            + "    <TotalDevices>5</TotalDevices>"
            + "    <NpvrHourQuota>500</NpvrHourQuota>"
            + "    <DtvBouquetBoxGroupTyp>44</DtvBouquetBoxGroupTyp>"
            + "    <SvodPackages />"
            + "  </Qltv>"
            + "</ProductConfiguration>";


            string theDataWithNoDeviceTypes = "<ProductConfiguration xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">"
            + "  <Qltv>"
            + "    <ConcurrentSessions>4</ConcurrentSessions>"
            + "    <OttDevices>10</OttDevices>"
            + "    <BouquetBoxGroupsWithDevice>"
            + "      <BouquetBoxGroup id=\"7\" />"
            + "    </BouquetBoxGroupsWithDevice>"
            + "    <IncludedDevices>3</IncludedDevices>"
            + "    <TotalDevices>5</TotalDevices>"
            + "    <NpvrHourQuota>500</NpvrHourQuota>"
            + "    <DtvBouquetBoxGroupTyp>44</DtvBouquetBoxGroupTyp>"
            + "    <SvodPackages />"
            + "  </Qltv>"
            + "</ProductConfiguration>";

            var qltvProductConfiguration = GetQltvProductConfiguration(theDataWithDeviceTypes);

            var qltvProductConfigurationWO = GetQltvProductConfiguration(theDataWithNoDeviceTypes);

            if (qltvProductConfigurationWO.DeviceTypes.Count > 0)
            {

            }
        }

        private static QlTvProductConfiguration GetQltvProductConfiguration(string xmltext)
        {
            var xmlSerializer = new XmlSerializer(typeof(ProductConfiguration));
            var textReader = new StringReader(xmltext);
            var productConfiguration = xmlSerializer.Deserialize(textReader) as ProductConfiguration;

            if (productConfiguration == null)
            {
                return null;
            }

            var qltvserializer = new XmlSerializer(typeof(QlTvProductConfiguration));
            var qltvNodeReader = new XmlNodeReader(productConfiguration.Elements.Find(e => e.Name == "Qltv"));

            return qltvserializer.Deserialize(qltvNodeReader) as QlTvProductConfiguration;
        }

        private static string _fileName = "QMCBillingProductConfig.xml";
        private static void SaveToFile()
        {
            var cnfg = new QlTvProductConfiguration();
            cnfg.DeviceTypes = new List<DeviceType>();
            cnfg.DeviceTypes.Add(new DeviceType { billingProductId = 152, id = 23 });

            var qltvserializer = new XmlSerializer(typeof(QlTvProductConfiguration));

            using (FileStream fileStream = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                qltvserializer.Serialize(fileStream, cnfg);
            }
        }
    }
}
