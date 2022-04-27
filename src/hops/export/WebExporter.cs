using System;
using System.IO;

namespace Hops.export
{
    public static class WebExporter
    {
        public static void CreateProject(string directory, string projectName)
        {
            string fullPath = Path.Combine(directory, projectName);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

        }
    }
}
