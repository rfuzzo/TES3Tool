using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.FACT
{
    /// <summary>
    /// Faction reaction value
    /// </summary>
    public class INTV : Subrecord, IIntegerView
    {
        public int Value { get => FactionReactionValue; set => FactionReactionValue = value; }
        public int FactionReactionValue { get; set; }

        public INTV()
        {
        }

        public INTV(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            FactionReactionValue = reader.ReadBytes<int>(Data);
        }
    }
}
