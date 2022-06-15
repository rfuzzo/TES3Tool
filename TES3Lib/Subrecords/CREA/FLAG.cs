using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TES3Lib.Base;
using TES3Lib.Enums.Flags;
using Utility;

namespace TES3Lib.Subrecords.CREA
{
    /// <summary>
    /// Creature flags
    /// </summary>
    public class FLAG : Subrecord
    {
        public HashSet<CreatureFlag> Flags { get; set; }
        public BloodType BloodType { get; set; }

        public FLAG()
        {
            Flags = new HashSet<CreatureFlag>();
        }

        public FLAG(byte[] rawData) : base(rawData)
        {
            (Flags, BloodType) = ReadFlagBytes(Data);
        }

        private (HashSet<CreatureFlag>, BloodType) ReadFlagBytes(byte[] data)
        {
            var converted = BitConverter.ToUInt32(data, 0);

            if (converted.Equals(0))
            {
                return (new HashSet<CreatureFlag>(), BloodType.BloodType0);
            }

            // first read the flags
            var setOfEnum = new HashSet<CreatureFlag>();
            uint enumFlag = 0;
            foreach (var enumVal in typeof(CreatureFlag).GetEnumValues())
            {
                var flag = Convert.ToUInt32(enumVal);
                if ((converted & flag) == flag)
                {
                    setOfEnum.Add((CreatureFlag)Enum.ToObject(typeof(CreatureFlag), enumVal));
                    enumFlag |= flag;
                }
            }

            // then parse the masked enum
            var blood = BloodType.BloodType0;
            if (enumFlag != converted)
            {
                blood = (BloodType)(converted & 0x1C00);
            }

            return (setOfEnum, blood);

        }

        public override byte[] SerializeSubrecord()
        {
            if (!IsImplemented)
            {
                return GetRawLoadedBytes();
            }

            List<byte> data = new();

            // Flags and Blood
            var enumval = SerializeFlag(Flags) | (uint)BloodType;

            data.AddRange(ByteWriter.ToBytes(enumval, typeof(uint)));

            var serialized = Encoding.ASCII.GetBytes(GetType().Name)
               .Concat(BitConverter.GetBytes(data.Count))
               .Concat(data).ToArray();
            return serialized;
        }
    }
}
