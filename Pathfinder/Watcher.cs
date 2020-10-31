using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Pathfinder.People;

namespace Pathfinder
{
    public class Watcher
    {
        public Watcher(ObservableCollection<Person> collection, IActor actor)
        {
            Collection = collection;
            Actor = actor;

            collection.CollectionChanged += Changed;

            if (collection.Count > 0)
                foreach (var item in collection)
                    item.PropertyChanged += ItemChanged;
        }

        public ObservableCollection<Person> Collection { get; set; }
        public IActor Actor { get; set; }

        // TODO: Move this to the watcher class.
        public void Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += ItemChanged;
                Actor.Added();
            }

            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= ItemChanged;
                Actor.Removed();
            }
        }

        // TODO: Move this to the watcher class.
        // Updates are more complicated:
        // https://stackoverflow.com/questions/901921/observablecollection-and-item-propertychanged
        public void ItemChanged(object sender, PropertyChangedEventArgs e)
        {
            Actor.Updated();
        }
    }
}