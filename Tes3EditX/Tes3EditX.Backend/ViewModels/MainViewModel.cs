using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tes3EditX.Backend.Services;
using Tes3EditX.Maui.Extensions;
using TES3Lib;

namespace Tes3EditX.Backend.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly ICompareService _compareService;
    private readonly ISettingsService _settingsService;

    [ObservableProperty]
    private List<RecordItemViewModel> _records;

    [ObservableProperty]
    private RecordItemViewModel _selectedRecord;

    [ObservableProperty]
    private List<ConflictItemViewModel> _conflicts;

    public MainViewModel(
        INavigationService navigationService,
        ICompareService compareService,
        ISettingsService settingsService)
    {
        _navigationService = navigationService;
        _compareService = compareService;
        _settingsService = settingsService;

        // init
        Conflicts = new List<ConflictItemViewModel>();

        RegenerateRecords(compareService);

    }

    private void RegenerateRecords(ICompareService compareService, string query = "")
    {
        var records = new List<RecordItemViewModel>();
        foreach (var (id, plugins) in compareService.Conflicts)
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
        Records = records;
    }

    partial void OnSelectedRecordChanged(RecordItemViewModel value)
    {
        if (value is null)
        {
            return;
        }
        // todo refactor?
        Conflicts = value.Plugins.Select(x => new ConflictItemViewModel(x)).ToList();
    }

    [RelayCommand]
    private void PerformSearch(string query)
    {
        RegenerateRecords(_compareService, query);
    }
}
