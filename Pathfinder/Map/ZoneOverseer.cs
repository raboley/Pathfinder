namespace Pathfinder.Map
{
    public class ZoneOverseer
    {
        public ZoneOverseer(ZoneMapFactory zoneMapFactory, CollectionWatcher<Node> collectionWatcher)
        {
            ZoneMapFactory = zoneMapFactory;
            CollectionWatcher = collectionWatcher;
        }

        public ZoneMapFactory ZoneMapFactory { get; set; }
        public CollectionWatcher<Node> CollectionWatcher { get; set; }
    }
}