using TES3Lib.Base;
using TES3Lib.Subrecords.Shared;
using TES3Lib.Subrecords.SKILL;
using static Utility.Common;

namespace TES3Lib.Records
{
    public class SKIL : Record
    {
        public INDX INDX { get; set; }

        public SKDT SKDT { get; set; }

        public DESC DESC { get; set; }

        public SKIL(byte[] rawData) : base(rawData)
        {
            BuildSubrecords();
        }

        public SKIL()
        {
        }

        public override string GetEditorId()
        {
            return INDX is not null ? $"Skill#{INDX.Skill}" : null;
        }
    }
}
