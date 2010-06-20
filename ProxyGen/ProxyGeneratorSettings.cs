using System;
using ProxyGen.Helper;
using ProxyGen.Settings;

namespace ProxyGen
{
    public class ProxyGeneratorSettings
    {
        static ProxyGeneratorSettings()
        {
            Options = new ProxyGenSetting();
        }

        public static ProxyGenSetting Options
        {
            get; set;
        }

        public static string ConfigPath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory + "ProxyGen.Settings.xml"; }
        }

        public static void Load()
        {
            Options = ObjectSerializer<ProxyGenSetting>.Load(ConfigPath);
        }

        public static void Save()
        {
            ObjectSerializer<ProxyGenSetting>.Save(Options, ConfigPath);
        }
    }
}