using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class PagingInformations
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int? PagesCount { get; set; }      
    }
}
