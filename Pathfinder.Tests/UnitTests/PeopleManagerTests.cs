using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using Pathfinder.People;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class PeopleManagerTests
    {
        [Fact]
        public void ConstructorCanInitializePeople()
        {
            var people = new ObservableCollection<Person>
            {
                new Person("Jim", Vector3.Zero),
                new Person("Sparky", Vector3.One)
            };

            var peopleManager = new PeopleManager {People = people};

            AssertPeopleEqual(people, peopleManager);
        }

        private static void AssertPeopleEqual(IReadOnlyList<Person> people, PeopleManager peopleManager)
        {
            Assert.Equal(people.Count, peopleManager.People.Count);
            for (var i = 0; i < people.Count; i++)
            {
                Assert.Equal(people[i].Name, peopleManager.People[i].Name);
                Assert.Equal(people[i].Position, peopleManager.People[i].Position);
            }
        }
    }
}