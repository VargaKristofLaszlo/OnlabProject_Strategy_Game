using System.Collections.Generic;

namespace Shared.Models.Response
{
    public record CollectionResponse<T>
    {
        public IEnumerable<T> Records { get; init; }
        public PagingInformations PagingInformations { get; init; }
    }
}
