using Pathfinder.Persistence;

namespace Pathfinder.Tests.UnitTests
{
    public class SpyPersister : IPersister
    {
        public string MapName { get; set; }
        public void Save<T>(T serializableData)
        {
            
        }

        public T Load<T>()
        {
            throw new System.NotImplementedException();
        }

        public void Delete()
        {
        }

        public bool ExistShouldReturn { get; set; } = false;
        public bool Exists()
        {
            return ExistShouldReturn;
        }
    }
}