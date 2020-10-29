namespace Pathfinder.Persistence
{
    public interface IPersister
    {
        string MapName { get; set; }
        void Save<T>(T serializableData);
        T Load<T>();
        void Delete();
        bool Exists();
    }
}