using log4net;

namespace ProxyGen
{
    public class ProxyGenerator
    {
        public ProxyGenerator()
        {
            Logger = LogManager.GetLogger(typeof (ProxyGenerator));
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            Logger.InfoFormat("Loading configuration file from {0}.", ProxyGeneratorSettings.ConfigPath);
            ProxyGeneratorSettings.Load();
            Logger.InfoFormat("Configuration loaded successfully.");
        }

        public void Generate()
        {
            Logger.InfoFormat("Generating code for {0} endpoints.", ProxyGeneratorSettings.Options.EndPoints.Count);
            var cug = CodeUnitGeneratorFactory.Create();

            Logger.Info("Initializing generators.");
            cug.Initialize();

            Logger.Info("Preparing code generation.");
            cug.Prepare();

            Logger.Info("Writing generated code to output.");
            cug.Write();
        }

        private ILog Logger
        {
            get; set;
        }
    }
}