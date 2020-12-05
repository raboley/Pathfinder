using System.Collections.Generic;

namespace FinalFantasyXI.Equipment
{
    public static class EquipmentManager
    {
        public static IEquipmentItem GetBestEquipmentItemForSlot(EquipSlots slot, List<IEquipmentItem> equipment)
        {
            return new EquipmentItem();
        }
    }

    public interface IPlayer
    {
        Job Job { get; set; }
        int Level { get; set; }
    }

    public class Player : IPlayer
    {
        public Job Job { get; set; }
        public int Level { get; set; }
    }
}