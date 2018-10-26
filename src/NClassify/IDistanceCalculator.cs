namespace NClassify
{
    public interface IDistanceCalculator<in T>
    {
        double CalculateDistance(T obj1, T obj2);
    }
}
