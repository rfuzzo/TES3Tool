using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.REGN
{
    public class CNAM : Subrecord
    {
        public byte Red { get; set; }

        public byte Green { get; set; }

        public byte Blue { get; set; }

        public byte Null { get; set; }

        public CNAM()
        {
        }

        public CNAM(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            Red = reader.ReadBytes<byte>(Data);
            Green = reader.ReadBytes<byte>(Data);
            Blue = reader.ReadBytes<byte>(Data);
            Null = reader.ReadBytes<byte>(Data);
        }
    }
}
