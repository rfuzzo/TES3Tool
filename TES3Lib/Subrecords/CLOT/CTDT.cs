using TES3Lib.Base;
using TES3Lib.Enums;
using Utility;

namespace TES3Lib.Subrecords.CLOT
{
    /// <summary>
    /// Clothing Data
    /// </summary>
    public class CTDT : Subrecord
    {
        public ClothingType Type { get; set; }

        public float Weight { get; set; }

        public short Value { get; set; }

        public short EnchancementPoints { get; set; }

        public CTDT()
        {
        }

        public CTDT(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            Type = reader.ReadBytes<ClothingType>(Data);
            Weight = reader.ReadBytes<float>(Data);
            Value = reader.ReadBytes<short>(Data);
            EnchancementPoints = reader.ReadBytes<short>(Data);
        }
    }
}
