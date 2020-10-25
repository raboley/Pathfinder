namespace Pathfinder.Pathfinder
{
    public interface IPersister
    {
        void Save<T>(T serializableData);
        T Load<T>();
    }
}