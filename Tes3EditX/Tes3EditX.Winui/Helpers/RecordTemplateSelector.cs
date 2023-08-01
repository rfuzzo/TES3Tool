using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tes3EditX.Backend.ViewModels;
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

        protected override DataTemplate SelectTemplateCore(object value)
        {
            if (value is RecordFieldViewModel vm)
            {
                //if (vm.WrappedField is IStringView)
                //{
                //    return StringTemplate;
                //}
                //else if (vm.WrappedField is IIntegerView)
                //{
                //    return IntegerTemplate;
                //}
                //else
                {
                    return Common;
                }
            }
            else
            { 
                return Common;
            }
        }
    }

    public class RecordViewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Common { get; set; }

        protected override DataTemplate SelectTemplateCore(object value)
        {
            return Common;
        }
    }
}
