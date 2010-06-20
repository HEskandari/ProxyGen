using System.CodeDom;
using ProxyGen.Settings;

namespace ProxyGen
{
    public class CodeGeneratorInitializer
    {
        public CodeGeneratorInitializer(BaseCodeGenerator generator)
        {
            Generator = generator;
        }

        private BaseCodeGenerator Generator
        {
            get; set;
        }

        public void InitializeMain()
        {
            var newNamespace = new CodeNamespace(Generator.Setting.Namespace);
            
            foreach (CodeNamespaceImport import in Generator.Namespace.Imports)
                newNamespace.Imports.Add(import);

            foreach (ImportSetting external in ProxyGeneratorSettings.Options.Imports)
                newNamespace.Imports.Add(new CodeNamespaceImport(external.ImportName));

            newNamespace.Types.Add(Generator.CodeType);

            var newUnit = new CodeCompileUnit();
            newUnit.Namespaces.Add(newNamespace);
            
            Generator.CodeUnit = newUnit;
            Generator.Namespace = newNamespace;
            Generator.CodeType.IsPartial = true;
        }

        public void InitializePartial()
        {
            var newNamespace = new CodeNamespace(Generator.Setting.Namespace);
            var partialType = new CodeTypeDeclaration(Generator.CodeType.Name);
            var partialUnit = new CodeCompileUnit();

            partialUnit.Namespaces.Add(newNamespace);
            partialType.IsPartial = true;
            partialType.Attributes = Generator.CodeType.Attributes;
            partialType.TypeAttributes = Generator.CodeType.TypeAttributes;

            newNamespace.Types.Add(partialType);

            Generator.PartialUnit = partialUnit;
        }
    }
}