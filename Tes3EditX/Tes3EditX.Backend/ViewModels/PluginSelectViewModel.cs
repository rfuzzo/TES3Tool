
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Tes3EditX.Backend.Services;
using TES3Lib;

namespace Tes3EditX.Backend.ViewModels;

public partial class PluginSelectViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly ICompareService _compareService;
    private readonly ISettingsService _settingsService;
    private readonly IFileApiService _folderPicker;

    [ObservableProperty]
    private ObservableCollection<PluginItemViewModel> _plugins = new();

    [ObservableProperty]
    private List<PluginItemViewModel> _selectedPlugins = new();

    [ObservableProperty]
    private DirectoryInfo _folderPath;

    [ObservableProperty]
    private int _progress = 0;

    [ObservableProperty]
    private int _maximum = 0;

    public PluginSelectViewModel(
        INavigationService navigationService,
        ICompareService compareService,
        ISettingsService settingsService,
        IFileApiService folderPicker)
    {
        _navigationService = navigationService;
        _compareService = compareService;
        _settingsService = settingsService;
        _folderPicker = folderPicker;

        _folderPath = settingsService.GetWorkingDirectory();

        // get plugins
        //InitPlugins();
    }

    private async Task InitPlugins()
    {
        Progress = 0;
        

        List<FileInfo> pluginPaths = FolderPath.EnumerateFiles("*", SearchOption.TopDirectoryOnly)
            .Where(x =>
                x.Extension.Equals(".esp", StringComparison.OrdinalIgnoreCase) ||
                x.Extension.Equals(".esm", StringComparison.OrdinalIgnoreCase)).ToList();
        
        
        Maximum = pluginPaths.Count;

        var plugins = new List<PluginItemViewModel>();
        foreach (var item in pluginPaths)
        {
            var plugin = await Task.Run(() => TES3.TES3Load(item.FullName));
            plugins.Add(new(item, plugin));
            
            Progress += 1;
        }

        // sort by load order
        var final = plugins.OrderBy(x => x.Info.Extension.ToLower()).ThenBy(x => x.Info.LastWriteTime).ToList();
        Plugins = new(final);
    }

    [RelayCommand]
    private async Task SelectFolder()
    {
        var result = await _folderPicker.PickAsync(CancellationToken.None);

        if (!string.IsNullOrEmpty(result))
        {
            FolderPath = new DirectoryInfo(result);

            if (FolderPath.Exists)
            {
               await InitPlugins();
            }
        }
    }

    [RelayCommand]
    private async Task Compare()
    {
        _compareService.Selectedplugins = SelectedPlugins;
        _compareService.CalculateConflicts(); // todo make async
        // navigate away
        await _navigationService.NavigateToAsync("//Main/Main");
    }
}
