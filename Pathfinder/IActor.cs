using System.Collections.Specialized;
using System.ComponentModel;

namespace Pathfinder
{
    // Implementation found at:
    // https://dotnetcodr.com/2015/05/29/getting-notified-when-collection-changes-with-observablecollection-in-c-net/
    public class SpyActor
    {
        public int CalledTimesAdd { get; set; }
        public int CalledTimesRemove { get; set; }
        public int CalledTimesUpdate { get; set; }

        private void Added()
        {
            CalledTimesAdd++;
        }

        private void Removed()
        {
            CalledTimesRemove++;
        }

        private void Updated()
        {
            CalledTimesUpdate++;
        }

        public void Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                // Need to subscribe to new items
                // And call add
                Added();

            if (e.OldItems != null)
                Removed();
        }

        // Updates are more complicated:
        // https://stackoverflow.com/questions/901921/observablecollection-and-item-propertychanged
        public void ItemChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}