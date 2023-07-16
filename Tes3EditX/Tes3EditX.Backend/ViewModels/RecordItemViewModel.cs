using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes3EditX.Backend.ViewModels;

public class RecordItemViewModel
{
    public RecordItemViewModel(string name, List<string> plugins, string tag)
    {
        Name = name;
        Plugins = plugins;
        Tag = tag;
    }

    public string Tag { get; set; }
    public string Name { get; set; }
    public List<string> Plugins { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
