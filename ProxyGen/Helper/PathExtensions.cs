using System.IO;
using log4net;

namespace ProxyGen.Helper
{
    public static class PathExtensions
    {
        public static string GetFilePath(this string folder, string file)
        {
            return Path.Combine(folder, file);
        }

        public static void EnsureFolderExists(this string folder)
        {
            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}