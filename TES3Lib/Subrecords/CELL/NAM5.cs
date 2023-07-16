using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.CELL
{
    /// <summary>
    /// Map color
    /// </summary>
    public class NAM5 : Subrecord, IIntegerView
    {
        public int Value { get => MapColor; set => MapColor = value; }

        public int MapColor { get; set; }

        public NAM5()
        {
        }

        public NAM5(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            MapColor = reader.ReadBytes<int>(Data);
        }
    }
}
