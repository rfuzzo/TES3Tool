﻿using System.Diagnostics;
using TES3Lib.Base;
using TES3Lib.Subrecords.REGN;
using TES3Lib.Subrecords.Shared;
using SNAM = TES3Lib.Subrecords.REGN.SNAM;

namespace TES3Lib.Records
{
    [DebuggerDisplay("{NAME.EditorId}")]
    public class REGN : Record
    {
        public NAME NAME { get; set; }

        public FNAM FNAM { get; set; }

        public WEAT WEAT { get; set; }

        /// <summary>
        /// Id of creature that wokes player when resting
        /// </summary>
        public BNAM BNAM { get; set; }

        public CNAM CNAM { get; set; }

        public SNAM SNAM { get; set; }

        public REGN(byte[] rawData) : base(rawData)
        {
            BuildSubrecords();
        }

        public REGN()
        {
        }
    }
}
