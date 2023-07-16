using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.BOOK
{
    public class TEXT : Subrecord, IStringView
    {
        public string Text
        {
            get => BookText;
            set => BookText = value;
        }

        public string BookText { get; set; }

        public TEXT()
        {

        }

        public TEXT(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            BookText = reader.ReadBytes<string>(Data, Size);
        }
    }
}