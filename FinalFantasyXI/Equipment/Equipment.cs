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
    public enum Job : byte
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

    public static class Equipment
    {
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
    }

    public class EquipmentItem
    {
        public List<EquipSlots> EquipAbleInSlots { get; set; } = new List<EquipSlots>();

        // public List<Job> EquipAbleByJobs { get; set; } = new List<Job>();
        public string Name { get; set; }
    }
}