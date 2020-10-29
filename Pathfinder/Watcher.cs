using System.Collections.ObjectModel;

namespace Pathfinder
{
    public class Watcher<T>
    {
        public ObservableCollection<T> Collection { get; set; }
        public SpyActor Actor { get; set; }
    }
}