namespace Pathfinder.Pathfinder
{
    public class GridFactory
    {
        public Grid LoadGrid(string mapName)
        {
            Persister.MapName = mapName;
            var grid = Persister.Load<Grid>();
            return grid;
        }

        public IPersister Persister { get; set; }
    }
}