using System.Xml.Serialization;

namespace ProxyGen.Settings
{
    public class TypeMappingSetting
    {
        [XmlAttribute]
        public string From
        {
            get; set;
        }

        [XmlAttribute()]
        public string To
        {
            get; set;
        }
    }
}