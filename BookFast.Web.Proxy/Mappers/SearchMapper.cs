using System.Collections.Generic;
using AutoMapper;
using BookFast.Search.Client.Models;

namespace BookFast.Web.Proxy.Mappers
{
    internal class SearchMapper : ISearchMapper
    {
        private static readonly IMapper Mapper;

        static SearchMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<SearchResult, Contracts.Search.SearchResult>()
                             .ConvertUsing(result => new Contracts.Search.SearchResult
                                                     {
                                                         Score = result.Score ?? 0,
                                                         Highlights = new Contracts.Search.HitHighlights(result.Highlights),
                                                         Document = new Contracts.Search.Document(result.Document)
                                                     });
            });

            mapperConfiguration.AssertConfigurationIsValid();
            Mapper = mapperConfiguration.CreateMapper();
        }

        public IList<Contracts.Search.SearchResult> MapFrom(IList<SearchResult> results)
        {
            return Mapper.Map<IList<Contracts.Search.SearchResult>>(results);
        }
    }
}