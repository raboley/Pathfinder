using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Pathfinder
{
    public class CollectionWatcher<T>
    {
        public CollectionWatcher(ObservableCollection<T> collection, IActor actor)
        {
            Collection = collection;
            Actor = actor;

            collection.CollectionChanged += Changed;

            if (collection.Count > 0)
                foreach (var unknown in collection)
                {
                    var item = (INotifyPropertyChanged) unknown;
                    item.PropertyChanged += ItemChanged;
                }
        }

        public ObservableCollection<T> Collection { get; set; }
        public IActor Actor { get; set; }

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

        // Updates are more complicated:
        // https://stackoverflow.com/questions/901921/observablecollection-and-item-propertychanged
        public void ItemChanged(object sender, PropertyChangedEventArgs e)
        {
            Actor.Updated();
        }
    }
}