﻿using System.Diagnostics;
using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.Shared
{
    /// <summary>
    /// Model path (32 characters max!)
    /// </summary>
    [DebuggerDisplay("{ModelPath}")]
    public class MODL : Subrecord
    {
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
