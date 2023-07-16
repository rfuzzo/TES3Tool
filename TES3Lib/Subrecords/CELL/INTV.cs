using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.CELL
{
    /// <summary>
    /// Water height in cell
    /// Some plugins (Morrowind.esm) use an INTV subrecord in CELL header instead of WHGT
    /// </summary>
    public class INTV : Subrecord, IIntegerView
    {
        public int Value { get => WaterHeight; set => WaterHeight = value; }

        public int WaterHeight { get; set; }
        
        public INTV()
        {
        }

        public INTV(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            WaterHeight = reader.ReadBytes<int>(Data);
        }
    }
}