using System.Xml.Serialization;

namespace ProxyGen.Settings
{
    public abstract class GeneratorSetting
    {
        public GeneratorSetting()
        {
            Namespace = string.Empty;
            OutputDirectory = string.Empty;
            PartialFileSuffix = "Designer";
            GeneratePartial = true;
        }
        
        [XmlIgnore]
        public string PartialFileSuffix
        {
            get; set;
        }

        public bool GeneratePartial
        {
            get; set;
        }

        public string Namespace
        {
            get; set;
        }

        public string OutputDirectory
        {
            get; set;
        }
    }
}