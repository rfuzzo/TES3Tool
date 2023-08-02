using TES3Lib.Base;
using TES3Lib.Subrecords.MGEF;
using TES3Lib.Subrecords.Shared;
using static Utility.Common;

namespace TES3Lib.Records
{
    public class MGEF : Record
    {
        public INDX INDX { get; set; }

        public MEDT MEDT { get; set; }

        public ITEX ITEX { get; set; }

        public PTEX PTEX { get; set; }

        public CVFX CVFX { get; set; }

        public BVFX BVFX { get; set; }

        public HVFX HVFX { get; set; }

        public AVFX AVFX { get; set; }

        public DESC DESC { get; set; }

        public CSND CSND { get; set; }

        public BSND BSND { get; set; }

        public HSND HSND { get; set; }

        public ASND ASND { get; set; }

        public MGEF(byte[] rawData) : base(rawData)
        {
            BuildSubrecords();
        }

        public MGEF()
        {
        }

        public override string GetEditorId()
        {
            return INDX is not null ? $"MagicEffect#{INDX.EffectId}" : null;
        }
    }
}
