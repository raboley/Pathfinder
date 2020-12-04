using System.Collections.Generic;
using FinalFantasyXI.Equipment;
using Xunit;

namespace FinalFantasyXI.Tests.UnitTests
{
    public class EquipmentTests
    {
        [Fact]
        public void GetSlotCanSetTheSlot()
        {
            var want = new List<EquipSlots>
            {
                EquipSlots.Main,
                EquipSlots.Sub
            };

            var weapon = new IItem {Slots = 3};
            var got = Equipment.Equipment.GetEquipSlots(weapon);

            Assert.Equal(want, got);
        }
    }
}