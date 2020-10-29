using System.Collections.ObjectModel;
using System.Linq;

namespace Pathfinder.People
{
    public class PeopleManager
    {
        public ObservableCollection<Person> People;

        public PeopleManager()
        {
            People = new ObservableCollection<Person>();
        }

        public void AddOrUpdatePerson(Person person)
        {
            if (PersonIsInPeople(person))
            {
                UpdatePerson(person);
                return;
            }

            AddPerson(person);
        }

        public void UpdatePerson(Person person)
        {
            var personToUpdate = GetPerson(person);
            if (personToUpdate != null)
            {
                personToUpdate.Position = person.Position;
            }
        }

        public Person GetPerson(Person person)
        {
            return People.FirstOrDefault(p => p.Id == person.Id);
        }

        public void AddPerson(Person person)
        {
            People.Add(person);
        }

        public bool PersonIsInPeople(Person person)
        {
            var got = GetPerson(person);
            if (got == null)
                return false;

            return true;
        }
    }
}