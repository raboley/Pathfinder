using System.Collections.Generic;
using FinalFantasyXI.Equipment;
using Xunit;

namespace FinalFantasyXI.Tests.UnitTests
{
    public class EquipmentManagerTests
    {
        [Fact(Skip = "Need to do equipable by job, and weapon type, and then pick best by damage or something.")]
        public void GetBestItemPicksTheHighestForTheLevel()
        {
            var player = TestLowLevelPlayer();

            var want = SteelSword();

            var equipmentList = TestEquipmentBag();

            var got = Equipment.EquipmentManager.GetBestEquipmentItemForSlot(EquipSlots.Main, equipmentList);

            Assert.Equal(want, got);
        }

        private static Player TestLowLevelPlayer()
        {
            return new Player
            {
                Job = Job.Warrior,
                Level = 10
            };
        }

        private static EquipmentItem SteelSword()
        {
            return new Equipment.EquipmentItem
            {
                Name = "Steel Sword",
                Level = 10,
                EquipAbleInSlots = new List<EquipSlots>
                {
                    EquipSlots.Sub,
                    EquipSlots.Main
                },
                EquipAbleByJobs = new List<Job>
                {
                    Job.Warrior
                }
            };
        }

        [Fact]
        public void GetHighestValidLevelItems()
        {
            var want = TestEquipmentBag();
            want.RemoveAll(x => x.Name == "Excalibur");

            var player = TestLowLevelPlayer();
            var equipmentBag = TestEquipmentBag();

            var got = Equipment.Equipment.GetEquipmentValidForLevel(player, equipmentBag);

            AssertEquipmentListEqual(want, got);
        }

        private static void AssertEquipmentListEqual(List<IEquipmentItem> want, List<IEquipmentItem> got)
        {
            Assert.Equal(want.Count, got.Count);
            for (int i = 0; i < want.Count; i++)
            {
                EquipmentTests.EquipmentItemEquals(want[i], got[i]);
            }
        }

        [Fact]
        public void GetEquipableInSlotItems()
        {
            var want = TestEquipmentBag();
            want.RemoveAll(x => x.Name == "Helmet");

            var got = Equipment.Equipment.GetEquipmentValidForSlot(EquipSlots.Main, TestEquipmentBag());

            AssertEquipmentListEqual(want, got);
        }

        private static List<IEquipmentItem> TestEquipmentBag()
        {
            var equipmentList = new List<IEquipmentItem>
            {
                new Equipment.EquipmentItem
                {
                    Name = "Excalibur",
                    Level = 90,
                    EquipAbleInSlots = new List<EquipSlots>
                    {
                        EquipSlots.Sub,
                        EquipSlots.Main
                    },
                    EquipAbleByJobs = new List<Job>
                    {
                        Job.Warrior
                    }
                },
                new Equipment.EquipmentItem
                {
                    Name = "Onion Sword",
                    Level = 1,
                    EquipAbleInSlots = new List<EquipSlots>
                    {
                        EquipSlots.Sub,
                        EquipSlots.Main
                    },
                    EquipAbleByJobs = new List<Job>
                    {
                        Job.Warrior
                    }
                },
                SteelSword(),
                new Equipment.EquipmentItem
                {
                    Name = "Helmet",
                    Level = 5,
                    EquipAbleInSlots = new List<EquipSlots>
                    {
                        EquipSlots.Head
                    },
                    EquipAbleByJobs = new List<Job>
                    {
                        Job.Warrior
                    }
                },
            };
            return equipmentList;
        }
    }
}