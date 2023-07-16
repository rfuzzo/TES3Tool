using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tes3EditX.Backend.Extensions;
using Tes3EditX.Backend.Models;
using Tes3EditX.Backend.ViewModels;
using TES3Lib;
using TES3Lib.Base;

namespace Tes3EditX.Backend.Services;

public partial class CompareService : ObservableObject, ICompareService
{
    [ObservableProperty]
    private Dictionary<string, List<string>> _conflicts;

    public IEnumerable<PluginItemViewModel> Selectedplugins { get; set; } = new List<PluginItemViewModel>();

    public CompareService()
    {
        Conflicts = new();
    }

    // todo get load order right
    // todo use hashes
    public void CalculateConflicts()
    {
        if (Selectedplugins is null)
        {
            return;
        }

        Conflicts.Clear();

        // map plugin records
       
        var pluginMap = new Dictionary<string, HashSet<string>>();
        foreach (var model in Selectedplugins)
        {
            var plugin = TES3.TES3Load(model.FileInfo.FullName);
            var records = plugin.Records
                .Where(x => x is not null)
                .Select(x => x.GetUniqueId())
                .ToHashSet();

            
            pluginMap.Add(plugin.Path, records);
        }


        Dictionary<string, List<string>> conflict_map = new();
        foreach (var (pluginKey, records) in pluginMap)
        {
            List<string> newrecords = new();
            foreach (var record in records)
            {
                // then we have a conflict
                if (conflict_map.ContainsKey(record))
                {
                    conflict_map[record].Add(pluginKey);
                }
                // no conflict, store for later adding
                else
                {
                    newrecords.Add(record);
                }
            }

            foreach (var item in newrecords)
            {
                conflict_map.Add(item, new() { pluginKey });
            }
        }

        // TODO dedup?
        var singleRecords = conflict_map.Where(x => x.Value.Count < 2).Select(x => x.Key);
        foreach (var item in singleRecords)
        {
            conflict_map.Remove(item);
        }


        Conflicts = conflict_map;
    }

    partial void OnConflictsChanged(Dictionary<string, List<string>> value)
    {
        WeakReferenceMessenger.Default.Send(new ConflictsChangedMessage(value));
    }
}
