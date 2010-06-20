using System;
using log4net;
using log4net.Appender;
using log4net.Layout;
using ProxyGen.Helper;

namespace ProxyGen
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigureLogger();

            GenerateFiles();

            Logger.Info("Finished generating files.");

            Console.ReadLine();
        }

        private static void GenerateFiles()
        {
            try
            {
                new ProxyGenerator().Generate();
            }
            catch (Exception ex)
            {
                Logger.FatalFormat("There was an error generating. Reason: {0}", ex.GetExceptionReport());
            }
        }

        private static ILog Logger
        {
            get; set;
        }

        private static void ConfigureLogger()
        {
            var appender = new ConsoleAppender();
            appender.Layout = new PatternLayout("%message %newline");

            log4net.Config.BasicConfigurator.Configure(appender);
            Logger = LogManager.GetLogger(typeof (ProxyGenerator));
        }
    }
}
