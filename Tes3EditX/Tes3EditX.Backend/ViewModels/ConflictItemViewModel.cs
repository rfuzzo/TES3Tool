using System.Reflection;
using TES3Lib.Base;
using TES3Lib.Interfaces;

namespace Tes3EditX.Backend.ViewModels;

/// <summary>
/// This viewmodel wraps a TES3 record's fileinfo
/// and a plugin name 
/// </summary>
public class ConflictItemViewModel
{
    private readonly FileInfo _path;

    public ConflictItemViewModel(FileInfo info, Record record)
    {
        _path = info;
        Record = record;

        // get properties with reflection recursively

        var recordProperties = record.GetType().GetProperties(
            BindingFlags.Public | 
            BindingFlags.Instance | 
            BindingFlags.DeclaredOnly
            ).ToList();
        foreach (PropertyInfo? prop in recordProperties)
        {
            // unneeded with DeclaredOnly
            if (
                prop.Name is
                (nameof(Record.Name)) or
                (nameof(Record.Size)) or 
                (nameof(Record.Header)) or 
                (nameof(Record.Flags)) or 
                (nameof(Record.DELE)))
            {
                continue;
            }

            //var v = prop.GetValue(record);
            //if (v is Subrecord subrecord)
            //{
            //    // flatten data records
            //    if (subrecord is IDataView data)
            //    {
            //        // add multiple
            //        // TODO
            //        foreach (var kvp in data.GetData())
            //        {
            //            Fields.Add(new(kvp.Value, kvp.Key));
            //        }
            //    }
            //    else if (subrecord is IStringView s)
            //    {
            //        Fields.Add(new(s.Text, prop.Name));
            //    }
            //    else if (subrecord is IIntegerView i)
            //    {
            //        Fields.Add(new(i.Value, prop.Name));
            //    }
            //    else if (subrecord is IFloatView f)
            //    {
            //        Fields.Add(new(f.Value, prop.Name));
            //    }
            //    else
            //    {
            //        Fields.Add(new(subrecord, prop.Name));
            //    }
            //}
            //else
            //{
            //    Fields.Add(new(null, prop.Name));
            //}
        }



    }

    public string Name => _path.Name;

    public Record Record { get; }

    public List<RecordFieldViewModel> Fields { get; set; } = new();
}
