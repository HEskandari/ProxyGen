using System;
using ProxyGen.ServiceGenerator;
using ProxyGen.Settings;

namespace ProxyGen
{
    public class CodeUnitGeneratorFactory
    {
        public static IServiceCodeGenerator Create()
        {
            switch(ProxyGeneratorSettings.Options.TargetType)
            {
                case ServiceTargetType.ASMX:
                    return new AsmxCodeUnitGenerator();
                case ServiceTargetType.WCF:
                    return new WcfCodeUnitGenerator();
                case ServiceTargetType.Silverlight:
                    return new SilverlightCodeUnitGenerator();
                default:
                    throw new NotImplementedException("This target type is not implemented");
            }
        }
    }
}