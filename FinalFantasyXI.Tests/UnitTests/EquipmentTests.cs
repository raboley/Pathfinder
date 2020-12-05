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
            var want = new EquipmentItem
            {
                Name = "OnionSword",
                Level = 1,
                EquipAbleInSlots = new List<EquipSlots> {EquipSlots.Main, EquipSlots.Sub},
                Skill = SkillType.Sword,
                EquipAbleByJobs = new List<Job>
                {
                    Job.Warrior,
                    Job.RedMage,
                    Job.Thief,
                    Job.Paladin,
                    Job.DarkKnight,
                    Job.BeastMaster,
                    Job.Bard,
                    Job.Ranger,
                    Job.Ninja,
                    Job.Dragoon,
                    Job.BlueMage,
                    Job.Corsair,
                    Job.RuneFencer
                }
            };

            var weapon = OnionSword();
            var got = Equipment.Equipment.GetEquipmentFromItem(weapon);

            EquipmentItemEquals(want, got);
        }

        public static void EquipmentItemEquals(IEquipmentItem want, IEquipmentItem got)
        {
            Assert.Equal(want.Name, got.Name);
            Assert.Equal(want.EquipAbleInSlots, got.EquipAbleInSlots);
            Assert.Equal(want.Skill, got.Skill);
            Assert.Equal(want.EquipAbleByJobs, got.EquipAbleByJobs);
            Assert.Equal(want.Level, got.Level);
        }

        private static IItem OnionSword()
        {
            var weapon = new IItem
            {
                Name = new[] {"OnionSword"},
                Slots = 3,
                Skill = 3,
                Jobs = 4419554,
                Level = 1
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

        [Fact]
        public void GetWeaponTypeCanGetWeaponType()
        {
            var want = SkillType.Sword;

            var weapon = OnionSword();
            var got = Equipment.Equipment.GetWeaponType(weapon);

            Assert.Equal(want, got);
        }

        [Fact]
        public void GetJobs()
        {
            var want = new List<Job>
            {
                Job.Warrior,
                Job.RedMage,
                Job.Thief,
                Job.Paladin,
                Job.DarkKnight,
                Job.BeastMaster,
                Job.Bard,
                Job.Ranger,
                Job.Ninja,
                Job.Dragoon,
                Job.BlueMage,
                Job.Corsair,
                Job.RuneFencer
            };

            var weapon = OnionSword();
            var got = Equipment.Equipment.GetJobs(weapon);


            Assert.Equal(want, got);
        }
    }
}