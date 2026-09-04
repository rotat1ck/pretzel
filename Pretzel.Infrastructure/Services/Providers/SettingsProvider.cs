using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Models.Search;

namespace Pretzel.Infrastructure.Services.Providers;

public class SettingsProvider<T> : ISettingProvider<T> where T : class, ICloneable, new()
{
    private readonly SemaphoreSlim semaphore = new(1, 1);
    private readonly JsonSerializerOptions jsonOptions = new() 
    {
        WriteIndented = true, 
        Converters = { new JsonStringEnumConverter() } 
    };

    private volatile T settings = null!;
    private string filePath;

    public SettingsProvider(string baseDirectory, string fileName)
    {
        this.filePath = Path.Combine(baseDirectory, fileName);
        _ = Load();
    }

    public T Value => settings;

    public async Task<T> UpdateValueAsync(Action<T> updateAction)
    {
        await semaphore.WaitAsync();
        try
        {
            var newSettings = (T)settings.Clone();

            updateAction(newSettings);

            await WriteWithRetryAsync(newSettings);

            settings = newSettings;
            return settings;
        }
        finally
        {
            semaphore.Release();
        }
    }

    private async Task Load()
    {
        await semaphore.WaitAsync();
        try
        {
            if (File.Exists(filePath))
            {
                var json = await File.ReadAllTextAsync(filePath);
                settings = JsonSerializer.Deserialize<T>(json, jsonOptions) ?? new();
            }
            else
            {
                settings = new();
                await WriteWithRetryAsync(settings);
            }
        }
        catch (Exception)
        {
            File.Delete(filePath);
            settings = new();
            await WriteWithRetryAsync(settings);
        }
        finally
        {
            semaphore.Release();
        }
    }

    private async Task WriteWithRetryAsync(T value)
    {
        const int maxRetries = 3;
        var json = JsonSerializer.Serialize(value, jsonOptions);

        for (int retry = 1; retry <= maxRetries; retry++)
        {
            try
            {
                await File.WriteAllTextAsync(filePath, json);
                return;
            }
            catch
            {
                if (retry == maxRetries)
                {
                    throw;
                }
            }
        }
    }
}
