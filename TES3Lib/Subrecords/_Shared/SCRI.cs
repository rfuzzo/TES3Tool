using System.Diagnostics;
using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.Shared
{
    /// <summary>
    /// Script
    /// </summary>
    [DebuggerDisplay("{ScriptName}")]
    public class SCRI : Subrecord, IStringView
    {
        public string Text
        {
            get => ScriptName;
            set => ScriptName = value;
        }

        public string ScriptName { get; set; }

        public SCRI()
        {
        }

        public SCRI(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            ScriptName = reader.ReadBytes<string>(Data, Size);
        }
    }
}
