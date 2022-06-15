using TES3Lib.Base;
using TES3Lib.Enums;
using Utility;

namespace TES3Lib.Subrecords.INFO
{
    /// <summary>
    /// Info data (12 bytes)
    /// </summary>
    public class DATA : Subrecord
    {
        public int Unknown1 { get; private set; }

        public int Disposition { get; set; }

        /// <summary>
        /// between 0-10
        /// </summary>
        public byte Rank { get; set; }

        public GenderType Gender { get; set; }

        public byte PCRank { get; set; }

        public byte Unknown2 { get; private set; }

        public DATA()
        {
            Unknown1 = 0;
            Unknown2 = 2;
        }

        public DATA(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            Unknown1 = reader.ReadBytes<int>(Data);
            Disposition = reader.ReadBytes<int>(Data);
            Rank = reader.ReadBytes<byte>(Data);
            Gender = reader.ReadBytes<GenderType>(Data);
            PCRank = reader.ReadBytes<byte>(Data);
            Unknown2 = reader.ReadBytes<byte>(Data);
        }
    }
}
