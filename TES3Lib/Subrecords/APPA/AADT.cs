using TES3Lib.Base;
using TES3Lib.Enums;
using Utility;

namespace TES3Lib.Subrecords.APPA
{
    public class AADT : Subrecord
    {
        public ApparatusType Type { get; set; }

        public float Quality { get; set; }

        public float Weight { get; set; }

        public int Value { get; set; }

        public AADT()
        {
        }

        public AADT(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            Type = reader.ReadBytes<ApparatusType>(Data);
            Quality = reader.ReadBytes<float>(Data);
            Weight = reader.ReadBytes<float>(Data);
            Value = reader.ReadBytes<int>(Data);
        }
    }
}
