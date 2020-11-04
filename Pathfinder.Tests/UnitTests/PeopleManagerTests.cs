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

            var peopleManager = new PeopleManager("Test") {People = people};

            AssertPeopleEqual(people, peopleManager.People);
        }


        [Fact]
        public void CanAddPeopleToPeopleList()
        {
            var people = new ObservableCollection<Person>
            {
                new Person(1, "Sparky", Vector3.One)
            };
            var personToAdd = new Person(1, "Sparky", Vector3.One);

            var peopleManager = new PeopleManager("test");
            peopleManager.AddPerson(personToAdd);

            AssertPeopleEqual(people, peopleManager.People);
        }

        [Fact]
        public void CanGetPersonInPeople()
        {
            var want = new Person(2, "Sparky", Vector3.One);

            var people = SetupPeopleCollection();

            var peopleManager = new PeopleManager("test") {People = people};
            var got = peopleManager.GetPersonById(want.Id);

            AssertPersonEqual(want, got);
        }


        [Fact]
        public void CanUpdatePersonAlreadyInCollectionById()
        {
            var people = new ObservableCollection<Person>
            {
                new Person(1, "Sparky", Vector3.One)
            };
            var personToModify = new Person(1, "Sparky", Vector3.One);

            var peopleManager = new PeopleManager("test")
                {People = new ObservableCollection<Person> {new Person(1, "Sparky", Vector3.Zero)}};
            peopleManager.UpdatePerson(personToModify);

            AssertPeopleEqual(people, peopleManager.People);
        }

        [Fact]
        public void PeopleIsInPeopleReturnsTrueWhenExists()
        {
            var want = true;
            var people = SetupPeopleCollection();

            var peopleManager = new PeopleManager("test") {People = people};
            bool got = peopleManager.PersonIsInPeople(people[0]);

            Assert.Equal(want, got);
        }

        [Fact]
        public void PeopleIsInPeopleReturnsFalseWhenDoesntExist()
        {
            var want = false;
            var person = new Person(99, "not in people", Vector3.One);
            var people = SetupPeopleCollection();

            var peopleManager = new PeopleManager("test") {People = people};
            bool got = peopleManager.PersonIsInPeople(person);

            Assert.Equal(want, got);
        }

        [Fact]
        public void InsertWhenTryingToAddPersonWhoDoesntExist()
        {
            var people = SetupPeopleCollection();
            var person = new Person(3, "Toby", Vector3.One);
            people.Add(person);

            var peopleManager = new PeopleManager("test")
            {
                People = new ObservableCollection<Person>
                {
                    new Person(1, "Jim", Vector3.Zero),
                    new Person(2, "Sparky", Vector3.One)
                }
            };
            peopleManager.AddOrUpdatePerson(person);

            AssertPeopleEqual(people, peopleManager.People);
        }

        [Fact]
        public void UpdateWhenTwoPeopleWithSameIdTryToGetAdded()
        {
            var people = SetupPeopleCollection();

            var peopleManager = new PeopleManager("test")
            {
                People = new ObservableCollection<Person>
                {
                    new Person(1, "Jim", Vector3.One),
                    new Person(2, "Sparky", Vector3.One)
                }
            };
            peopleManager.AddOrUpdatePerson(new Person(1, "Jim", Vector3.Zero));

            AssertPeopleEqual(people, peopleManager.People);
        }

        [Fact]
        public void AddingPeopleWithSameIdWillNotAddDuplicates()
        {
            var name = "TestMap";
            var got = new PeopleManager(name);

            got.People = new ObservableCollection<Person>();
            got.AddPerson(new Person(0, "test person", Vector3.One));
            got.AddPerson(new Person(1, "a person", Vector3.One));
            // second shouldn't add even though position changed due to property
            got.AddPerson(new Person(1, "a person", Vector3.Zero));
            got.AddPerson(new Person(1, "I changed name person", Vector3.One));

            Assert.Equal(2, got.People.Count);
        }

        public static ObservableCollection<Person> SetupPeopleCollection()
        {
            var people = new ObservableCollection<Person>
            {
                new Person(1, "Jim", Vector3.Zero),
                new Person(2, "Sparky", Vector3.One)
            };
            return people;
        }

        private static void AssertPeopleEqual(IReadOnlyList<Person> want, ObservableCollection<Person> got)
        {
            Assert.Equal(want.Count, got.Count);
            for (var i = 0; i < want.Count; i++) AssertPersonEqual(want[i], got[i]);
        }

        private static void AssertPersonEqual(Person want, Person got)
        {
            Assert.Equal(want.Id, got.Id);
            Assert.Equal(want.Position, got.Position);
            Assert.Equal(want.Name, got.Name);
        }
    }
}