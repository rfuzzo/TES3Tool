using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes3EditX.Backend.ViewModels;

public class ConflictItemViewModel
{
    private string _path;
    public ConflictItemViewModel(string path)
    {
        _path = path;

        Name = Path.GetFileName(path);
    }

    public string Name { get; }
}
