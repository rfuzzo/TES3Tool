using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.PGRD
{
    public class PGRP : Subrecord
    {
        public Point[] Points { get; set; }

        public PGRP()
        {

        }

        public PGRP(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            int nodeCount = Data.Length / 16;
            Points = new Point[nodeCount];

            for (int i = 0; i < nodeCount; i++)
            {
                Points[i].X = reader.ReadBytes<int>(Data);
                Points[i].Y = reader.ReadBytes<int>(Data);
                Points[i].Z = reader.ReadBytes<int>(Data);
                Points[i].IsUserPoint = reader.ReadBytes<byte>(Data);
                Points[i].EdgeCount = reader.ReadBytes<byte>(Data);
                Points[i].Unknown1 = reader.ReadBytes<byte>(Data);
                Points[i].Unknown2 = reader.ReadBytes<byte>(Data);
            }

        }

        public struct Point
        {
            public int X;
            public int Y;
            public int Z;
            public byte IsUserPoint;
            public byte EdgeCount;
            public byte Unknown1;
            public byte Unknown2;
        }

        public override byte[] SerializeSubrecord()
        {
            List<byte> data = new();
            foreach (Point gridNode in Points)
            {
                data.AddRange(ByteWriter.ToBytes(gridNode.X, typeof(int)));
                data.AddRange(ByteWriter.ToBytes(gridNode.Y, typeof(int)));
                data.AddRange(ByteWriter.ToBytes(gridNode.Z, typeof(int)));
                data.Add(gridNode.IsUserPoint);
                data.Add(gridNode.EdgeCount);
                data.Add(gridNode.Unknown1);
                data.Add(gridNode.Unknown2);
            }

            var serialized = Encoding.ASCII.GetBytes(GetType().Name)
               .Concat(BitConverter.GetBytes(data.Count))
               .Concat(data).ToArray();
            return serialized;
        }
    }
}
