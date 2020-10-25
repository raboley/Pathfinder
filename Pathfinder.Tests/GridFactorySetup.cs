using System.Numerics;
using Pathfinder.Pathfinder;

namespace Pathfinder.Tests.Pathfinder
{
    static internal class GridFactorySetup
    {
        public static GridFactory SetupGridFactory()
        {
            var gridFactory = new GridFactory();
            var persister = PersisterSetup.SetupTestFilePersister();
            gridFactory.Persister = persister;
            gridFactory.DefaultGridSize = new Vector2(3f, 3f);
            return gridFactory;
        }
    }
}