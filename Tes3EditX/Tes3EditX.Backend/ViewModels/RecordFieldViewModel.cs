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
        public RecordFieldViewModel(object? wrappedField, string name)
        {
            WrappedField = wrappedField;
            Name = name;
        }

        public object? WrappedField { get; init; }
        public string Name { get; }
        public bool IsConflict { get; set; }

        // we display only the text in the normal compare view 
        // and double click opens an editor
        public override string ToString()
        {
            //    if (WrappedSubrecord is IStringView s)
            //    {
            //        return s.Text;
            //    }
            //    else if (WrappedSubrecord is IIntegerView i)
            //    {
            //        return i.Value.ToString();
            //    }
            //    else if (WrappedSubrecord is IFloatView f)
            //    {
            //        return f.Value.ToString();
            //    }
            //    else
            //    {
            var str = WrappedField?.ToString();
            return !string.IsNullOrEmpty(str) ? str : "NULL";
            //    }
        }
    }
}
