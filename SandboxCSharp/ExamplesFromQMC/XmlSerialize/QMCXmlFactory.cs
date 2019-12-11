using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Unity;

namespace SandboxCSharp.ExamplesFromQMC.XmlSerialize
{
    class QMCXmlFactory : IFactory
    {
        public void DoRegister(IUnityContainer Container)
        {
           
        }

        public void Run(IUnityContainer Container)
        {
            var cnfg = new QlTvProductConfiguration();
            cnfg.DeviceTypes = new List<DeviceType>();
            cnfg.DeviceTypes.Add(new DeviceType { billingProductId = 152, id = 23 });

            var qltvserializer = new XmlSerializer(typeof(QlTvProductConfiguration));

            using (FileStream fileStream = new FileStream("QMCBillingProductConfig.xml",  FileMode.OpenOrCreate, FileAccess.ReadWrite))
            { 
                qltvserializer.Serialize(fileStream, cnfg);                
            }
        }
    }
}
