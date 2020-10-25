using System.Numerics;

namespace Pathfinder
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

        public Grid LoadGridOrCreateNew(string mapName)
        {
            Persister.MapName = mapName;
            if (Persister.Exists())
            {
                return LoadGrid(mapName);
            }

            var grid = new Grid();
            grid.MapName = mapName;
            grid.GridCenter = DefaultGridCenter;
            grid.GridWorldSize = DefaultGridSize;
            grid.BuildGridMap();

            return grid;
        }

        public Vector3 DefaultGridCenter
        {
            get => new Vector3(_defaultGridCenterX, _defaultGridCenterY, _defaultGridCenterZ);
            set
            {
                _defaultGridCenterX = value.X;
                _defaultGridCenterY = value.Y;
                _defaultGridCenterZ = value.Z;
            }
        }

        private float _defaultGridCenterX = 0f;
        private float _defaultGridCenterY = 0f;
        private float _defaultGridCenterZ = 0f;

        public Vector2 DefaultGridSize
        {
            get => new Vector2(_defaultGridSizeX, _defaultGridSizeY);
            set
            {
                _defaultGridSizeX = value.X;
                _defaultGridSizeY = value.Y;
            }
        }

        private float _defaultGridSizeX = 1000f;
        private float _defaultGridSizeY = 1000f;
    }
}