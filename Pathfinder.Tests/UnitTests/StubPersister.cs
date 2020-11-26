using System.Collections.Generic;
using Pathfinder.Persistence;

namespace Pathfinder.Tests.UnitTests
{
    public class StubPersister : IPersister
    {
        public bool ExistShouldReturn { get; set; } = false;
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

        public bool Exists()
        {
            return ExistShouldReturn;
        }

        public List<T> LoadAllOfType<T>()
        {
            throw new System.NotImplementedException();
        }
    }
}