using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Chart;

namespace Pretzel.App.ViewModels;

public partial class ChartViewModel(IChartDownloadService downloadService) : BaseViewModel
{
    [ObservableProperty]
    public partial Chart Chart { get; set; }

    [RelayCommand]
    public async Task DownloadChart((Chart chartInfo, ChartDownloadSource chartDownloadSource) parameter)
    {
        var existingDownload = downloadService.GetChartDownload(parameter.chartDownloadSource);
        if (existingDownload is not null)
        {
            await downloadService.RemoveDownloadAsync(parameter.chartDownloadSource);
        }

        await downloadService.DownloadAsync(parameter.chartDownloadSource, parameter.chartInfo);
    }
}
