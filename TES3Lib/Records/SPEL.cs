using System.Collections.Generic;
using System.Diagnostics;
using TES3Lib.Base;
using TES3Lib.Subrecords.Shared;
using TES3Lib.Subrecords.Shared.Castable;
using TES3Lib.Subrecords.SPEL;

namespace TES3Lib.Records
{
    [DebuggerDisplay("{NAME.EditorId}")]
    public class SPEL : Record
    {
        public NAME NAME { get; set; }

        public FNAM FNAM { get; set; }

        public SPDT SPDT { get; set; }

        public List<ENAM> ENAM { get; set; }

        public SPEL()
        {
        }

        public SPEL(byte[] rawData) : base(rawData)
        {
            BuildSubrecords();
        }
    }
}
