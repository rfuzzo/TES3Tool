using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.BSGN
{
    /// <summary>
    /// Birthsign graphic texture path
    /// </summary>
    public class TNAM : Subrecord, IStringView
    {
        public string Text
        {
            get => TexturePath;
            set => TexturePath = value;
        }

        public string TexturePath { get; set; }

        public TNAM()
        {
        }

        public TNAM(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            TexturePath = reader.ReadBytes<string>(Data, Size);
        }
    }
}
