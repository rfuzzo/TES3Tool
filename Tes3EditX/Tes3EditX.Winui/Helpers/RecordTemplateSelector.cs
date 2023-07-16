using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TES3Lib.Subrecords.Shared;
using Utility;

namespace Tes3EditX.Winui.Helpers
{
    public class RecordTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NAME { get; set; }
        public DataTemplate MODL { get; set; }
        public DataTemplate Common { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is NAME)
            {
                return NAME;
            }
            else if (item is MODL)
            {
                return MODL;
            }
            else
            {
                return Common;
            }
        }
    }
}
