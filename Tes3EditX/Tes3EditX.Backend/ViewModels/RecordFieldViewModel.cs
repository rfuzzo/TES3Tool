using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TES3Lib.Base;
using TES3Lib.Interfaces;

namespace Tes3EditX.Backend.ViewModels
{
    public class RecordFieldViewModel : ObservableObject
    {
        public RecordFieldViewModel(Subrecord? wrappedField)
        {
            WrappedField = wrappedField;
        }

        public Subrecord? WrappedField { get; init; }

        public bool IsConflict { get; set; }

        public override string ToString()
        {
            if (WrappedField is IStringView s)
            {
                return s.Text;
            }
            else if (WrappedField is IIntegerView i)
            {
                return i.Value.ToString();
            }
            else if (WrappedField is IFloatView f)
            {
                return f.Value.ToString();
            }
            else
            {
                var str = WrappedField?.ToString();
                return !string.IsNullOrEmpty(str) ? str : "NULL";
            }
        }
    }
}
