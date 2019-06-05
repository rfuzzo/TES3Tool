using System.Diagnostics;
using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.Shared
{
    /// <summary>
    /// Display name of object
    /// Used in BODY record to assign body part to race
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class FNAM : Subrecord
    {
        public string FileName { get; set; }

        public FNAM()
        {
        }

        public FNAM(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            FileName = reader.ReadBytes<string>(base.Data, base.Size);
        }
    }
}