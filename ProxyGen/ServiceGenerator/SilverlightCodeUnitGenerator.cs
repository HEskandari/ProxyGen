using System;
using System.CodeDom;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Silverlight.ServiceReference;
using ProxyGen.Settings;
using ProxyGen.Helper;
using System.Linq;

namespace ProxyGen.ServiceGenerator
{
    public class SilverlightCodeUnitGenerator : BaseCodeUnitGenerator
    {
        protected override CodeCompileUnit GenerateCode(EndPointSetting wsdl)
        {
            if (wsdl == null || string.IsNullOrEmpty(wsdl.Uri))
                throw new ApplicationException("WSDL Uri in endpoint section of the configuration is empty.");

            var wsdlContent = TryGetContent(wsdl.Uri);
            if (string.IsNullOrEmpty(wsdlContent))
                throw new ApplicationException("Could not get content from WSDL at " + wsdl.Uri);

            var codeCompileUnit = new CodeCompileUnit();

            try
            {
                var importer = CreateImporter(wsdl);
                var generator = new ServiceContractGenerator(codeCompileUnit);
                var ns = ProxyGeneratorSettings.Options.Services.Namespace;

                generator.Options = ServiceContractGenerationOptions.AsynchronousMethods | 
                                    ServiceContractGenerationOptions.ChannelInterface |
                                    ServiceContractGenerationOptions.ClientClass |
                                    ServiceContractGenerationOptions.EventBasedAsynchronousMethods;

                // Data contract options
                importer.State.Remove(typeof(XsdDataContractExporter));
                var contractImporter = new XsdDataContractImporter();
                contractImporter.Options = new ImportOptions { EnableDataBinding = true };
                contractImporter.Options.Namespaces.Add("*", ns);
                importer.State.Add(typeof(XsdDataContractImporter), contractImporter);

                var contracts = importer.ImportAllContracts();
                var endpoints = importer.ImportAllEndpoints();

                var codeNamespace = new CodeNamespace(ns);
                codeCompileUnit.Namespaces.Add(codeNamespace);

                foreach(ContractDescription contract in contracts)
                {
                    generator.GenerateServiceContractType(contract);
                }

                if(generator.Errors.HasWarnings())
                {
                    Logger.WarnFormat("There was warnings when importing WSDL file at {0}. Warning message is: {1}.", wsdl.Uri, generator.Errors.GetWarnings());
                }

                // Call SL fixups
                var slfix = new WcfSilverlightCodeGenerationExtension();
                slfix.ClientGenerated(generator);
            }
            catch (Exception ex)
            {
                string message = "The wsdl with path:" + wsdl.Uri + " could not be processed.Error during the CodeCompileUnit generation.";
                throw new ApplicationException(message, ex);
            }

            return codeCompileUnit;
        }

        private WsdlImporter CreateImporter(EndPointSetting wsdl)
        {
            var endpoint = new EndpointAddress(wsdl.Uri);

            var binding = new WSHttpBinding(SecurityMode.None)
            {
                MaxReceivedMessageSize = Int32.MaxValue,
            };

            var mex = new MetadataExchangeClient(binding);
            mex.ResolveMetadataReferences = true;
            
            var sections = new SingleEnumerable<EndpointAddress>(endpoint)
                                    .Select(ma => mex.GetMetadata(ma.Uri, MetadataExchangeClientMode.HttpGet))
                                    .SelectMany(mds => mds.MetadataSections);

            var metaDocs = new MetadataSet(sections); 
            
            return new WsdlImporter(metaDocs);
        }
    }
}