using System.Collections.Generic;

namespace Shared.Models.Response
{
    public class CollectionResponse<T>
    {
        public IEnumerable<T> Records { get; set; }
        public PagingInformations PagingInformations { get; set; }
    }
}
