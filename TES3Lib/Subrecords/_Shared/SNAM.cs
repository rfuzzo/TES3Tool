using System.Diagnostics;
using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.Shared
{
    /// <summary>
    /// EditorId of sound reference
    /// </summary>
    [DebuggerDisplay("{EditorId}")]
    public class SNAM : Subrecord
    {
        public string SoundEditorId { get; set; }

        public SNAM()
        {
        }

        public SNAM(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            SoundEditorId = reader.ReadBytes<string>(Data, Size);
        }
    }
}
