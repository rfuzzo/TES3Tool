using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TES3Lib.Interfaces;
using TES3Lib.Subrecords.Shared;
using Utility;

namespace Tes3EditX.Winui.Helpers
{
    public class RecordTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }
        public DataTemplate IntegerTemplate { get; set; }
      

        public DataTemplate Common { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is IStringView)
            {
                return StringTemplate;
            }
            else if (item is IIntegerView)
            {
                return IntegerTemplate;
            }
            else
            {
                return Common;
            }
        }
    }
}
