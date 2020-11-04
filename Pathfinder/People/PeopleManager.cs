using System.Collections.ObjectModel;
using System.Linq;
using Pathfinder.Persistence;

namespace Pathfinder.People
{
    public class PeopleManager
    {
        public string MapName;
        public ObservableCollection<Person> People;

        public PeopleManager(string mapName)
        {
            People = new ObservableCollection<Person>();
            MapName = mapName;
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
            var personToUpdate = GetPersonById(person.Id);
            if (personToUpdate != null)
            {
                personToUpdate.Position = person.Position;
            }
        }

        public Person GetPersonById(int id)
        {
            return People.FirstOrDefault(p => p.Id == id);
        }

        public void AddPerson(Person person)
        {
            person.MapName = MapName;
            if (!PersonIsInPeople(person))
                People.Add(person);
        }

        public bool PersonIsInPeople(Person person)
        {
            var got = GetPersonById(person.Id);
            if (got == null)
                return false;

            return true;
        }

        public void LoadNpcs(FilePersister persister)
        {
            var npcs = persister.Load<ObservableCollection<Person>>();
            People = npcs;
        }

        public void LoadNpcsOrCreateNew(FilePersister persister)
        {
            if (persister.Exists())
            {
                LoadNpcs(persister);
                return;
            }

            People = new ObservableCollection<Person>();
        }
    }
}