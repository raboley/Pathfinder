using System.Collections.Generic;
using FinalFantasyXI.Equipment;
using Xunit;

namespace FinalFantasyXI.Tests.UnitTests
{
    public class EquipmentTests
    {
        [Fact]
        public void NewEquipment()
        {
            var want = new EquipmentItem();
            want.Name = "OnionSword";
            want.EquipAbleInSlots = new List<EquipSlots>
            {
                EquipSlots.Main,
                EquipSlots.Sub
            };

            var weapon = OnionSword();
            var got = Equipment.Equipment.GetEquipmentFromItem(weapon);

            EquipmentItemEquals(want, got);
        }

        private static void EquipmentItemEquals(EquipmentItem want, EquipmentItem got)
        {
            Assert.Equal(want.Name, got.Name);
            Assert.Equal(want.EquipAbleInSlots, got.EquipAbleInSlots);
        }

        private static IItem OnionSword()
        {
            var weapon = new IItem
            {
                Name = new[] {"OnionSword"},
                Slots = 3
            };
            return weapon;
        }

        [Fact]
        public void GetName()
        {
            var want = "OnionSword";
            var weapon = OnionSword();

            var got = Equipment.Equipment.GetEquipmentName(weapon);

            Assert.Equal(want, got);
        }

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