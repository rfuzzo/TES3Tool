using System.Diagnostics;
using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.INFO
{
    /// <summary>
    /// Previous info ID
    /// </summary>
    [DebuggerDisplay("{PreviousInfoId}")]
    public class PNAM : Subrecord, IStringView
    {
        public string Text
        {
            get => PreviousInfoId;
            set => PreviousInfoId = value;
        }
        public string PreviousInfoId { get; set; }

        public PNAM()
        {
        }

        public PNAM(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            PreviousInfoId = reader.ReadBytes<string>(Data, Size);
        }
    }
}
