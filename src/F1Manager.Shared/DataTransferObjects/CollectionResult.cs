using System.Collections.Generic;

namespace F1Manager.Shared.DataTransferObjects
{
    public sealed class CollectionResult<T>
    {

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalEntries { get; set; }
        public List<T> Entities { get; set; }

    }
}