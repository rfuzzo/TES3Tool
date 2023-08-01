using System.Collections.Generic;
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
    private readonly Dictionary<string, object?> _map = new();

    public ConflictItemViewModel(FileInfo info, Record record, List<string> names)
    {
        _path = info;
        Record = record;

        
        foreach (var name in names)
        {
            _map.Add(name, null);
        }

        // get properties with reflection recursively
        var recordProperties = record.GetType().GetProperties(
               BindingFlags.Public |
               BindingFlags.Instance |
               BindingFlags.DeclaredOnly).ToList();
        foreach (PropertyInfo prop in recordProperties)
        {
            var v = prop.GetValue(record);

            if (v is Subrecord subrecord)
            {
                var subRecordProperties = subrecord.GetType().GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.DeclaredOnly).ToList();
                foreach (PropertyInfo subProp in subRecordProperties)
                {
                    if (_map.ContainsKey($"{subrecord.Name}.{subProp.Name}"))
                    {
                        _map[$"{subrecord.Name}.{subProp.Name}"] = subProp.GetValue(subrecord);
                    }
                    else if (_map.ContainsKey(subProp.Name))
                    {
                        _map[subProp.Name] = subProp.GetValue(subrecord);
                    }
                }
            }
        }

        // fill fields
        // todo to refactor
        foreach (var (name, field) in _map)
        {
            Fields.Add(new(field, name));
        }

    }

    public string Name => _path.Name;

    public Record Record { get; }

    public List<RecordFieldViewModel> Fields { get; set; } = new();
}
