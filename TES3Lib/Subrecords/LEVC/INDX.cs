using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.LEVC
{
    /// <summary>
    ///  Number of items in list
    /// </summary>
    public class INDX : Subrecord, IIntegerView
    {
        public int Value { get => CreatureCount; set => CreatureCount = value; }
        public int CreatureCount { get; set; }

        public INDX()
        {
        }

        public INDX(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            CreatureCount = reader.ReadBytes<int>(Data, Size);
        }
    }
}
