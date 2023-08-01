using System.Collections.Generic;
using TES3Lib.Base;
using TES3Lib.Enums.Flags;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.ALCH
{
    /// <summary>
    /// Alchemy data
    /// </summary>
    public class ALDT : Subrecord, IDataView
    {
        public float Weight { get; set; }

        public int Value { get; set; }

        /// <summary>
        /// Possible values 0 or 1
        /// </summary>
        public AlchemyFlag Flags { get; set; } //?

        public ALDT()
        {
        }

        public ALDT(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            Weight = reader.ReadBytes<float>(Data);
            Value = reader.ReadBytes<int>(Data);
            Flags = reader.ReadBytes<AlchemyFlag>(Data);
        }

        public Dictionary<string, object> GetData() => new()
        {
            {nameof(Weight), Weight },
            {nameof(Value), Value }
        };

    }
}
