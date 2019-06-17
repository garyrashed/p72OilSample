using System.IO;

namespace P72OilTest.Infrastructure.Utilities
{
    internal static class AppUtils
    {
        public static string GetAppName()
        {
            var nameOfExe = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            return Path.GetFileNameWithoutExtension(nameOfExe);
        }
    }
}
