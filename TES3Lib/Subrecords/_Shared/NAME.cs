using System.Diagnostics;
using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.Shared
{
    /// <summary>
    /// Editor Id
    /// </summary>
    [DebuggerDisplay("{EditorId}")]
    public class NAME : Subrecord, IStringView
    {
        public string Text
        {
            get => EditorId;
            set => EditorId = value;
        }

        public string EditorId { get; set; }
       

        public NAME()
        {

        }

        public NAME(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            EditorId = reader.ReadBytes<string>(Data, Size);
        }
    }
}
