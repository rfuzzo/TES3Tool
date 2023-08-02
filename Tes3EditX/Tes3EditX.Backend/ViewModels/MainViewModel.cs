using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tes3EditX.Backend.Extensions;
using Tes3EditX.Backend.Models;
using Tes3EditX.Backend.Services;
using TES3Lib.Base;

namespace Tes3EditX.Backend.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly ICompareService _compareService;
    private readonly ISettingsService _settingsService;


    // Record Selet View
    private readonly List<RecordItemViewModel> _records = new();
    private List<ConflictItemViewModel> _conflicts = new();

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
    private ObservableCollection<ConflictRecordFieldViewModel> _fields = new();

    public MainViewModel(
        INavigationService navigationService,
        ICompareService compareService,
        ISettingsService settingsService)
    {
        _navigationService = navigationService;
        _compareService = compareService;
        _settingsService = settingsService;

        // init
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
                if (!string.IsNullOrEmpty(SelectedTag) && SelectedTag != "_")
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

    // TODO refactor this shit

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

        Fields.Clear();

        //Conflicts.Clear();
        var Conflicts = new List<ConflictItemViewModel>();
        var Names = new List<string>();

        // loop through plugins to get a vm for each plugin
        List<object> plugins = new();
        foreach (var pluginPath in recordViewModel.Plugins)
        {
            // get plugin
            if (_compareService.Plugins.TryGetValue(pluginPath, out var plugin))
            {
                // recordViewModel.Plugins.Select(p => p.Name).Cast<object>().ToList(); 
                plugins.Add(pluginPath.Name);

                // get record
                var record = plugin.Records.FirstOrDefault(x => x is not null && x.GetUniqueId() == recordId);
                if (record != null)
                {
                    if (!Names.Any())
                    {
                        var type = record.GetType();
                        var obj = (Record?)Activator.CreateInstance(type!);
                        Names = new(obj!.GetPropertyNames());
                    }

                    var item = new ConflictItemViewModel(pluginPath, record, Names.ToList());
                    Conflicts.Add(item);
                }
                else
                {
                    // 
                }
            } 
        }

        // loop again to get field equality
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


        // -----------------------------------------

        Fields.Clear();

           
        // hack
        Fields.Add(new("Plugins", plugins));

        foreach (var name in Names)
        {
            List<object> list = new();

            foreach (var c in Conflicts)
            {
                var f = c.Fields.FirstOrDefault(x => x.Name.Equals(name));
                if (f is not null)
                {
                    list.Add(f);
                }
            }

            Fields.Add(new ConflictRecordFieldViewModel(name, list));

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
