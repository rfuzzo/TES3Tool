using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tes3EditX.Backend.Models;
using Tes3EditX.Backend.Services;
using Tes3EditX.Maui.Extensions;
using TES3Lib;

namespace Tes3EditX.Backend.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly ICompareService _compareService;
    private readonly ISettingsService _settingsService;

    [ObservableProperty]
    private ObservableCollection<RecordItemViewModel> _records;

    [ObservableProperty]
    private object _selectedRecord;

    [ObservableProperty]
    private ObservableCollection<ConflictItemViewModel> _conflicts;

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
        Records = new();

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

    public void RegenerateRecords(Dictionary<string, List<string>> conflicts, string query = "")
    {
        var records = new List<RecordItemViewModel>();
        foreach (var (id, plugins) in conflicts)
        {
            if (!string.IsNullOrEmpty(query))
            {
                if (id.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    records.Add(new RecordItemViewModel(id, plugins));
                }
            }
            else
            {
                records.Add(new RecordItemViewModel(id, plugins));
            }
        }

        Records.Clear();
        foreach (var item in records)
        {
            Records.Add(item);
        }
        
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

    [RelayCommand]
    private void PerformSearch(string query)
    {
        RegenerateRecords(_compareService.Conflicts, query);
    }
}
