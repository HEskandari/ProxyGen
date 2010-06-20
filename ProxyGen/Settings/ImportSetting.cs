using System.Xml.Serialization;

namespace ProxyGen.Settings
{
    public class ImportSetting
    {
        [XmlAttribute("Namespace")]
        public string ImportName
        {
            get; set;
        }
    }
}