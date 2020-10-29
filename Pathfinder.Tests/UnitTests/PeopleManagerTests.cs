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
            var people = SetupPeopleCollection();

            var peopleManager = new PeopleManager {People = people};

            AssertPeopleEqual(people, peopleManager.People);
        }


        [Fact]
        public void CanAddPeopleToPeopleList()
        {
            var people = new ObservableCollection<Person>
            {
                new Person("Sparky", Vector3.One)
            };
            var personToAdd = new Person("Sparky", Vector3.One);

            var peopleManager = new PeopleManager();
            peopleManager.AddPerson(personToAdd);

            AssertPeopleEqual(people, peopleManager.People);
        }

        [Fact]
        public void CanGetPersonInPeople()
        {
            var want = new Person("Sparky", Vector3.One, 2);

            var people = SetupPeopleCollection();

            var peopleManager = new PeopleManager {People = people};
            var got = peopleManager.GetPerson(want);

            AssertPersonEqual(want, got);
        }


        [Fact]
        public void CanUpdatePersonAlreadyInCollectionById()
        {
            var people = new ObservableCollection<Person>
            {
                new Person("Sparky", Vector3.One, 1)
            };
            var personToModify = new Person("Sparky", Vector3.One, 1);

            var peopleManager = new PeopleManager
                {People = new ObservableCollection<Person> {new Person("Sparky", Vector3.Zero, 1)}};
            peopleManager.UpdatePerson(personToModify);

            AssertPeopleEqual(people, peopleManager.People);
        }

        [Fact]
        public void PeopleIsInPeopleReturnsTrueWhenExists()
        {
            var want = true;
            var people = SetupPeopleCollection();

            var peopleManager = new PeopleManager {People = people};
            var got = peopleManager.PersonIsInPeople(people[0]);

            Assert.Equal(want, got);
        }

        [Fact]
        public void PeopleIsInPeopleReturnsFalseWhenDoesntExist()
        {
            var want = false;
            var person = new Person("not in people", Vector3.One, 99);
            var people = SetupPeopleCollection();

            var peopleManager = new PeopleManager {People = people};
            var got = peopleManager.PersonIsInPeople(person);

            Assert.Equal(want, got);
        }


        [Fact]
        public void UpdateWhenTwoPeopleWithSameIdTryToGetAdded()
        {
            var people = SetupPeopleCollection();

            var peopleManager = new PeopleManager
            {
                People = new ObservableCollection<Person>
                {
                    new Person("Jim", Vector3.One, 1),
                    new Person("Sparky", Vector3.One, 2)
                }
            };
            peopleManager.AddOrUpdatePerson(new Person("Jim", Vector3.Zero, 1));

            AssertPeopleEqual(people, peopleManager.People);
        }

        private static ObservableCollection<Person> SetupPeopleCollection()
        {
            var people = new ObservableCollection<Person>
            {
                new Person("Jim", Vector3.Zero, 1),
                new Person("Sparky", Vector3.One, 2)
            };
            return people;
        }

        private static void AssertPeopleEqual(IReadOnlyList<Person> want, ObservableCollection<Person> got)
        {
            Assert.Equal(want.Count, got.Count);
            for (var i = 0; i < want.Count; i++)
            {
                AssertPersonEqual(want[i], got[i]);
            }
        }

        private static void AssertPersonEqual(Person want, Person got)
        {
            Assert.Equal(want.Id, got.Id);
            Assert.Equal(want.Position, got.Position);
            Assert.Equal(want.Name, got.Name);
        }
    }
}