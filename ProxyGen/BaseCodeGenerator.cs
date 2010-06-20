using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using ProxyGen.Settings;

namespace ProxyGen
{
    public abstract class BaseCodeGenerator
    {
        public void Initialize(CodeCompileUnit unit, CodeNamespace ns)
        {
            CodeUnit = unit;
            Namespace = ns;

            var initer = new CodeGeneratorInitializer(this);

            initer.InitializeMain();
            initer.InitializePartial();
        }

        public abstract GeneratorSetting Setting { get; }

        public CodeCompileUnit PartialUnit
        {
            get; set;
        }

        public CodeCompileUnit CodeUnit
        {
            get; set;
        }

        public CodeTypeDeclaration CodeType
        {
            get; set;
        }

        public CodeNamespace Namespace
        {
            get; set;
        }

        public virtual void OnBeforeGenerate()
        {
        }

        public virtual void OnAfterGenerate()
        {
        }

        public bool ShouldGeneratePartialCode()
        {
            return CodeType.IsEnum == false;
        }
    }
}