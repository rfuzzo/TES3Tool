using System.Collections.Generic;
using TES3Lib.Base;
using TES3Lib.Enums;
using TES3Lib.Enums.Flags;
using Utility;

namespace TES3Lib.Subrecords.WEAP
{
    /// <summary>
    /// Weapon Data, 0x20 bytes binary, required
    /// </summary>
    public class WPDT : Subrecord
    {
        public float Weight { get; set; }

        public int Value { get; set; }

        public WeaponType Type { get; set; }

        public short Health { get; set; }

        public float Speed { get; set; }

        public float Reach { get; set; }

        public short EnchantmentPoints { get; set; }

        public byte ChopMin { get; set; }

        public byte ChopMax { get; set; }

        public byte SlashMin { get; set; }

        public byte SlashMax { get; set; }

        public byte ThrustMin { get; set; }

        public byte ThrustMax { get; set; }

        public HashSet<WeaponFlag> Flags { get; set; }

        public WPDT()
        {
            Flags = new HashSet<WeaponFlag>();
        }

        public WPDT(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            Weight = reader.ReadBytes<float>(Data);
            Value = reader.ReadBytes<int>(Data);
            Type = (WeaponType)reader.ReadBytes<short>(Data);
            Health = reader.ReadBytes<short>(Data);
            Speed = reader.ReadBytes<float>(Data);
            Reach = reader.ReadBytes<float>(Data);
            EnchantmentPoints = reader.ReadBytes<short>(Data);
            ChopMin = reader.ReadBytes<byte>(Data);
            ChopMax = reader.ReadBytes<byte>(Data);
            SlashMin = reader.ReadBytes<byte>(Data);
            SlashMax = reader.ReadBytes<byte>(Data);
            ThrustMin = reader.ReadBytes<byte>(Data);
            ThrustMax = reader.ReadBytes<byte>(Data);
            Flags = reader.ReadFlagBytes<WeaponFlag>(Data);
        }
    }
}
