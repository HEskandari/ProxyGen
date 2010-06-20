using ProxyGen.Settings;

namespace ProxyGen.Generators
{
    public class ContractCodeGenerator : BaseCodeGenerator
    {
        public override GeneratorSetting Setting
        {
            get { return ProxyGeneratorSettings.Options.Contracts; }
        }
    }
}