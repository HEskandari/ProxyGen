using System;
using System.CodeDom;
using System.Linq;
using System.ServiceModel;
using ProxyGen.Generators;

namespace ProxyGen
{
    public class CodeGeneratorFactory
    {
        public static BaseCodeGenerator Create(CodeTypeDeclaration type)
        {
            if(IsProxyType(type))
            {
                return new ProxyCodeGenerator();
            }

            return new ContractCodeGenerator();
        }

        private static bool IsProxyType(CodeTypeDeclaration type)
        {
            var baseClassName = typeof(ClientBase<>).FullName;

            return type.BaseTypes
                       .Cast<CodeTypeReference>()
                       .Any(t => t.BaseType.Equals(baseClassName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}