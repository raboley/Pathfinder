using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Pathfinder.People;
using Pathfinder.Persistence;

namespace Pathfinder
{
    public class PersonActor : IActor
    {
        public IPersister Persister { get; set; }

        public void Added(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<Person>;
            Persister.Save(collection);
        }

        public void Removed(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Updated(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}