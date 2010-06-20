using ProxyGen.Settings;

namespace ProxyGen.Generators
{
    public class ProxyCodeGenerator : BaseCodeGenerator
    {
        public override GeneratorSetting Setting
        {
            get { return ProxyGeneratorSettings.Options.Services; }
        }
    }
}