using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Chart;
using Pretzel.Core.Models.Search;
using System.Collections.ObjectModel;

namespace Pretzel.App.ViewModels;

public partial class ExploreViewModel(IChartSearchService searchService,
                                      IChartDownloadService downloadService,
                                      SearchSessionViewModel searchSession) : BaseViewModel
{
    private readonly IChartSearchService searchService = searchService;
    private readonly IChartDownloadService downloadService = downloadService;

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
        if (IsSearching)
        {
            return;
        }

        SearchSession.ComposeSearchOptions();
        ResetSearch();

        cts.Cancel();
        cts.Dispose();
        cts = new();

        // shallow copying for new page functionallity
        currentOptions = IsAdvancedSearch ? SearchSession.AdvancedSearchOptions with { } : SearchSession.BasicSearchOptions with { };
        await SearchAsync(currentOptions);
    }

    [RelayCommand]
    public async Task SearchNewPage()
    {
        if (IsSearching || isAllReturned || currentOptions is null)
        {
            return;
        }

        currentOptions.Page++;
        await SearchAsync(currentOptions);
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    public async Task DownloadChart((Chart chartInfo, ChartDownloadSource chartDownloadSource) parameter)
    {
        var existingDownload = downloadService.GetDownloadResult(parameter.chartDownloadSource);
        if (existingDownload is not null)
        {
            await downloadService.RemoveDownloadAsync(parameter.chartDownloadSource);
        } 
        else
        {
            await downloadService.DownloadAsync(parameter.chartDownloadSource, parameter.chartInfo);
        }

    }

    [RelayCommand]
    public void Cancel()
    {
        cts.Cancel();
    }

    private async Task SearchAsync(ChartSearchOptions options)
    {
        IsSearching = true;

        try
        {
            await foreach (var sourceResult in searchService.SearchAsync(options, cts.Token))
            {
                if (sourceResult.ProblemDetails is not null)
                {
                    continue;
                }

                SourcesResults.Add(sourceResult);

                Count = CalculateTotalCount();
                Returned += sourceResult.Items.Count();
                foreach (var chart in sourceResult.Items)
                {
                    DisplayCharts.Add(chart);
                }
            }

            if (Returned >= Count)
            {
                isAllReturned = true;
            }
        }
        finally
        {
            IsSearching = false;
        }
    }

    private void ResetSearch()
    {
        Count = 0;
        Returned = 0;
        DisplayCharts.Clear();
        SourcesResults.Clear();
        isAllReturned = false;
    }

    private int CalculateTotalCount()
    {
        return SourcesResults.DistinctBy(sourceResult => sourceResult.ChartSource).Sum(sourceResult => sourceResult.Count);
    }

    private CancellationTokenSource cts = new();
    private ChartSearchOptions? currentOptions;
    private bool isAllReturned = false;

    private List<ChartSearchResults> SourcesResults { get; set; } = new();

    [ObservableProperty]
    public partial ObservableCollection<Chart> DisplayCharts { get; set; } = new();

    [ObservableProperty]
    public partial int? Count { get; set; }

    [ObservableProperty]
    public partial int? Returned { get; set; }

    [ObservableProperty]
    public partial bool IsAdvancedSearch { get; set; } = false;

    [ObservableProperty]
    public partial bool IsSearching { get; set; } = false;

}
