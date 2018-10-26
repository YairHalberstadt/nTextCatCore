using System.Collections.Generic;

namespace NClassify
{
    public interface IFeatureExtractor<TSource, TFeature>
    {
        IEnumerable<TFeature> GetFeatures(TSource obj);
    }
}
