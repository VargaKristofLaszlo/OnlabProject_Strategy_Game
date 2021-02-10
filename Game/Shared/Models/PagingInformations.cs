using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class PagingInformations
    {
        [JsonPropertyName("pagenumber")]
        public int? PageNumber { get; set; }

        [JsonPropertyName("pagesize")]
        public int? PageSize { get; set; }

        [JsonPropertyName("pagescount")]
        public int? PagesCount { get; set; }

        public PagingInformations() {}

        public PagingInformations(int? pageNumber = null, int? pageSize = null, int? pagesCount = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PagesCount = pagesCount;
        }
    }
}
