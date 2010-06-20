using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Net;
using log4net;
using ProxyGen.Settings;

namespace ProxyGen.ServiceGenerator
{
    public abstract class BaseCodeUnitGenerator : IServiceCodeGenerator
    {
        protected BaseCodeUnitGenerator()
        {
            Logger = LogManager.GetLogger(typeof(AsmxCodeUnitGenerator));
            CodeUnits = new List<CodeCompileUnit>();
            FileGenerators = new List<BaseCodeGenerator>();
        }

        public void Initialize()
        {
            foreach (var wsdl in ProxyGeneratorSettings.Options.EndPoints)
            {
                var unit = GenerateCode(wsdl);
                if (unit != null)
                {
                    CodeUnits.Add(unit);
                }
            }
        }

        protected ILog Logger
        {
            get;
            set;
        }

        protected IList<CodeCompileUnit> CodeUnits
        {
            get;
            set;
        }

        protected IList<BaseCodeGenerator> FileGenerators
        {
            get;
            set;
        }

        protected abstract CodeCompileUnit GenerateCode(EndPointSetting wsdl);

        public void Prepare()
        {
            foreach (var unit in CodeUnits)
            {
                foreach (CodeNamespace nameSpace in unit.Namespaces)
                {
                    foreach (CodeTypeDeclaration type in nameSpace.Types)
                    {
                        var generator = CodeGeneratorFactory.Create(type);

                        generator.CodeType = type;
                        generator.Initialize(unit, nameSpace);

                        FileGenerators.Add(generator);
                    }
                }
            }
        }

        public void Write()
        {
            foreach (var generator in FileGenerators)
            {
                new CodeGeneratorFileWriter(generator).Write();
            }
        }

        protected string TryGetContent(string uri)
        {
            Logger.InfoFormat("Trying to get WSDL content from {0} Uri.", uri);
            string content = TryGetContentByHttp(uri);

            if (string.IsNullOrEmpty(content))
            {
                Logger.InfoFormat("Trying to get WSDL content from file at {0}.", uri);
                content = TryGetContentByFile(uri);
            }

            return content;
        }

        private string TryGetContentByFile(string uri)
        {
            if (!File.Exists(uri))
            {
                Logger.WarnFormat("WSDL file at {0} could not be processed because file does not exist.", uri);
                return null;
            }

            try
            {
                return File.ReadAllText(uri);
            }
            catch
            {
                Logger.WarnFormat("There was a problem accessing WSDL at {0}.", uri);
                return null;
            }
        }

        private string TryGetContentByHttp(string uri)
        {
            if (uri.IndexOf("http://", 0, 8, StringComparison.InvariantCultureIgnoreCase) == -1 &&
                uri.IndexOf("https://", 0, 9, StringComparison.InvariantCultureIgnoreCase) == -1)
                return null;

            try
            {
                var webRequest = WebRequest.Create(uri);

                using (var webResponse = webRequest.GetResponse())
                using (var webStream = webResponse.GetResponseStream())
                using (var reader = new StreamReader(webStream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch
            {
                Logger.WarnFormat("There was a problem accessing the WSDL over the web at {0}", uri);
                return null;
            }
        }
    }
}