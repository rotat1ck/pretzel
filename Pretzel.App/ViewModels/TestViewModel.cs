using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Pretzel.Core.Models;
using Pretzel.Infrastructure.DTOs.Search;
using Pretzel.Infrastructure.DTOs.Search.ChorusEncore;
using Pretzel.Infrastructure.DTOs.Search.RhythmVerse;
using System.Net.Http.Json;

namespace Pretzel.App.ViewModels;

public partial class TestViewModel : ObservableObject
{
    private readonly IMapper mapper;

    public TestViewModel(IMapper mapper)
    {
        this.mapper = mapper;
        _ = Test();
    }

    [ObservableProperty]
    private List<Chart> charts;

    public async Task Test()
    {
        var chorusClient = new HttpClient();
        var rhythmClient = new HttpClient();

        chorusClient.BaseAddress = new Uri("https://api.enchor.us");
        rhythmClient.BaseAddress = new Uri("https://rhythmverse.co");

        var rhythmFormData = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "text", "Cadmium Colors" },
            { "data_type", "full" },
            { "page", "1" },
            { "records", "10" },
        });

        var chorusJson = new
        {
            search = "Jamie Paige - Cadmium Colors",
            page = 1
        };

        var chorusContent = await chorusClient.PostAsJsonAsync("search", chorusJson);
        var rhythmResponse = await rhythmClient.PostAsync("api/all/songfiles/search/live", rhythmFormData);

        var chorusDto = await chorusContent.Content.ReadFromJsonAsync<ChorusEncoreSearchResponse>();
        var rhythmDto = await rhythmResponse.Content.ReadFromJsonAsync<RhythmVerseSearchResponse>();

        var searchResponse = mapper.Map<SearchResponse>(chorusDto);
        var toCombine = mapper.Map<SearchResponse>(rhythmDto);

        var response = searchResponse.Items as List<Chart>;
        response.AddRange(toCombine.Items);


        Charts = response;
    }
}
