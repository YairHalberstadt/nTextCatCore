namespace NClassify
{
    public interface IClassifier<T>
    {
        double Classify(T obj);
    }
}
