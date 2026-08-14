using AutoMapper;
using Pretzel.Core.Enums;
using Pretzel.Core.Models;
using Pretzel.Core.Models.Search;
using Pretzel.Infrastructure.DTOs.Search;
using Pretzel.Infrastructure.DTOs.Search.RhythmVerse;

namespace Pretzel.Infrastructure.Mappers;

public class RhythmVerseProfile : Profile
{
    public RhythmVerseProfile()
    {
        CreateMap<RhythmVerseChartResponse, Chart>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Data.Name))
            .ForMember(dest => dest.Album, opt => opt.MapFrom(src => src.Data.Album))
            .ForMember(dest => dest.Artist, opt => opt.MapFrom(src => src.Data.Artist))
            .ForMember(dest => dest.Charter, opt => opt.MapFrom(src => src.File.Author.Charter))
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Data.Genre))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Data.Year))
            .AfterMap((src, dest) =>
            {
                dest.DownloadSources.Add(new ChartDownloadSource
                {
                    Source = ChartSource.RhythmVerse,
                    ChartUri = src.File.ChartUri,
                    AlbumArtUri = src.File.AlbumArtUri
                });
            });

        CreateMap<RhythmVerseSearchResponse, SearchResponse>()
            .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Data.Records.Count))
            .ForMember(dest => dest.Returned, opt => opt.MapFrom(src => src.Data.Records.Returned))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Data.Items));


        CreateMap<ChartSearchOptions, RhythmVerseSearchRequest>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Search))
            .ForMember(dest => dest.Records, opt => opt.MapFrom(src => src.Records <= 0 ? 10 : src.Records));

        CreateMap<ChartSearchAdvancedOptions, RhythmVerseSearchRequest>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Records, opt => opt.MapFrom(src => src.Records <= 0 ? 10 : src.Records))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Charter));
    }
}
