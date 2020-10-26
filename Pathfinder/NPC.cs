using System.Numerics;

namespace Pathfinder
{
    public class NPC : IEntity
    {
        public NPC(string name)
        {
            Name = name;
        }

        public NPC(string name, Vector3 position)
        {
            Name = name;
            Position = position;
        }

        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public string MapName { get; set; }
        public int ID { get; set; }
    }
}