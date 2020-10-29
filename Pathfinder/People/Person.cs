using System.Numerics;

namespace Pathfinder.People
{
    public class Person : IEntity
    {
        public Person(int id, string name, Vector3 position)
        {
            Name = name;
            Position = position;
            Id = id;
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public string MapName { get; set; }
    }
}