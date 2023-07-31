using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using Tes3EditX.Backend.Extensions;
using Tes3EditX.Backend.Models;
using Tes3EditX.Backend.Services;

namespace Tes3EditX.Backend.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly ICompareService _compareService;
    private readonly ISettingsService _settingsService;


    // Record Selet View
    private readonly List<RecordItemViewModel> _records = new();

    [ObservableProperty]
    private ObservableCollection<GroupInfoList> _groupedRecords;

    [ObservableProperty]
    private object? _selectedRecord = null;

    public string FilterName { get; set; } = "";

    [ObservableProperty]
    private ObservableCollection<string> _tags;

    [ObservableProperty]
    private string _selectedTag = "";

    // Conflicts view
    [ObservableProperty]
    private ObservableCollection<ConflictItemViewModel> _conflicts;

    [ObservableProperty]
    private List<string> _currentFieldNames = new();

    public MainViewModel(
        INavigationService navigationService,
        ICompareService compareService,
        ISettingsService settingsService)
    {
        _navigationService = navigationService;
        _compareService = compareService;
        _settingsService = settingsService;

        // init
        Conflicts = new();
        GroupedRecords = new();
        Tags = new ObservableCollection<string>(Tes3Extensions.GetAllTags().Order());

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

    public void RegenerateRecords(Dictionary<string, List<FileInfo>> conflicts)
    {
        _records.Clear();
        foreach ((var id, List<FileInfo> plugins) in conflicts)
        {
            var tag = id.Split(',').First();
            var name = id.Split(',').Last();
            _records.Add(new RecordItemViewModel(tag, name, plugins));
        }

        FilterRecords();
    }

    public void FilterRecords()
    {
        IEnumerable<GroupInfoList> query = _records
            .Where(x =>
            {
                if (!string.IsNullOrEmpty(SelectedTag) || SelectedTag == "_")
                {
                    return x.Tag.Equals(SelectedTag, StringComparison.CurrentCultureIgnoreCase);
                }
                else
                {
                    return true;
                }
            })
            .Where(x =>
            {
                if (!string.IsNullOrEmpty(FilterName))
                {
                    return x.Name.Contains(FilterName, StringComparison.CurrentCultureIgnoreCase);
                }
                else
                {
                    return true;
                }
            })
            .GroupBy(x => x.Tag)
            .OrderBy(x => x.Key)
            .Select(g => new GroupInfoList(g, g.Key));

        GroupedRecords = new ObservableCollection<GroupInfoList>(query);


    }

    /// <summary>
    /// Populate conflicts view
    /// </summary>
    /// <param name="value"></param>
    partial void OnSelectedRecordChanged(object? value)
    {
        if (value is not RecordItemViewModel recordViewModel)
        {
            return;
        }

        var recordId = recordViewModel.GetUniqueId();

        Conflicts.Clear();
        foreach (var pluginPath in recordViewModel.Plugins)
        {
            // get plugin
            if (_compareService.Plugins.TryGetValue(pluginPath, out var plugin))
            {   
                // get record
                var record = plugin.Records.First(x => x is not null && x.GetUniqueId() == recordId);
                if (record != null)
                {
                    var item = new ConflictItemViewModel(pluginPath, record);
                    Conflicts.Add(item);
                }
            } 
        }

        // get field equality
        var anyConflict = false;
        for (var i = 1; i < Conflicts.Count; i++)
        {
            ConflictItemViewModel c = Conflicts[i];
            ConflictItemViewModel c_last = Conflicts[i-1];

            for (var j = 0; j < c.Fields.Count; j++)
            {
                RecordFieldViewModel f = c.Fields[j];
                if (j > c_last.Fields.Count)
                {
                    f.IsConflict = true;
                    anyConflict = true;
                }
                else
                {
                    RecordFieldViewModel f_last = c_last.Fields[j];
                    if (f_last.WrappedField is not null && f.WrappedField is not null)
                    {
                        if (!f_last.WrappedField.Equals(f.WrappedField))
                        {
                            f.IsConflict = true;
                            anyConflict = true;
                        }
                    }
                    else if (f_last.WrappedField is null && f.WrappedField is null)
                    {
                        // do nothing
                    }
                    else
                    {
                        f.IsConflict = true;
                        anyConflict = true;
                    }
                    
                }
               

            }
        }

        // if no visible conflicts found, do not display it
        if (!anyConflict)
        {
            // do not display this record

        }

    }

    partial void OnSelectedTagChanged(string value)
    {
        FilterRecords();
    }

    [RelayCommand]
    private void PerformSearch(string value)
    {
        FilterRecords();
    }


}
