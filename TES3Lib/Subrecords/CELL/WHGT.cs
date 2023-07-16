using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.CELL
{
    /// <summary>
    /// Interior only
    /// Contains water level in cell
    /// </summary>
    public class WHGT : Subrecord, IFloatView
    {
        public float Value { get => WaterHeight; set => WaterHeight = value; }
        public float WaterHeight { get; set; }

        public WHGT()
        {
        }

        public WHGT(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            WaterHeight = reader.ReadBytes<float>(Data);
        }
    }
}
