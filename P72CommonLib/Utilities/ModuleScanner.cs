using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using log4net;

namespace P72CommonLib.Utilities
{
    public class ModuleScanner
    {
        private static readonly ILog _logger;

        static ModuleScanner()
        {
            ModuleScanner._logger = LogManager.GetLogger(typeof(ModuleScanner));
        }

        public ModuleScanner()
        {
        }

        public static void ScanForModules(ContainerBuilder builder, string filePath = null)
        {
            IEnumerable<string> files;
            string workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            files = (string.IsNullOrEmpty(filePath) ?
                from c in Directory.GetFiles(workingDir)
                where (c.EndsWith("exe") ? true : c.EndsWith("dll"))
                select c :
                from c in Directory.GetFiles(filePath)
                where (c.EndsWith("exe") ? true : c.EndsWith("dll"))
                select c);
            foreach (string file in files)
            {
                try
                {
                    IEnumerable<Type> iautofacTypes =
                        from c in (IEnumerable<Type>)Assembly.LoadFile(file).GetTypes()
                        where c.GetInterfaces().Contains<Type>(typeof(IAutofacModule))
                        select c;
                    foreach (Type module in iautofacTypes)
                    {
                        IAutofacModule autofacModule = (IAutofacModule)Activator.CreateInstance(module);
                        ModuleScanner._logger.Debug(string.Format("Found module. FileName={0}. Module={1}", file, module.Name));
                        autofacModule.Register(builder);
                        ModuleScanner._logger.Debug(string.Format("Registered module {0}", module.Name));
                    }
                }
                catch (BadImageFormatException badImageFormatException1)
                {
                    BadImageFormatException badImageFormatException = badImageFormatException1;
                    ModuleScanner._logger.Error(string.Format("Could not load {0} due to badImageFormatException. Module={1}. FusionLog={2}", file, badImageFormatException.Message, badImageFormatException.FusionLog));
                }
                catch (Exception exception)
                {
                    Exception ex = exception;
                    ModuleScanner._logger.Error(string.Format("Failed to load a module for an unknown reason. Error={0}", ex.Message));
                }
            }
        }
    }
}
