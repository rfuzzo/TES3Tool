using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TES3Lib.Base;
using TES3Lib.Records;

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
        var members = record.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
        foreach (var prop in members)
        {
            var v = prop.GetValue(record);
            if (v != null)
            {
                Fields.Add( v);
            }
        }
    }

    public string Name => _path.Name;

    public Record Record { get; }

    public List<object> Fields { get; set; } = new();
}
