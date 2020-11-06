using System.Numerics;
using Pathfinder.Persistence;

namespace Pathfinder.Map
{
    public class ZoneMapFactory
    {
        private float _defaultGridCenterX;
        private float _defaultGridCenterY;
        private float _defaultGridCenterZ;

        private float _defaultGridSizeX = 500f;
        private float _defaultGridSizeY = 500f;

        public IPersister Persister { get; set; }

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

        public Vector2 DefaultGridSize
        {
            get => new Vector2(_defaultGridSizeX, _defaultGridSizeY);
            set
            {
                _defaultGridSizeX = value.X;
                _defaultGridSizeY = value.Y;
            }
        }

        public ZoneMap LoadGrid(string mapName)
        {
            Persister.MapName = mapName;
            var grid = Persister.Load<ZoneMap>();
            return grid;
        }

        public ZoneMap LoadGridOrCreateNew(string mapName)
        {
            Persister.MapName = mapName;
            if (Persister.Exists()) return LoadGrid(mapName);

            var grid = new ZoneMap();
            grid.MapName = mapName;
            grid.GridCenter = DefaultGridCenter;
            grid.GridWorldSize = DefaultGridSize;
            grid.BuildGridMap();

            return grid;
        }
    }
}