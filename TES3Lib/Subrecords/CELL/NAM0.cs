using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.CELL
{
    /// <summary>
    /// Reference count
    /// </summary>
    public class NAM0 : Subrecord, IIntegerView
    {
        public int Value { get => ReferenceCount; set => ReferenceCount = value; }
        public int ReferenceCount { get; set; }

        public NAM0()
        {
        }

        public NAM0(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            ReferenceCount = reader.ReadBytes<int>(Data);
        }
    }
}
