using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes3EditX.Maui.Services
{
    public interface ISettingsService
    {
        DirectoryInfo GetWorkingDirectory();
        void SetWorkingDirectory(DirectoryInfo value);
    }
}
