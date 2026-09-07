using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Interfaces.Download;
using Pretzel.Core.Models.Chart;
using Pretzel.Core.Models.Download;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Pretzel.Infrastructure.Services.Download;

public class ChartWriterService(ISettingProvider<ChartDownloadSettings> downloadSettingsProvider) : IChartWriterService
{
    private readonly ISettingProvider<ChartDownloadSettings> downloadSettingsProvider = downloadSettingsProvider;
    private static readonly char[] illegalCharacters = Path.GetInvalidFileNameChars();

    public async Task<DownloadStatus> WriteChartAsync(DownloadResult downloadResult, Chart chartInfo, CancellationToken cancellationToken)
    {
        var settings = downloadSettingsProvider.Value;
        var selectedDirectoryIndex = settings.SelectedDirectory;

        if (selectedDirectoryIndex is null || selectedDirectoryIndex < 0)
        {
            /*
             * subject to future refactoring
             * declare a custom base exception and derive from it
             * for each logic component - ChartDownloadException
             * containing an enum instead of exception message
            */
            throw new InvalidOperationException("Select a folder before downloading");
        }

        if (selectedDirectoryIndex >= settings.AvailableDirectories.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(selectedDirectoryIndex), "Selected directory index is out of range.");
        }

        var selectedDirectory = settings.AvailableDirectories[(int)selectedDirectoryIndex];

        if (!Directory.Exists(selectedDirectory))
        {
            Directory.CreateDirectory(selectedDirectory);
        }

        if (downloadResult.Stream is null)
        {
            throw new InvalidOperationException($"Chart stream is null");
        }

        var filePath = Path.Combine(selectedDirectory, ComposeFileName(settings.FileNamePattern, chartInfo));
        try
        {
            using FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await downloadResult.Stream.CopyToAsync(fs, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            RemoveFile(filePath);
            return DownloadStatus.Cancelled;
        } 
        catch
        {
            RemoveFile(filePath);
            throw;
        }

        return DownloadStatus.Finished;
    }

    private void RemoveFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }        
    }

    private string ComposeFileName(string pattern, Chart chartInfo)
    {
        return Regex.Replace(pattern, @"\{(.+?)\}", match =>
        {
            string propertyName = match.Groups[1].Value;
            var property = chartInfo.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                ?? throw new InvalidOperationException($"{propertyName} is invalid in FileNamePattern");

            var value = property.GetValue(chartInfo)
                ?? throw new NullReferenceException();

            return string.Join('_', value.ToString()!.Split(illegalCharacters));
        });
    }


}
