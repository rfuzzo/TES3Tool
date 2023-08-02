#define PARALLEL

using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using Tes3EditX.Backend.Services;
using TES3Lib;
using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace Tes3EditX.Backend.ViewModels;

public partial class PluginSelectViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;
    private readonly ICompareService _compareService;
    private readonly ISettingsService _settingsService;
    private readonly IFileApiService _folderPicker;

    [ObservableProperty]
    private ObservableCollection<PluginItemViewModel> _plugins = new();

    [ObservableProperty]
    private List<PluginItemViewModel> _selectedPlugins = new();

    [ObservableProperty]
    private DirectoryInfo _folderPath;

   

    public PluginSelectViewModel(
        INavigationService navigationService,
        INotificationService notificationService,
        ICompareService compareService,
        ISettingsService settingsService,
        IFileApiService folderPicker)
    {
        _navigationService = navigationService;
        _notificationService = notificationService;
        _compareService = compareService;
        _settingsService = settingsService;
        _folderPicker = folderPicker;

        _folderPath = settingsService.GetWorkingDirectory();

        // get plugins
        //InitPlugins();
    }

    private async Task InitPlugins()
    {
        var stopwatch = Stopwatch.StartNew();

        List<FileInfo> pluginPaths = FolderPath.EnumerateFiles("*", SearchOption.TopDirectoryOnly)
            .Where(x =>
                x.Extension.Equals(".esp", StringComparison.OrdinalIgnoreCase) ||
                x.Extension.Equals(".esm", StringComparison.OrdinalIgnoreCase)).ToList();
        
        _notificationService.Progress = 0;
        _notificationService.Maximum = pluginPaths.Count;

        var plugins = new List<PluginItemViewModel>();



#if PARALLEL
        var progress = new Progress<int>(_ => _notificationService.Progress++) as IProgress<int>;

        await Task.Run(() =>
        {
            Parallel.ForEach(pluginPaths, (item, token) =>
            {
                var plugin = TES3.TES3Load(item.FullName);
                plugins.Add(new(item, plugin));
                progress.Report(0);
            });
        });
#else
         foreach (var item in pluginPaths)
         {
            var plugin = await Task.Run(() => TES3.TES3Load(item.FullName));
            plugins.Add(new(item, plugin));
            _notificationService.Progress++;
         }
#endif

        // sort by load order
        var final = plugins.OrderBy(x => x.Info.Extension.ToLower()).ThenBy(x => x.Info.LastWriteTime).ToList();
        Plugins = new(final);

        stopwatch.Stop();
        _notificationService.Text = stopwatch.Elapsed.TotalSeconds.ToString();
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
        await _compareService.CalculateConflicts(); // todo make async
        // navigate away
        await _navigationService.NavigateToAsync("//Main/Main");
    }
}
