using CommunityToolkit.Mvvm.ComponentModel;
using Pretzel.Core.Models.Search;

namespace Pretzel.App.ViewModels;

public partial class SearchSessionViewModel : BaseViewModel
{
    [ObservableProperty]
    public partial List<ChartSearchResults>? SourcesResults { get; set; }


    public ChartSearchOptions BasicSearchOptions { get; set; } = new();

    [ObservableProperty]
    public partial string? Search { get; set; }


    private ChartSearchAdvancedOptions AdvancedSearchOptions { get; set; } = new();

    [ObservableProperty]
    public partial string? Name { get; set; }

    [ObservableProperty]
    public partial string? Artist { get; set; }

    [ObservableProperty]
    public partial string? Charter { get; set; }

    [ObservableProperty]
    public partial string? Album { get; set; }

    [ObservableProperty]
    public partial string? Genre { get; set; }

    [ObservableProperty]
    public partial int? Year { get; set; }
}
