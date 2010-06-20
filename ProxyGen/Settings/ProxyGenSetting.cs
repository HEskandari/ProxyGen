using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProxyGen.Settings
{
    public class ProxyGenSetting
    {
        public ProxyGenSetting()
        {
            Language = "CSharp";
            TargetType = ServiceTargetType.ASMX;
            OverwritePartials = false;
            Services = new ServiceSetting();
            Contracts = new ContractSetting();
            Imports = new List<ImportSetting>();
            TypeMappings = new List<TypeMappingSetting>();
            EndPoints = new List<EndPointSetting>();
        }

        public string Language
        {
            get; set;
        }

        public ServiceTargetType TargetType
        {
            get; set;
        }

        public bool OverwritePartials
        {
            get; set;
        }

        [XmlElement]
        public ServiceSetting Services
        {
            get; set;
        }

        [XmlElement]
        public ContractSetting Contracts
        {
            get; set;
        }

        [XmlArrayItem("Import")]
        public List<ImportSetting> Imports
        {
            get; set;
        }

        [XmlArrayItem("TypeMapping")]
        public List<TypeMappingSetting> TypeMappings
        {
            get; set;
        }

        [XmlArrayItem("EndPoint")]
        public List<EndPointSetting> EndPoints
        {
            get; set;
        }
    }
}