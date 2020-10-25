namespace Pathfinder.Pathfinder
{
    public interface IPersister
    {
        void Save(GridNode node);
        GridNode Load();
    }
}