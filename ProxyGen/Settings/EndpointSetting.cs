using System.Xml.Serialization;

namespace ProxyGen.Settings
{
    public class EndPointSetting
    {
        [XmlAttribute]
        public string Uri
        {
            get; set;
        }
    }
}