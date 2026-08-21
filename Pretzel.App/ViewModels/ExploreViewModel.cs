using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Models;

namespace Pretzel.App.ViewModels;

public partial class ExploreViewModel(IChartSearchService searchService, SearchSessionViewModel searchSession) : BaseViewModel
{
    private readonly IChartSearchService searchService = searchService;

    [ObservableProperty]
    private SearchSessionViewModel searchSession = searchSession;

    [RelayCommand]
    public void ToggleAdvanced()
    {
        IsAdvancedSearch = !IsAdvancedSearch;
    }

    [RelayCommand]
    public async Task Search()
    {
        cts.Dispose();
        cts = new();

    }

    [RelayCommand]
    public void Cancel()
    {
        cts.Cancel();
    }

    private CancellationTokenSource cts = new();

    [ObservableProperty]
    public partial List<Chart> DisplayCharts { get; set; } = new();

    [ObservableProperty]
    public partial int? Count { get; set; }

    [ObservableProperty]
    public partial int? Returned { get; set; }

    [ObservableProperty]
    public partial bool IsAdvancedSearch { get; set; } = false;

    [ObservableProperty]
    public partial bool IsSearching { get; set; } = false;

}
