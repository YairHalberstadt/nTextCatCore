namespace NClassify
{
    public interface ITrainee<T>
    {
        void LearnMatch(T obj);
        void LearnMismatch(T obj);
    }
}
