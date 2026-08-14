using AutoMapper;
using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Models;
using Pretzel.Core.Models.Search;
using Pretzel.Infrastructure.DTOs.Search;
using Pretzel.Infrastructure.DTOs.Search.RhythmVerse;
using System.Net.Http.Json;
using System.Text.Json;

namespace Pretzel.Infrastructure.Services.Search;

public class RhythmVerseSearchStrategy(IHttpClientFactory clientFactory, IMapper mapper) : IChartSearchStrategy
{
    private readonly IHttpClientFactory clientFactory = clientFactory;
    private readonly IMapper mapper = mapper;

    public ChartSource Source => ChartSource.RhythmVerse;

    public bool CanHandle(ChartSearchOptions options)
    {
        /* 
         * as for now there is no other unsupported search options
         * it'll be less overengeneering, in future (other search sources)
         * this might be refactored for a runtime reflection properties check
        */
        if (options is ChartSearchAdvancedOptions advancedOptions)
        {
            /*
             * all except year, requires additional requests to retrieve "short_names"
             * so for now I mark these fields as unhandlable
            */
            return advancedOptions.Album is null &&
                advancedOptions.Artist is null &&
                advancedOptions.Charter is null &&
                advancedOptions.Genre is null;
        }

        return true;
    }

    public async Task<IEnumerable<Chart>> SearchAsync(ChartSearchOptions options)
    {
        var client = clientFactory.CreateClient(Source.ToString());
        var message = ComposeRequestMessage(client, options);
        var response = await client.SendAsync(message);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<RhythmVerseSearchResponse>();
        return mapper.Map<SearchResponse>(content).Items;
    }

    private HttpRequestMessage ComposeRequestMessage(HttpClient client, ChartSearchOptions options)
    {
        string uri = "/api/chm/songfiles/search/live";

        if (options is ChartSearchAdvancedOptions advanced && string.IsNullOrEmpty(advanced.Name))
        {
            uri = "/api/chm/songfiles/list";
        }

        var searchRequest = mapper.Map<RhythmVerseSearchRequest>(options);

        var payload = new Dictionary<string, string>();
        var namingPolicy = JsonNamingPolicy.SnakeCaseLower;

        var properties = searchRequest.GetType().GetProperties();

        foreach (var prop in properties)
        {
            string convertedKey = namingPolicy.ConvertName(prop.Name);
            string? value = prop.GetValue(searchRequest)?.ToString();
            if (value is not null)
            {
                payload.Add(convertedKey, value);
            }
        }

        return new HttpRequestMessage
        {
            RequestUri = new Uri(client.BaseAddress!, uri),
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(payload)
        };
    }
}
