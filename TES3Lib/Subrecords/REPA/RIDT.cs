using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.REPA
{
    /// <summary>
    /// Repair item data
    /// </summary>
    public class RIDT : Subrecord
    {
        public float Weight { get; set; }

        public int Value { get; set; }

        public int Uses { get; set; }

        public float Quality { get; set; }

        public RIDT()
        {
        }

        public RIDT(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            Weight = reader.ReadBytes<float>(Data);
            Value = reader.ReadBytes<int>(Data);
            Uses = reader.ReadBytes<int>(Data);
            Quality = reader.ReadBytes<float>(Data);
        }
    }
}
