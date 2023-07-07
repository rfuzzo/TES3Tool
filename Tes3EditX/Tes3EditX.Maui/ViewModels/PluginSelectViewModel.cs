using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tes3EditX.Maui.Extensions;
using Tes3EditX.Maui.Services;
using TES3Lib;

namespace Tes3EditX.Maui.ViewModels;

public partial class PluginSelectViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly ICompareService _compareService;
    private readonly ISettingsService _settingsService;
    private readonly IFolderPicker _folderPicker;

    [ObservableProperty]
    private List<PluginItemViewModel> _plugins;

    [ObservableProperty]
    private DirectoryInfo _folderPath;

    public PluginSelectViewModel(
        INavigationService navigationService,
        ICompareService compareService, 
        ISettingsService settingsService, 
        IFolderPicker folderPicker)
    {
        _navigationService = navigationService;
        _compareService = compareService;
        _settingsService = settingsService;
        _folderPicker = folderPicker;

        _folderPath = settingsService.GetWorkingDirectory();

        // get plugins
        InitPlugins();
    }

    private void InitPlugins()
    {


        var pluginPaths = FolderPath.EnumerateFiles("*", SearchOption.TopDirectoryOnly)
            .Where(x => 
                x.Extension.Equals(".esp", StringComparison.OrdinalIgnoreCase) || 
                x.Extension.Equals(".esm", StringComparison.OrdinalIgnoreCase));
        Plugins = pluginPaths.Select(x => new PluginItemViewModel(x)).ToList();
    }

    [RelayCommand]
    private async Task SelectFolder()
    {
        var result = await _folderPicker.PickAsync(CancellationToken.None);
        result.EnsureSuccess();

        FolderPath = new DirectoryInfo(result.Folder.Path);

        InitPlugins();
    }

    [RelayCommand]
    private async Task Compare()
    {
        _compareService.Selectedplugins = Plugins.Where(x => x.Enabled);
        _compareService.SetConflicts(); // todo make async
        // navigate away
        await _navigationService.NavigateToAsync("//Main/Main");
    }
}
