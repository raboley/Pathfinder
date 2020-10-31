using System.ComponentModel;

namespace Pathfinder
{
    // Implementation found at:
    // https://dotnetcodr.com/2015/05/29/getting-notified-when-collection-changes-with-observablecollection-in-c-net/
    public class SpyActor : IActor
    {
        public int CalledTimesAdd { get; set; }
        public int CalledTimesRemove { get; set; }
        public int CalledTimesUpdate { get; set; }

        public void Added()
        {
            CalledTimesAdd++;
        }

        public void Removed()
        {
            CalledTimesRemove++;
        }

        public void Updated(object sender, PropertyChangedEventArgs e)
        {
            CalledTimesUpdate++;
        }
    }
}