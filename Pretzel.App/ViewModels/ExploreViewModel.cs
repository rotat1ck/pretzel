using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Models;
using Pretzel.Core.Models.Search;

namespace Pretzel.App.ViewModels;

public partial class ExploreViewModel(IChartSearchService searchService) : BaseViewModel
{
    private readonly IChartSearchService searchService = searchService;

    [RelayCommand]
    public void ToggleAdvanced()
    {
        IsAdvancedSearch = !IsAdvancedSearch;
    }

    [RelayCommand]
    public async Task Search()
    {

    }

    [ObservableProperty]
    private List<Chart> displayCharts = new();

    [ObservableProperty]
    private int? count;

    [ObservableProperty]
    private int? returned;

    [ObservableProperty]
    private bool isAdvancedSearch = false;

    [ObservableProperty]
    private bool isSearching = false;

    // maybe later be combined into a ChartSearchSession,
    // if not needed to bind directly to these properties
    private List<ChartSearchResults> SourcesResults { get; set; }

    private ChartSearchOptions BasicSearchOptions { get; set; }
    private ChartSearchAdvancedOptions AdvancedSearchOptions { get; set; }
}
