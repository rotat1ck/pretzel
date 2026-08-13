using AutoMapper;
using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Models;
using Pretzel.Core.Models.Search;
using Pretzel.Infrastructure.DTOs.Search;
using Pretzel.Infrastructure.DTOs.Search.ChorusEncore;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pretzel.Infrastructure.Services.Search;

public class ChorusEncoreSearchStrategy(IHttpClientFactory clientFactory, IMapper mapper) : IChartSearchStrategy
{
    private readonly IHttpClientFactory clientFactory = clientFactory;
    private readonly IMapper mapper = mapper;

    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ChartSource Source => ChartSource.ChorusEncore;

    public bool CanHandle(ChartSearchOptions options)
    {
        /* 
         * as for now there is no other unsupported search options
         * it'll be less overengeneering, in future (other search sources)
         * this might be refactored for a runtime reflection properties check
        */
        //if (options is ChartSearchAdvancedOptions advancedOptions)
        //{
        //    return advancedOptions.Album is null;
        //}

        return true;
    }

    public async Task<IEnumerable<Chart>> SearchAsync(ChartSearchOptions options)
    {
        var client = clientFactory.CreateClient(Source.ToString());
        var message = ComposeRequestMessage(client, options);
        var response = await client.SendAsync(message);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<ChorusEncoreSearchResponse>();
        content?.Items = content.Items.DistinctBy(chart => chart.Ordering);
        return mapper.Map<SearchResponse>(content).Items;
    }

    private HttpRequestMessage ComposeRequestMessage(HttpClient client, ChartSearchOptions options)
    {
        object payload;
        string uri;

        if (options is ChartSearchAdvancedOptions advanced)
        {
            uri = "/search/advanced";
            payload = mapper.Map<ChorusEncoreSearchAdvancedRequest>(advanced);
        }
        else
        {
            uri = "/search";
            payload = mapper.Map<ChorusEncoreSearchRequest>(options);
        }

        return new HttpRequestMessage
        {
            RequestUri = new Uri(client.BaseAddress!, uri),
            Method = HttpMethod.Post,
            Content = JsonContent.Create(payload, options: jsonOptions)
        };
    }
}
