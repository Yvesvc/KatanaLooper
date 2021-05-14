using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatanaLooper.Settings
{
    public static class GeneralSettings
    {
        private static string rootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        public static string IconsDirectory { get => rootPath + "\\Icons\\"; }
        public static string ImagesDirectory { get => rootPath + "\\Images\\"; }
        public static string RecordingsDirectory { get => rootPath + "\\Recordings\\"; }
    }
}
