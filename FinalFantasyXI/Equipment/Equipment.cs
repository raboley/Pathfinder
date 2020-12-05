using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace FinalFantasyXI.Equipment
{
    [StructLayout(LayoutKind.Sequential)]
    public class IItem
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2424)]
        public byte[] Bitmap;

        public ushort CastDelay;
        public ushort CastTime;
        public ushort Damage;
        public ushort Delay;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.LPStr)]
        public string[] Description;

        public ushort DPS;
        public ushort Flags;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] ImageName;

        public uint ImageSize;
        public byte ImageType;
        public ushort InstinctCost;
        public uint ItemID;
        public ushort ItemLevel;
        public ushort ItemType;
        public uint Jobs;
        public byte JugSize;
        public ushort Level;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.LPStr)]
        public string[] LogNamePlural;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.LPStr)]
        public string[] LogNameSingular;

        public byte MaxCharges;
        public ushort MonipulatorID;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] MonipulatorName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.LPStr)]
        public string[] Name;

        public uint PuppetElements;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.Struct)]
        // public EliteAPI.MonAbility[] MonipulatorAbilities;
        public ushort PuppetSlot;

        public ushort Races;
        public uint RecastDelay;
        public ushort ResourceID;
        public ushort ShieldSize;
        public byte Skill;
        public ushort Slots;
        public ushort StackSize;
        public byte SuperiorLevel;
        public ushort ValidTargets;
    }

    /// <summary>
    /// Equipment slot ids for equiped weapons and armor referenced by the api
    /// </summary>
    public enum EquipSlots : int
    {
        Main = 0,
        Sub = 1,
        Range = 2,
        Ammo = 3,
        Head = 4,
        Body = 5,
        Hands = 6,
        Legs = 7,
        Feet = 8,
        Neck = 9,
        Waist = 10,
        LEar = 11,
        REar = 12,
        LRing = 13,
        RRing = 14,
        Back = 15,
    }

    /// <summary>
    /// Jobs
    /// </summary>
    public enum Job
    {
        None = 0,
        Warrior = 1,
        Monk = 2,
        WhiteMage = 3,
        BlackMage = 4,
        RedMage = 5,
        Thief = 6,
        Paladin = 7,
        DarkKnight = 8,
        BeastMaster = 9,
        Bard = 10,
        Ranger = 11,
        Samurai = 12,
        Ninja = 13,
        Dragoon = 14,
        Summon = 15,
        BlueMage = 16,
        Corsair = 17,
        PuppetMaster = 18,
        Dancer = 19,
        Scholar = 20,
        Geomancer = 21,
        RuneFencer = 22
    }

    public enum JobMask : uint
    {
        None = 0,
        WAR = 2,
        MNK = 4,
        WHM = 8,
        BLM = 16, // 0x00000010
        RDM = 32, // 0x00000020
        THF = 64, // 0x00000040
        PLD = 128, // 0x00000080
        DRK = 256, // 0x00000100
        BST = 512, // 0x00000200
        BRD = 1024, // 0x00000400
        RNG = 2048, // 0x00000800
        SAM = 4096, // 0x00001000
        NIN = 8192, // 0x00002000
        DRG = 16384, // 0x00004000
        SMN = 32768, // 0x00008000
        BLU = 65536, // 0x00010000
        COR = 131072, // 0x00020000
        PUP = 262144, // 0x00040000
        DNC = 524288, // 0x00080000
        SCH = 1048576, // 0x00100000
        GEO = 2097152, // 0x00200000
        RUN = 4194304, // 0x00400000
        AllJobs = 8388606, // 0x007FFFFE
        MON = 8388608, // 0x00800000
        JOB24 = 16777216, // 0x01000000
        JOB25 = 33554432, // 0x02000000
        JOB26 = 67108864, // 0x04000000
        JOB27 = 134217728, // 0x08000000
        JOB28 = 268435456, // 0x10000000
        JOB29 = 536870912, // 0x20000000
        JOB30 = 1073741824, // 0x40000000
        JOB31 = 2147483648, // 0x80000000
    }

    public static class Equipment
    {
        public static EquipmentItem GetEquipmentFromItem(IItem weapon)
        {
            var equipment = new EquipmentItem
            {
                Name = GetEquipmentName(weapon),
                EquipAbleInSlots = GetEquipSlots(weapon),
                Skill = GetWeaponType(weapon),
                EquipAbleByJobs = GetJobs(weapon),
                Level = weapon.Level
            };

            return equipment;
        }


        public static List<EquipSlots> GetEquipSlots(IItem item)
        {
            var slotsCanEquip = new List<EquipSlots>();

            foreach (var slotInt in Enum.GetValues(typeof(EquipSlots)).Cast<int>())
            {
                if (CanEquipOnSlot(item, slotInt))
                    slotsCanEquip.Add((EquipSlots) slotInt);
            }

            return slotsCanEquip;
        }

        public static bool CanEquipOnSlot(IItem item, int Slot)
        {
            int slotsThaCanBeEquipToBitMask = (int) item.Slots;
            var canEquip = slotsThaCanBeEquipToBitMask & (uint) (Math.Pow(2, Slot));
            if (canEquip == 0)
                return false;

            return true;
        }


        public static string GetEquipmentName(IItem weapon)
        {
            return weapon.Name[0];
        }

        public static SkillType GetWeaponType(IItem weapon)
        {
            return (SkillType) weapon.Skill;
        }

        public static List<Job> GetJobs(IItem item)
        {
            var jobs = new List<Job>();

            foreach (var slotInt in Enum.GetValues(typeof(Job)).Cast<int>())
            {
                if (CanJobEquipItem((byte) slotInt, item))
                    jobs.Add((Job) slotInt);
            }

            return jobs;
        }

        private static bool CanJobEquipItem(byte Job, IItem Item)
        {
            return CanJobEquipItem((uint) Math.Pow(2, Job), Item);
        }

        private static bool CanJobEquipItem(uint JobMask, IItem Item)
        {
            return ((Item.Jobs & JobMask) != 0);
        }

        public static int GetLevel(IItem weapon)
        {
            throw new NotImplementedException();
        }
    }

    public class EquipmentItem
    {
        public List<EquipSlots> EquipAbleInSlots { get; set; } = new List<EquipSlots>();

        // public List<Job> EquipAbleByJobs { get; set; } = new List<Job>();
        public string Name { get; set; }
        public SkillType Skill { get; set; }
        public IEnumerable<Job> EquipAbleByJobs { get; set; }
        public int Level { get; set; }
    }

    public enum SkillType
    {
        HandToHand = 1,
        Dagger = 2,
        Sword = 3,
        GreatSword = 4,
        Axe = 5,
        GreatAxe = 6,
        Scythe = 7,
        Polarm = 8,
        Katana = 9,
        GreatKatana = 10, // 0x0000000A
        Club = 11, // 0x0000000B
        Staff = 12, // 0x0000000C
        Archery = 25, // 0x00000019
        Marksmanship = 26, // 0x0000001A
        Throwing = 27, // 0x0000001B
        Guard = 28, // 0x0000001C
        Evasion = 29, // 0x0000001D
        Shield = 30, // 0x0000001E
        Parry = 31, // 0x0000001F
        Divine = 32, // 0x00000020
        Healing = 33, // 0x00000021
        Enhancing = 34, // 0x00000022
        Enfeebling = 35, // 0x00000023
        Elemental = 36, // 0x00000024
        Dark = 37, // 0x00000025
        Summoning = 38, // 0x00000026
        Ninjitsu = 39, // 0x00000027
        Singing = 40, // 0x00000028
        String = 41, // 0x00000029
        Wind = 42, // 0x0000002A
        BlueMagic = 43, // 0x0000002B
        Fishing = 48, // 0x00000030
        Woodworking = 49, // 0x00000031
        Smithing = 50, // 0x00000032
        Goldsmithing = 51, // 0x00000033
        Clothcraft = 52, // 0x00000034
        Leathercraft = 53, // 0x00000035
        Bonecraft = 54, // 0x00000036
        Alchemy = 55, // 0x00000037
        Cooking = 56, // 0x00000038
        Synergy = 57, // 0x00000039
        ChocoboDigging = 58, // 0x0000003A
    }
}