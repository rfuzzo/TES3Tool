using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes3EditX.Maui.Services
{
    public class SettingsService : ISettingsService
    {
        public static string WDIR = "WDIR";

        public DirectoryInfo GetWorkingDirectory()
        {
            return Preferences.Default.Get(WDIR, new DirectoryInfo(Directory.GetCurrentDirectory()));
        }

        public void SetWorkingDirectory(DirectoryInfo value)
        {
            Preferences.Default.Set(WDIR, value);
        }
    }
}
