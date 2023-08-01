﻿using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.CELL
{
    public class RGNN : Subrecord
    {
        /// <summary>
        /// Cells region name
        /// </summary>
        public string RegionName { get; set; }

        public RGNN()
        {

        }

        public RGNN(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            RegionName = reader.ReadBytes<string>(Data, Size);
        }
    }
}
