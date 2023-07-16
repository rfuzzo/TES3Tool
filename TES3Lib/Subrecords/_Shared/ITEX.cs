using System.Diagnostics;
using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.Shared
{
    /// <summary>
    /// Icon path (32 characters max!)
    /// </summary>
    [DebuggerDisplay("{IconPath}")]
    public class ITEX : Subrecord, IStringView
    {
        public string Text
        {
            get => IconPath;
            set => IconPath = value;
        }

        public string IconPath { get; set; }

        public ITEX()
        {
        }

        public ITEX(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            IconPath = reader.ReadBytes<string>(Data, Size);
        }
    }
}
