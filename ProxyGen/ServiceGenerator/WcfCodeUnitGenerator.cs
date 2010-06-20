using System;
using System.CodeDom;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web.Services.Description;
using ProxyGen.Settings;
using ProxyGen.Helper;

namespace ProxyGen.ServiceGenerator
{
    public class WcfCodeUnitGenerator : BaseCodeUnitGenerator
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
                var contracts = importer.ImportAllContracts();
                var endpoints = importer.ImportAllEndpoints();

                var codeNamespace = new CodeNamespace(ProxyGeneratorSettings.Options.Services.Namespace);
                codeCompileUnit.Namespaces.Add(codeNamespace);

                foreach(ContractDescription contract in contracts)
                {
                    generator.GenerateServiceContractType(contract);
                }

                if(generator.Errors.HasWarnings())
                {
                    Logger.WarnFormat("There was warnings when importing WSDL file at {0}. Warning message is: {1}.", wsdl.Uri, generator.Errors.GetWarnings());
                }
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
            
            var metaDocs = mex.GetMetadata(new Uri(wsdl.Uri), MetadataExchangeClientMode.HttpGet);
            return new WsdlImporter(metaDocs);
        }
    }
}