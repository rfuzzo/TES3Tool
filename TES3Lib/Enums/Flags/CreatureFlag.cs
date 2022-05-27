using System;

namespace TES3Lib.Enums.Flags
{
    public enum CreatureFlag : int
    {
        Biped = 0x0001,
        Respawn = 0x0002,
        WeaponAndShield = 0x0004,
        None = 0x0008,
        Swims = 0x0010,
        Flies = 0x0020,
        Walks = 0x0040,
        Essential = 0x0080,
        //SkeletonBlood = 0x0400,
        //MetalBlood = 0x0800
    }

    public enum BloodType : int
    {
        BloodType0 = 0x0000,
        BloodType1 = 0x0400,
        BloodType2 = 0x0800,
        BloodType3 = 0x0C00,
        BloodType4 = 0x1000,
        BloodType5 = 0x1400,
        BloodType6 = 0x1800,
        BloodType7 = 0x1C00,
    }

}
