using System.Numerics;
using Pathfinder.Map.WorldMap;

namespace Pathfinder.Tests
{
    internal static class GridFactorySetup
    {
        public static ZoneMapFactory SetupGridFactory()
        {
            var gridFactory = new ZoneMapFactory();
            var persister = SetupPersister.SetupTestFilePersister();
            gridFactory.Persister = persister;
            gridFactory.DefaultGridSize = new Vector2(3f, 3f);
            return gridFactory;
        }
    }
}