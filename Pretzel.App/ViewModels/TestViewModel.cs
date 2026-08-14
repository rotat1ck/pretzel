using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Models;
using Pretzel.Core.Models.Search;

namespace Pretzel.App.ViewModels;

public partial class TestViewModel : ObservableObject
{
    private readonly IMapper mapper;
    private readonly IChartSearchStrategy chorusStrategy;
    private readonly IChartSearchStrategy rhythmStrategy;

    public TestViewModel(IMapper mapper, [FromKeyedServices(ChartSource.ChorusEncore)] IChartSearchStrategy chorusStrategy,
        [FromKeyedServices(ChartSource.RhythmVerse)] IChartSearchStrategy rhythmStrategy)
    {
        this.mapper = mapper;
        this.chorusStrategy = chorusStrategy;
        this.rhythmStrategy = rhythmStrategy;
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

        var searchOptions = new ChartSearchAdvancedOptions
        {
            Year = 2025,
            Name = "BIRDBRAIN",
            Page = 1
        };

        var charts = await chorusStrategy.SearchAsync(searchOptions) as List<Chart>;
        var chartsRhythm = await rhythmStrategy.SearchAsync(searchOptions) as List<Chart>;
        if (charts is not null)
        {
            Charts = charts;

            if (chartsRhythm is not null)
            {
                charts.AddRange(chartsRhythm);
            }
        }
    }
}
