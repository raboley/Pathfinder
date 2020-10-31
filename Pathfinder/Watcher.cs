using System.ComponentModel;
using Pathfinder.Travel;

namespace Pathfinder
{
    public class Watcher
    {
        public Watcher(Traveler traveler, IActor actor)
        {
            Traveler = traveler;
            Actor = actor;

            traveler.PropertyChanged += ItemChanged;
        }

        public Traveler Traveler { get; set; }
        public IActor Actor { get; set; }

        public void ItemChanged(object sender, PropertyChangedEventArgs e)
        {
            Actor.Updated();
        }
    }
}