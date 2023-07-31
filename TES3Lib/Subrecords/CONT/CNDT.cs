using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.CONT
{
    /// <summary>
    /// Container weight
    /// </summary>
    public class CNDT : Subrecord, IFloatView
    {
        public float Weight { get; set; }
        public float Value { get => Weight; set => Weight = value; }
        public CNDT()
        {

        }

        public CNDT(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            Weight = reader.ReadBytes<float>(Data);
        }
    }
}
