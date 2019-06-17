using System;
using Autofac;
using log4net;
using P72CommonLib.Utilities;
using P72OilTest.Infrastructure.Utilities;
using P72OilTest.Services;

namespace P72OilTest
{
    public class P72OilTestEntrypoint
    {
        private static ILog _logger;
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            GlobalContext.Properties["LogName"] = AppUtils.GetAppName();
            log4net.Config.XmlConfigurator.Configure();
            _logger = LogManager.GetLogger(typeof(P72OilTestEntrypoint));

            LogHeader();
            var builder = new ContainerBuilder();
            ModuleScanner.ScanForModules(builder);
            Container = builder.Build();
            _logger.Info($"Modules fully loaded.");

            using (var scope = Container.BeginLifetimeScope())
            {
                try
                {
                    var oilService = scope.Resolve<IP72Service>();
                    oilService.Run();
                }
                catch (Exception ee)
                {
                    _logger.Error("Unhandled Error", ee);
                }
                finally
                {
                    Console.ReadLine();
                }
            }

            //post op
            LogFooter();
        }

        private static void LogHeader()
        {
            _logger.Info($"Header");
        }

        private static void LogFooter()
        {
            _logger.Info($"Footer");
        }
    }
}
