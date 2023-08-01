﻿using System.Diagnostics;
using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.ARMO
{
    /// <summary>
    /// Female part name
    /// </summary>
    [DebuggerDisplay("{FemalePartName}")]
    public class CNAM : Subrecord
    {
        /// <summary>
        /// Female tagged bodpart id
        /// </summary>
        public string FemalePartName { get; set; }

        public CNAM()
        {

        }

        public CNAM(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            FemalePartName = reader.ReadBytes<string>(Data, Size);
        }
    }
}
