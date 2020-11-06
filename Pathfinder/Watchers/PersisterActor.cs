using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Pathfinder.People;
using Pathfinder.Persistence;

namespace Pathfinder
{
    public class PersisterActor<T> : IActor
    {
        public IPersister Persister { get; set; }

        public void Added(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<T>;
            Persister.Save(collection);
        }

        public void Removed(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<T>;
            Persister.Save(collection);
        }

        public void Updated(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}