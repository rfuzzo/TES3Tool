using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TES3Lib.Base;

namespace Tes3EditX.Maui.Extensions
{
    public static class Tes3Extensions
    {
        public static string GetUniqueId(this Record record)
        {
            return $"{record.Name},{record.GetEditorId()}";
        }

        public static ulong GetUniqueNameHash(this Record record)
        {
            return 0;
        }
    }
}
