using System.Diagnostics;
using TES3Lib.Base;
using TES3Lib.Subrecords.GLOB;
using TES3Lib.Subrecords.Shared;
using FNAM = TES3Lib.Subrecords.GLOB.FNAM;

namespace TES3Lib.Records
{
    /// <summary>
    /// Global variable record
    /// </summary>
    [DebuggerDisplay("{NAME.EditorId}")]
    public class GLOB : Record
    {
        /// <summary>
        /// EditorId
        /// </summary>
        public NAME NAME { get; set; }

        /// <summary>
        /// Global type
        /// </summary>
        public FNAM FNAM { get; set; }

        /// <summary>
        /// Float value
        /// </summary>
        public FLTV FLTV { get; set; }

        public GLOB()
        {
        }

        public GLOB(byte[] rawData) : base(rawData)
        {
            BuildSubrecords();
        }
    }
}
