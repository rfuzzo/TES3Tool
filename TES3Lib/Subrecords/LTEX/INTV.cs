using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.LTEX
{
    /// <summary>
    /// Not realy know, numbers seem increment for next entries from 0
    /// </summary>
    public class INTV : Subrecord, IIntegerView
    {
        public int Value { get => IndexNumber; set => IndexNumber = value; }
        public int IndexNumber { get; set; }

        public INTV()
        {
        }

        public INTV(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            IndexNumber = reader.ReadBytes<int>(Data);
        }
    }
}
