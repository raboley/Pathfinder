using System.Numerics;
using Pathfinder.WorldMap;

namespace Pathfinder.Tests
{
    internal static class GridFactorySetup
    {
        public static GridFactory SetupGridFactory()
        {
            var gridFactory = new GridFactory();
            var persister = SetupPersister.SetupTestFilePersister();
            gridFactory.Persister = persister;
            gridFactory.DefaultGridSize = new Vector2(3f, 3f);
            return gridFactory;
        }
    }
}