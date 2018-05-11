using BookFast.Search.Contracts;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using System.Threading.Tasks;

namespace BookFast.Search.Adapter
{
    internal class SearchIndexer : ISearchIndexer
    {
        private readonly ISearchIndexClient client;

        public SearchIndexer(ISearchIndexClient client)
        {
            this.client = client;
        }

        public Task DeleteAccommodationIndexAsync(int accommodationId)
        {
            var action = IndexAction.Delete(new Document { { "Id", accommodationId.ToString() } });
            return client.Documents.IndexAsync(new IndexBatch(new[] { action }));
        }

        public Task IndexAccommodationAsync(Contracts.Models.Accommodation accommodation)
        {
            var action = IndexAction.MergeOrUpload(CreateDocument(accommodation));
            return client.Documents.IndexAsync(new IndexBatch(new[] { action }));
        }

        private static Document CreateDocument(Contracts.Models.Accommodation accommodation)
        {
            return new Document
                   {
                       { "Id", accommodation.Id.ToString() },
                       { "FacilityId", accommodation.FacilityId },
                       { "Name", accommodation.Name },
                       { "Description", accommodation.Description },
                       { "FacilityName", accommodation.FacilityName },
                       { "FacilityDescription", accommodation.FacilityDescription },
                       { "Location", CreateGeographyPoint(accommodation.FacilityLocation) },
                       { "RoomCount", accommodation.RoomCount },
                       { "Images", accommodation.Images }
                   };
        }

        private static GeographyPoint CreateGeographyPoint(Contracts.Models.Location location)
        {
            if (location != null && location.Latitude != null && location.Longitude != null)
            {
                return GeographyPoint.Create(location.Latitude.Value, location.Longitude.Value);
            }

            return null;
        }
    }
}
