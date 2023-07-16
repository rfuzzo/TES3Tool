using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using Tes3EditX.Backend.Models;
using Tes3EditX.Backend.Services;

namespace Tes3EditX.Backend.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly ICompareService _compareService;
    private readonly ISettingsService _settingsService;

    private readonly List<RecordItemViewModel> _records = new();

    [ObservableProperty]
    private ObservableCollection<GroupInfoList> _groupedRecords;

    [ObservableProperty]
    private object _selectedRecord = null;

    [ObservableProperty]
    private ObservableCollection<ConflictItemViewModel> _conflicts;

    [ObservableProperty]
    private string _filterName;

    [ObservableProperty]
    private ObservableCollection<string> _tags;

    [ObservableProperty]
    private string _selectedTag = "";

    public MainViewModel(
        INavigationService navigationService,
        ICompareService compareService,
        ISettingsService settingsService)
    {
        _navigationService = navigationService;
        _compareService = compareService;
        _settingsService = settingsService;

        // init
        FilterName = "";
        Conflicts = new();
        GroupedRecords = new();
        Tags = new();

        RegenerateRecords(_compareService.Conflicts);

        // Register a message in some module
        WeakReferenceMessenger.Default.Register<ConflictsChangedMessage>(this, (r, m) =>
        {
            // Handle the message here, with r being the recipient and m being the
            // input message. Using the recipient passed as input makes it so that
            // the lambda expression doesn't capture "this", improving performance.
            if (r is MainViewModel vm)
            {
                vm.RegenerateRecords(m.Value);
            }
        });
    }

    public void RegenerateRecords(Dictionary<string, List<string>> conflicts)
    {
        _records.Clear();
        foreach ((var id, List<string> plugins) in conflicts)
        {
            var tag = id.Split(',').First();
            var name = id.Split(',').Last();
            _records.Add(new RecordItemViewModel(name, plugins, tag));
        }

        FilterRecords("");
    }

    public void FilterRecords(string filter)
    {
        var query = _records
            .Where( x =>
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    return x.Name.Contains(filter, StringComparison.CurrentCultureIgnoreCase);
                }
                else
                {
                    return true;
                }
            })
            .GroupBy(x => x.Tag)
            .OrderBy(x => x.Key)
            .Select(g => new GroupInfoList(g) { Key = g.Key });

        GroupedRecords = new ObservableCollection<GroupInfoList>(query);

        // regenerate tags
        Tags = new ObservableCollection<string>(query.Select(x => x.Key.ToString()!));
    }

    partial void OnSelectedRecordChanged(object value)
    {
        if (value is RecordItemViewModel vm)
        {
            // todo refactor?
            Conflicts.Clear();
            foreach (var item in vm.Plugins.Select(x => new ConflictItemViewModel(x)))
            {
                Conflicts.Add(item);
            }
        }
    }

    partial void OnFilterNameChanged(string value)
    {
        FilterRecords(value);
    }

    [RelayCommand]
    private void PerformSearch(string value)
    {
        FilterRecords(value);
    }


}
