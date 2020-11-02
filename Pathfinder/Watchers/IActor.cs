using System.Collections.Specialized;
using System.ComponentModel;

namespace Pathfinder
{
    public interface IActor
    {
        void Added(object sender, NotifyCollectionChangedEventArgs e);
        void Removed(object sender, NotifyCollectionChangedEventArgs e);
        void Updated(object sender, PropertyChangedEventArgs e);
    }
}