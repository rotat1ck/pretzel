using AutoMapper;
using Pretzel.Core.Enums;
using Pretzel.Core.Models;
using Pretzel.Infrastructure.DTOs.Search;
using Pretzel.Infrastructure.DTOs.Search.ChorusEncore;

namespace Pretzel.Infrastructure.Mappers;

public class ChorusEncoreProfile : Profile
{
    public ChorusEncoreProfile()
    {
        CreateMap<ChorusEncoreChartResponse, Chart>()
            .AfterMap((src, dest) =>
            {
                dest.DownloadSources.Add(new ChartDownloadSource
                {
                    Source = ChartSource.ChorusEncore,
                    ChartUri = src.ChartUri,
                    AlbumArtUri = src.AlbumArtUri
                });
            });

        CreateMap<ChorusEncoreSearchResponse, SearchResponse>()
            .ForMember(dest => dest.Returned, opt => opt.AddTransform(_ => 10));
    }
}
