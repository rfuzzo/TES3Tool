using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tes3EditX.Backend.ViewModels;

namespace Tes3EditX.Backend.Services;

public interface ICompareService
{
    public Dictionary<string, List<string>> Conflicts { get; set; }
    public IEnumerable<PluginItemViewModel> Selectedplugins { get; set; }

    void CalculateConflicts();
}
