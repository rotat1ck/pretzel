using Pretzel.Core.Interfaces;
using Pretzel.Core.Models.Search;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pretzel.Infrastructure.Services.Search;

public class ChartSearchSettingsProvider : ISettingProvider<ChartSearchSettings>
{
    private readonly SemaphoreSlim semaphore = new(1, 1);
    private readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };

    private volatile ChartSearchSettings settings = null!;
    private string filePath;

    public ChartSearchSettingsProvider(string baseDirectory)
    {
        this.filePath = Path.Combine(baseDirectory, "search_settings.json");
        Load();
    }

    public ChartSearchSettings GetValue() => settings;

    public async Task<ChartSearchSettings> UpdateValueAsync(Action<ChartSearchSettings> updateAction)
    {
        await semaphore.WaitAsync();
        try
        {
            var newSettings = new ChartSearchSettings(settings);

            updateAction(newSettings);

            var json = JsonSerializer.Serialize(newSettings, jsonOptions);
            await File.WriteAllTextAsync(filePath, json);

            settings = newSettings;
            return settings;
        }
        finally
        {
            semaphore.Release();
        }
    }

    private void Load()
    {
        semaphore.Wait();
        try
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                settings = JsonSerializer.Deserialize<ChartSearchSettings>(json, jsonOptions) ?? new();
            }
            else
            {
                settings = new();
                var json = JsonSerializer.Serialize(settings, jsonOptions);
                File.WriteAllText(filePath, json);
            }
        }
        catch (JsonException)
        {
            File.Delete(filePath);
            settings = new();
            var json = JsonSerializer.Serialize(settings, jsonOptions);
            File.WriteAllText(filePath, json);
        }
        finally
        {
            semaphore.Release();
        }
    }
}
