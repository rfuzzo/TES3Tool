﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.FACT
{
    public class RNAM : Subrecord
    {
        /// <summary>
        /// Always 32 bytes padded with junk data if less
        /// </summary>
        public string RankName { get; set; }

        public RNAM()
        {
        }

        public RNAM(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            RankName = reader.ReadBytes<string>(Data, Size);
        }

        public override byte[] SerializeSubrecord()
        {
            List<byte> data = new();

            byte[] ranknameBytes = Encoding.ASCII.GetBytes(RankName);
            Array.Resize(ref ranknameBytes, 32);
            data.AddRange(ranknameBytes);

            var serialized = Encoding.ASCII.GetBytes(GetType().Name)
               .Concat(BitConverter.GetBytes(data.Count))
               .Concat(data).ToArray();
            return serialized;
        }
    }
}
