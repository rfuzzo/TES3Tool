using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes3EditX.Backend.ViewModels;

/// <summary>
/// This viewmodel wraps a TES3 record's fileinfo
/// </summary>
public partial class PluginItemViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _enabled;

    [ObservableProperty]
    private FileInfo _info;

    public string Name => Info.Name;

    public PluginItemViewModel(FileInfo item)
    {
        _info = item;
    }
}
