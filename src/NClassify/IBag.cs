using System.Collections.Generic;

namespace NClassify
{
    public interface IBag<T> : IEnumerable<KeyValuePair<T, long>>
    {
        long GetNumberOfCopies(T item);
        IEnumerable<T> DistinctItems { get; }
        long DistinctItemsCount { get; }
        bool Add(T item, long count);
        bool RemoveCopies(T item, long count);
        void RemoveAllCopies(T item);
        long TotalCopiesCount { get; }
    }
}
