using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.ALCH
{
    /// <summary>
    /// Icon path (32 characters max!)
    /// </summary>
    public class TEXT : Subrecord, IStringView
    {
        public string Text
        {
            get => IconPath;
            set => IconPath = value;
        }

        public string IconPath { get; set; }

        public TEXT()
        {
        }

        public TEXT(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            IconPath = reader.ReadBytes<string>(Data, Size);
        }
    }
}
