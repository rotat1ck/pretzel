using CommunityToolkit.Mvvm.ComponentModel;
using Pretzel.Core.Models.Search;

namespace Pretzel.App.ViewModels;

public partial class SearchSessionViewModel : BaseViewModel
{
    public ChartSearchOptions BasicSearchOptions { get; set; } = new();

    [ObservableProperty]
    public partial string? Search { get; set; }


    public ChartSearchAdvancedOptions AdvancedSearchOptions { get; set; } = new();

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

    public void ComposeSearchOptions()
    {
        BasicSearchOptions.Page = 1;
        BasicSearchOptions.Search = Search;

        AdvancedSearchOptions.Page = 1;
        AdvancedSearchOptions.Name = Name;
        AdvancedSearchOptions.Artist = Artist;
        AdvancedSearchOptions.Charter = Charter;
        AdvancedSearchOptions.Album = Album;
        AdvancedSearchOptions.Genre = Genre;
        AdvancedSearchOptions.Year = Year;
    }
}
