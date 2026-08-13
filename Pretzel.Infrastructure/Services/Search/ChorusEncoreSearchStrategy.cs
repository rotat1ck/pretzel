using AutoMapper;
using Pretzel.Core.Enums;
using Pretzel.Core.Interfaces;
using Pretzel.Core.Models;
using Pretzel.Core.Models.Search;
using Pretzel.Infrastructure.DTOs.Search;
using Pretzel.Infrastructure.DTOs.Search.ChorusEncore;
using System.Net.Http.Json;

namespace Pretzel.Infrastructure.Services.Search;

public class ChorusEncoreSearchStrategy(IHttpClientFactory clientFactory, IMapper mapper) : IChartSearchStrategy
{
    private readonly IHttpClientFactory clientFactory = clientFactory;
    private readonly IMapper mapper = mapper;

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
        var message = ComposeRequestMessage(options);
        var response = await client.SendAsync(message);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadFromJsonAsync<SearchResponse>();
        return content!.Items;
    }

    private HttpRequestMessage ComposeRequestMessage(ChartSearchOptions options)
    {
        object payload;
        string uri = "/search";

        if (options is ChartSearchAdvancedOptions advanced)
        {
            uri += "/advanced";
            payload = mapper.Map<ChorusEncoreSearchAdvancedRequest>(advanced);
        }
        else
        {
            payload = mapper.Map<ChorusEncoreSearchRequest>(options);
        }

        return new HttpRequestMessage
        {
            RequestUri = new Uri(uri),
            Method = HttpMethod.Post,
            Content = JsonContent.Create(payload)
        };
    }
}
