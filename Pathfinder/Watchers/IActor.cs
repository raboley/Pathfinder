using System.ComponentModel;

namespace Pathfinder
{
    public interface IActor
    {
        void Added();
        void Removed();
        void Updated(object sender, PropertyChangedEventArgs e);
    }
}