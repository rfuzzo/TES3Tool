using System.Diagnostics;
using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.Shared
{
    /// <summary>
    /// Model path (32 characters max!)
    /// </summary>
    [DebuggerDisplay("{ModelPath}")]
    public class MODL : Subrecord, IStringView
    {
        public string Text
        {
            get => ModelPath;
            set => ModelPath = value;
        }
        public string ModelPath { get; set; }

        public MODL()
        {
        }

        public MODL(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            ModelPath = reader.ReadBytes<string>(Data, Size);
        }
    }
}
