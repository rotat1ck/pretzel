using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Chart;
using Pretzel.Core.Models.Download;

namespace Pretzel.App.ViewModels;

public partial class ChartViewModel(IChartDownloadService downloadService) : BaseViewModel
{
    [ObservableProperty]
    public partial Chart Chart { get; set; }

    [ObservableProperty]
    public partial ChartDownload? ChartDownload { get; set; }

    [ObservableProperty]
    public partial ChartDownloadSource SelectedDownloadSource { get; set; }

    [RelayCommand]
    public async Task DownloadChart((Chart chartInfo, ChartDownloadSource chartDownloadSource) parameter)
    {
        ChartDownload = downloadService.GetChartDownload(parameter.chartDownloadSource);
        if (ChartDownload is not null)
        {
            await downloadService.RemoveDownloadAsync(parameter.chartDownloadSource);
        }

        // unsubscribe from ChartDownload.OnStatusChanged

        downloadService.OnChartDownloadAdded += OnChartDownloadAdded;

        await downloadService.DownloadAsync(parameter.chartDownloadSource, parameter.chartInfo);
    }

    private void OnChartDownloadAdded(object? sender, ChartDownloadAddedArgs e)
    {
        if (e.ChartDownloadSource == SelectedDownloadSource)
        {
            downloadService.OnChartDownloadAdded -= OnChartDownloadAdded;
        }
    }
}
