using System.ComponentModel;

namespace ProxyGen.Settings
{
    public class ServiceSetting : GeneratorSetting
    {
        public const string DefaultBaseClass = "";

        [DefaultValue(DefaultBaseClass)]
        public string BaseClass
        {
            get; set;
        }
    }
}