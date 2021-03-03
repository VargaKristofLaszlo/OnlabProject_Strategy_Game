using System.Text.Json.Serialization;

namespace Shared.Models
{
    public record PagingInformations
    {
        public int? PageNumber { get; init; }
        public int? PageSize { get; init; }
        public int? PagesCount { get; init; }      
    }
}
