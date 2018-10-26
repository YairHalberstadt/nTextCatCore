using System;
using System.Collections.Generic;

namespace NClassify
{
    public interface ICategorizedClassifier<TItem, TCategory>
    {
        IEnumerable<Tuple<TCategory, double>> Classify(TItem item);
    }
}
