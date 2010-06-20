using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Schema;
using ProxyGen.Settings;

namespace ProxyGen.ServiceGenerator
{
    public class AsmxCodeUnitGenerator : BaseCodeUnitGenerator
    {
        protected override CodeCompileUnit GenerateCode(EndPointSetting wsdl)
        {
            if (wsdl == null || string.IsNullOrEmpty(wsdl.Uri))
                throw new ApplicationException("WSDL Uri in endpoint section of the configuration is empty.");

            var wsdlContent = TryGetContent(wsdl.Uri);
            if(string.IsNullOrEmpty(wsdlContent))
                throw new ApplicationException("Could not get content from WSDL at " + wsdl.Uri);

            var codeCompileUnit = new CodeCompileUnit();

            try
            {
                var reader = new StringReader(wsdlContent);
                var serviceDescription = ServiceDescription.Read(reader);
                var codeNamespace = new CodeNamespace(ProxyGeneratorSettings.Options.Services.Namespace);
                var importer = new ServiceDescriptionImporter();

                RemovePolicyFormat(serviceDescription);

                importer.ProtocolName = "Soap";
                importer.AddServiceDescription(serviceDescription, null, null);

                AddExternalSchema(serviceDescription, importer);

                codeCompileUnit.Namespaces.Add(codeNamespace);

                ServiceDescriptionImportWarnings warning = importer.Import(codeNamespace, codeCompileUnit);
                if (warning != 0)
                {
                    Logger.WarnFormat("There was warnings when importing WSDL file at {0}. Warning message is: {1}.", wsdl.Uri, warning);
                }
            }
            catch (Exception ex)
            {
                string message = "The wsdl with path:" + wsdl.Uri + " could not be processed.Error during the CodeCompileUnit generation.";
                throw new ApplicationException(message, ex);
            }

            return codeCompileUnit;
        }

        private void AddExternalSchema(ServiceDescription sd, ServiceDescriptionImporter importer)
        {
            foreach (XmlSchema wsdlSchema in sd.Types.Schemas)
            {
                foreach (XmlSchemaObject externalSchema in wsdlSchema.Includes)
                {
                    if (externalSchema is XmlSchemaImport)
                    {
                        var uri = ((XmlSchemaExternal) externalSchema).SchemaLocation;
                        var content = TryGetContent(uri);

                        if (!string.IsNullOrEmpty(content))
                        {
                            var schemaReader = new StringReader(content);
                            var schema = XmlSchema.Read(schemaReader, null);
                            
                            importer.Schemas.Add(schema);
                        }
                    }
                }
            }
        }

        private void RemovePolicyFormat(ServiceDescription serviceDescription)
        {
            RemovePolicyFormatFromExtensions(serviceDescription.Extensions);
            RemovePolicyFormatFromBinding(serviceDescription.Bindings);
        }

        private static void RemovePolicyFormatFromBinding(BindingCollection bindings)
        {
            foreach (Binding binding in bindings)
            {
                foreach (OperationBinding opBinding in binding.Operations)
                {
                    RemovePolicyFormatFromExtensions(opBinding.Extensions);
                }
            }
        }

        private static void RemovePolicyFormatFromExtensions(ServiceDescriptionFormatExtensionCollection extensions)
        {
            var toRemove = new List<XmlElement>();

            extensions.OfType<XmlElement>()
                      .Where(element => element.Name.EndsWith(":Policy") || 
                             element.Name.EndsWith(":PolicyReference"))
                      .ToList()
                      .ForEach(toRemove.Add);

            toRemove.ForEach(extensions.Remove);
        }
    }
}