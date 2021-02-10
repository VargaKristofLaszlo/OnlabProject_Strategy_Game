using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Shared.Models.Response
{
    public class CollectionResponse<T> : BaseResponse
    {
        [JsonPropertyName("records")]
        public IEnumerable<T> Records { get; set; }

        [JsonPropertyName("paginginformations")]
        public PagingInformations PagingInformations { get; set; }


        public CollectionResponse() : base() { } 


        private CollectionResponse(string message, bool isSuccess, IEnumerable<T> records,
            PagingInformations pagingInformations, IEnumerable<string> errors):base(message, isSuccess, errors)
        {
            Records = records;
            PagingInformations = pagingInformations;
        }

        public static CollectionResponse<T> Succeeded(string message, IEnumerable<T> records, 
            PagingInformations pagingInformations) 
        {
            return new CollectionResponse<T>(message, true, records, pagingInformations, null);
        }

        public static CollectionResponse<T> Failed(string message, IEnumerable<string> errors = null)
        {
            return new CollectionResponse<T>(message, false, Enumerable.Empty<T>(), null, errors);
        }
    }
}
