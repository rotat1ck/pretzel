using AutoMapper;
using Pretzel.Core.Enums;
using Pretzel.Core.Models;
using Pretzel.Core.Models.Search;
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

        CreateMap<ChorusEncoreSearchResponse, ChartSearchResults>()
            .ForMember(dest => dest.Returned, opt => opt.MapFrom(src => Math.Min(src.Count, ChartSearchOptions.DefaultPageSize)));

        CreateMap<ChartSearchOptions, ChorusEncoreSearchRequest>();
        CreateMap<ChartSearchAdvancedOptions, ChorusEncoreSearchAdvancedRequest>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new ChorusEncoreSearchFilter { Value = src.Name ?? string.Empty }))
            .ForMember(dest => dest.Album, opt => opt.MapFrom(src => new ChorusEncoreSearchFilter { Value = src.Album ?? string.Empty }))
            .ForMember(dest => dest.Artist, opt => opt.MapFrom(src => new ChorusEncoreSearchFilter { Value = src.Artist ?? string.Empty }))
            .ForMember(dest => dest.Charter, opt => opt.MapFrom(src => new ChorusEncoreSearchFilter { Value = src.Charter ?? string.Empty }))
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => new ChorusEncoreSearchFilter { Value = src.Genre ?? string.Empty }))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => new ChorusEncoreSearchFilter { Value = src.Year.ToString() ?? string.Empty }));
    }
}
