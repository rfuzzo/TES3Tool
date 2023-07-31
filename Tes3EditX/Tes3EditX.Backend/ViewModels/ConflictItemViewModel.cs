using System.Reflection;
using TES3Lib.Base;

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

        // get properties with reflection
        var members = record.GetType().GetProperties(
            BindingFlags.Public | 
            BindingFlags.Instance | 
            BindingFlags.DeclaredOnly
            ).ToList();
        foreach (PropertyInfo? prop in members)
        {
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

            var v = prop.GetValue(record);
            if (v is Subrecord subrecord)
            {
                Fields.Add(new(subrecord));
            }
            else
            {
                Fields.Add(new(null));
            }
        }
    }

    public string Name => _path.Name;

    public Record Record { get; }

    public List<RecordFieldViewModel> Fields { get; set; } = new();
}
