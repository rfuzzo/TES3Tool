using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.SCPT
{
    public class SCHD : Subrecord
    {
        /// <summary>
        /// Script name (31 characters + null termnator)
        /// </summary>
        public new string Name { get; set; }

        public int NumShorts { get; set; }

        public int NumLongs { get; set; }

        public int NumFloats { get; set; }

        public int ScriptDataSize { get; set; }

        public int LocalVarSize { get; set; }

        public SCHD()
        {
        }

        public SCHD(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            Name = reader.ReadBytes<string>(Data, 32);
            NumShorts = reader.ReadBytes<int>(Data);
            NumLongs = reader.ReadBytes<int>(Data);
            NumFloats = reader.ReadBytes<int>(Data);
            ScriptDataSize = reader.ReadBytes<int>(Data);
            LocalVarSize = reader.ReadBytes<int>(Data);
        }

        public override byte[] SerializeSubrecord()
        {
            List<byte> data = new();
            byte[] nameBytes = Encoding.ASCII.GetBytes(Name);
            Array.Resize(ref nameBytes, 32);

            data.AddRange(nameBytes);
            data.AddRange(ByteWriter.ToBytes(NumShorts, NumShorts.GetType()));
            data.AddRange(ByteWriter.ToBytes(NumLongs, NumLongs.GetType()));
            data.AddRange(ByteWriter.ToBytes(NumFloats, NumFloats.GetType()));
            data.AddRange(ByteWriter.ToBytes(ScriptDataSize, ScriptDataSize.GetType()));
            data.AddRange(ByteWriter.ToBytes(LocalVarSize, LocalVarSize.GetType()));

            var serialized = Encoding.ASCII.GetBytes("SCHD")
               .Concat(BitConverter.GetBytes(data.Count))
               .Concat(data).ToArray();
            return serialized;
        }
    }
}
