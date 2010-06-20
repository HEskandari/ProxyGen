using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using log4net;
using ProxyGen.Helper;

namespace ProxyGen
{
    public class CodeGeneratorFileWriter : IDisposable
    {
        private readonly StringBuilder _builderMain;
        private readonly StringBuilder _builderPartial;
        private readonly BaseCodeGenerator _generator;
        private readonly CodeDomProvider _provider;
        private readonly StringWriter _writerMain;
        private readonly StringWriter _writerPartial;
        private readonly CodeGeneratorOptions _options;

        public CodeGeneratorFileWriter(BaseCodeGenerator generator)
        {
            _generator = generator;
            _builderMain = new StringBuilder();
            _builderPartial = new StringBuilder();
            _writerMain = new StringWriter(_builderMain);
            _writerPartial = new StringWriter(_builderPartial);
            _options = new CodeGeneratorOptions { BracingStyle = "C" };
            _provider = CodeDomProvider.CreateProvider(ProxyGeneratorSettings.Options.Language);

            Logger = LogManager.GetLogger(typeof (CodeGeneratorFileWriter));
        }

        public void Write()
        {
            GeneratorSteps();

            Compile();

            WriteGeneratedFile();

            WritePartialFile();
        }

        private ILog Logger
        {
            get; set;
        }

        private void Compile()
        {
            _provider.GenerateCodeFromCompileUnit(_generator.CodeUnit, _writerMain, _options);
            _provider.GenerateCodeFromCompileUnit(_generator.PartialUnit, _writerPartial, _options);
        }

        private void GeneratorSteps()
        {
            _generator.Setting.OutputDirectory.EnsureFolderExists();
            _generator.OnBeforeGenerate();
        }

        private void WriteGeneratedFile()
        {
            var className = _generator.CodeType.Name;
            var partialFile = string.Format("{0}.{1}.{2}", className, _generator.Setting.PartialFileSuffix, _provider.FileExtension);
            var generatedFilePath = _generator.Setting.OutputDirectory.GetFilePath(partialFile);

            Logger.InfoFormat("Writing generated file for {0}", className);
            
            WriteToMainFile(generatedFilePath);
        }

        private void WritePartialFile()
        {
            var className = _generator.CodeType.Name;
            var generatedFile = string.Format("{0}.{1}", className, _provider.FileExtension);
            var partialFilePath = _generator.Setting.OutputDirectory.GetFilePath(generatedFile);
            var file = new FileInfo(partialFilePath);

            if(!ProxyGeneratorSettings.Options.OverwritePartials && file.Exists)
            {
                Logger.InfoFormat("Skipped overwriting existing file: {0}", className);
                return;
            }

            if(!_generator.ShouldGeneratePartialCode())
            {
                Logger.InfoFormat("Skipped generating partial class for {0}. Type is not partialable.", className);
                return;
            }

            Logger.InfoFormat("Writing partial file for {0}", className);
            WriteToPartialFile(partialFilePath);
        }

        private void WriteToMainFile(string path)
        {
            WriteContentToFile(_builderMain.ToString(), path);
        }

        private void WriteToPartialFile(string path)
        {
            WriteContentToFile(_builderPartial.ToString(), path);
        }

        private void WriteContentToFile(string content, string path)
        {
            StreamWriter f = File.CreateText(path);
            f.Write(content);
            f.Close();
        }

        public void Dispose()
        {
            if(_writerMain != null)
            {
                _writerMain.Flush();
                _writerMain.Close();
            }
        }
    }
}