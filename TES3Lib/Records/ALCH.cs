﻿using System.Collections.Generic;
using TES3Lib.Base;
using TES3Lib.Subrecords.ALCH;
using TES3Lib.Subrecords.Shared;

namespace TES3Lib.Records
{
    public class ALCH : Record
    {
        public NAME NAME { get; set; }

        public MODL MODL { get; set; }

        public FNAM FNAM { get; set; }

        public ALDT ALDT { get; set; }

        public List<ENAM> ENAM { get; set; }

        public TEXT TEXT { get; set; }

        public SCRI SCRI { get; set; }

        public ALCH()
        {
        }

        public ALCH(byte[] rawData) : base(rawData)
        {
            BuildSubrecords();
        }
    }
}
