using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Chart;
using Pretzel.Core.Models.Download;

namespace Pretzel.App.ViewModels;

public partial class ChartViewModel(IChartDownloadService downloadService) : BaseViewModel, IDisposable
{
    [ObservableProperty]
    public partial Chart Chart { get; set; }

    [ObservableProperty]
    public partial ChartDownloadSource SelectedDownloadSource { get; set; }

    [ObservableProperty]
    public partial string? ChartDownloadStatus { get; set; }

    private ChartDownload? chartDownload;

    public void Dispose()
    {
        downloadService.OnChartDownloadAdded -= OnChartDownloadAdded;
        chartDownload?.OnStatusChanged -= OnChartDownloadStatusChanged;
    }

    [RelayCommand]
    public async Task DownloadChart((Chart chartInfo, ChartDownloadSource chartDownloadSource) parameter)
    {
        chartDownload?.OnStatusChanged -= OnChartDownloadStatusChanged;

        var existing = downloadService.GetChartDownload(parameter.chartDownloadSource);
        if (existing is not null && existing.Status is DownloadStatus.Finished or DownloadStatus.Failed or DownloadStatus.Cancelled)
        {
            await downloadService.RemoveDownloadAsync(parameter.chartDownloadSource);
        }

        downloadService.OnChartDownloadAdded += OnChartDownloadAdded;

        await downloadService.DownloadAsync(parameter.chartDownloadSource, parameter.chartInfo);
    }

    private void OnChartDownloadAdded(object? sender, ChartDownloadAddedArgs e)
    {
        if (Chart.DownloadSources.Contains(e.ChartDownloadSource))
        {
            downloadService.OnChartDownloadAdded -= OnChartDownloadAdded;

            chartDownload = e.ChartDownload;
            chartDownload.OnStatusChanged += OnChartDownloadStatusChanged;
            ChartDownloadStatus = chartDownload.Status.ToString();
        }
    }

    private void OnChartDownloadStatusChanged(object? sender, DownloadStatus status)
    {
        if (status.ToString() != ChartDownloadStatus)
        {
            ChartDownloadStatus = status.ToString();
        }
    }
}
