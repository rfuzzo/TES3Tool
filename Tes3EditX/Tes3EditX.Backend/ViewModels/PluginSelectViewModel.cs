
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Tes3EditX.Backend.Services;

namespace Tes3EditX.Backend.ViewModels;

public partial class PluginSelectViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly ICompareService _compareService;
    private readonly ISettingsService _settingsService;
    private readonly IFileApiService _folderPicker;

    [ObservableProperty]
    private ObservableCollection<PluginItemViewModel> _plugins;

    [ObservableProperty]
    private List<PluginItemViewModel> _selectedPlugins;

    [ObservableProperty]
    private DirectoryInfo _folderPath;

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
        Plugins = new();
        SelectedPlugins = new();
        InitPlugins();
    }

    private void InitPlugins()
    {
        IEnumerable<FileInfo> pluginPaths = FolderPath.EnumerateFiles("*", SearchOption.TopDirectoryOnly)
            .Where(x =>
                x.Extension.Equals(".esp", StringComparison.OrdinalIgnoreCase) ||
                x.Extension.Equals(".esm", StringComparison.OrdinalIgnoreCase));

        Plugins.Clear();
        foreach (var item in pluginPaths.Select(x => new PluginItemViewModel(x)))
        {
            Plugins.Add(item);
        }
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
                InitPlugins();
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
