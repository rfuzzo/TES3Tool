using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tes3EditX.Backend.ViewModels;
using Tes3EditX.Maui.Extensions;
using TES3Lib;
using TES3Lib.Base;

namespace Tes3EditX.Backend.Services;

public class CompareService : ICompareService
{
    public Dictionary<string, List<string>> Conflicts { get; set; } = new();

    public IEnumerable<PluginItemViewModel> Selectedplugins { get; set; }

    // todo get load order right
    // todo use hashes
    public void SetConflicts()
    {
        if (Selectedplugins is null)
        {
            return;
        }

        Conflicts.Clear();

        // map plugin records
        var recordMap = new Dictionary<string, HashSet<string>>();
        foreach (var model in Selectedplugins)
        {
            var plugin = TES3.TES3Load(model.FileInfo.FullName);
            recordMap.Add(plugin.Path, plugin.Records
                .Where(x => x is not null)
                .Select(x => x.GetUniqueId())
                .ToHashSet()
                );
        }

        // parse current folder and determine conflicts
        Dictionary<string, List<string>> conflict_map = new();
        var idx = 0;
        foreach (var pluginKey in recordMap.Keys)
        {
            foreach (var record in recordMap[pluginKey])
            {
                for (var i = idx + 1; i < recordMap.Count; i++)
                {
                    var (otherPluginKey, otherRecords) = recordMap.ElementAt(i);
                    if (otherRecords.Contains(record))
                    {
                        // add to map
                        if (conflict_map.ContainsKey(record))
                        {
                            conflict_map[record].Add(otherPluginKey);
                        }
                        else
                        {
                            conflict_map.Add(record, new List<string> { pluginKey, otherPluginKey });
                        }
                    }
                }

            }
            idx++;
        }

        Conflicts = conflict_map;
    }
}
