using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Models.Search;

namespace Pretzel.App.ViewModels;

public partial class TestViewModel : ObservableObject
{
    private readonly IMapper mapper;
    private readonly IChartSearchService searchService;

    public TestViewModel(IMapper mapper, IChartSearchService searchService)
    {
        this.mapper = mapper;
        this.searchService = searchService;
    }

    [ObservableProperty]
    private List<ChartSearchResults> charts = new();

    [RelayCommand]
    public async Task Test()
    {
        //var searchOptions = new ChartSearchAdvancedOptions
        //{
        //    Charter = "3-UP",
        //    Artist = "Jamie Paige",
        //    Page = 1
        //};

        var searchOptions = new ChartSearchOptions
        {
            Search = "Jamie Paige",
            Page = 1
        };

        //var charts = await chorusStrategy.SearchAsync(searchOptions);
        //var chartsRhythm = await rhythmStrategy.SearchAsync(searchOptions);
        //if (charts is not null && chartsRhythm is not null)
        //{
        //    charts.Count += chartsRhythm.Count;
        //    charts.Returned += chartsRhythm.Returned;
        //}

        await foreach (var result in searchService.SearchAsync(searchOptions, new()))
        {
            if (result.ProblemDetails is null)
            {
                Charts.Add(result);
            }
        }
    }
}
