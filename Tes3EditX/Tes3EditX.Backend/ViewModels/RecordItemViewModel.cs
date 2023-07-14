using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes3EditX.Backend.ViewModels;

public class RecordItemViewModel
{
    public RecordItemViewModel(string name, List<string> plugins)
    {
        Name = name;
        Plugins = plugins;
    }

    public string Name { get; set; }
    public List<string> Plugins { get; set; }   
}
