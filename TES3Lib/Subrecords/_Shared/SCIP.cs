﻿using System.Diagnostics;
using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.Shared
{
    /// <summary>
    /// Script
    /// </summary>
    [DebuggerDisplay("{ScriptName}")]
    public class SCIP : Subrecord
    {
        public string ScriptName { get; set; }

        public SCIP()
        {

        }

        public SCIP(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            ScriptName = reader.ReadBytes<string>(Data, Size);
        }
    }
}
