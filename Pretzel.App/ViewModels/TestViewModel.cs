using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Models;
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
        _ = Test();
    }

    [ObservableProperty]
    private List<Chart> charts;

    public async Task Test()
    {
        //var searchOptions = new ChartSearchAdvancedOptions
        //{
        //    Charter = "3-UP",
        //    Artist = "Jamie Paige",
        //    Page = 1
        //};

        //var searchOptions = new ChartSearchOptions
        //{
        //    Search = "Jamie Paige",
        //    Page = 1
        //};

        //var charts = await chorusStrategy.SearchAsync(searchOptions);
        //var chartsRhythm = await rhythmStrategy.SearchAsync(searchOptions);
        //if (charts is not null && chartsRhythm is not null)
        //{
        //    charts.Count += chartsRhythm.Count;
        //    charts.Returned += chartsRhythm.Returned;
        //}

        await searchService.SearchAsync(new ChartSearchOptions(), default);
    }
}
