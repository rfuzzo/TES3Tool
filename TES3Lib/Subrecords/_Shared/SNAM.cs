using System.Diagnostics;
using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.Shared
{
    /// <summary>
    /// EditorId of sound reference
    /// </summary>
    [DebuggerDisplay("{EditorId}")]
    public class SNAM : Subrecord, IStringView
    {
        public string Text
        {
            get => SoundEditorId;
            set => SoundEditorId = value;
        }
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
