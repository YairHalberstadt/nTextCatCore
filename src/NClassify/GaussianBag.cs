﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NClassify
{
    public class GaussianBag : IBag<ulong>
    {
        private const int ShortcutsRange = 256*256;
        private Dictionary<ulong, DictionaryCount> _store = new Dictionary<ulong, DictionaryCount>();
        private long[] _speedCounts = new long[ShortcutsRange];
        private long _totalCopiesCount;

        public long GetNumberOfCopies(ulong item)
        {
            if (item < ShortcutsRange)
            {
                return _speedCounts[item];
            }

            DictionaryCount count;
            if (_store.TryGetValue(item, out count))
                return count._count;
            return 0;
        }

        public IEnumerable<ulong> DistinctItems
        {
            get
            {
                // todo: check if this yielding doesn't introduce a bottleneck
                var length = (ulong) _speedCounts.Length;
                for (ulong i = 0; i < length; i++)
                {
                    var count = _speedCounts[i];
                    if (count > 0)
                        yield return i;

                }
                foreach (var key in _store.Keys)
                {
                    yield return key;
                }
            }
        }

        public long DistinctItemsCount
        {
            get { return _store.Keys.Count; }
        }

        public bool Add(ulong item, long count)
        {
            if (item < ShortcutsRange)
            {
                _speedCounts[item] += count;
                _totalCopiesCount += count;
                return true;
            }

            DictionaryCount oldCount;
            long newCount = (_store.TryGetValue(item, out oldCount) ? oldCount._count : 0) + count;
            if (newCount < 0)
                return false;
            //_store[item] = newCount;
            if (oldCount == null)
                _store.Add(item, new DictionaryCount(newCount));
            else 
                oldCount._count = newCount;
            _totalCopiesCount += count;
            return true;
        }

        public bool RemoveCopies(ulong item, long count)
        {
            return Add(item, -count);
        }

        public void RemoveAllCopies(ulong item)
        {
            if (item < ShortcutsRange)
            {
                _totalCopiesCount -= _speedCounts[item]; 
                _speedCounts[item] = 0;
                return;
            }

            DictionaryCount oldCount;
            if (_store.TryGetValue(item, out oldCount))
            {
                _totalCopiesCount -= oldCount._count;
                _store.Remove(item);
            }
        }

        public long TotalCopiesCount
        {
            get { return _totalCopiesCount; }
        }

        public IEnumerator<KeyValuePair<ulong, long>> GetEnumerator()
        {
            List<KeyValuePair<ulong, long>> list = new List<KeyValuePair<ulong, long>>();
            for (uint i = 0; i < _speedCounts.Length; i++)
            {
                long speedCount = _speedCounts[i];
                if (speedCount > 0)
                    list.Add(new KeyValuePair<ulong, long>(i, speedCount));
            }
            return list.Concat(_store.Select(kvp => new KeyValuePair<ulong, long>(kvp.Key, kvp.Value._count))).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class DictionaryCount
        {
            public DictionaryCount(long count)
            {
                _count = count;
            }

            public long _count;
        }
    }
}
