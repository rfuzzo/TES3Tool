using System.Diagnostics;
using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.Shared
{
    /// <summary>
    /// Description
    /// </summary>
    [DebuggerDisplay("{Description}")]
    public class DESC : Subrecord, IStringView
    {
        public string Text
        {
            get => Description;
            set => Description = value;
        }


        /// <summary>
        /// Text description
        /// </summary>
        public string Description { get; set; }

        public DESC()
        {

        }

        public DESC(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            Description = reader.ReadBytes<string>(Data, Size);
        }
    }
}