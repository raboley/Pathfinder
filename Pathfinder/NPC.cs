using System.Numerics;

namespace Pathfinder.Pathfinder
{
    public class NPC : IEntity
    {
        public NPC(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public string MapName { get; set; }
    }
}