﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.LAND
{
    /// <summary>
    /// Vertex normals
    /// A RGB color map 65x65 pixels in size representing the land normal vectors.
    /// The signed value of the 'color' represents the vector's component. Blue
	/// is vertical(Z), Red the X direction and Green the Y direction.Note that
    /// the y-direction of the data is from the bottom up.
    /// </summary>
    public class VNML : Subrecord
    {
        const int size = 65;

        public Normal[,] normals { get; set; }

        public VNML()
        {
        }

        public VNML(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            normals = new Normal[size, size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    var bytes = reader.ReadBytes<byte[]>(Data, 3);
                    normals[y, x].x = bytes[0];
                    normals[y, x].y = bytes[1];
                    normals[y, x].z = bytes[2];
                }
            }
        }

        public override byte[] SerializeSubrecord()
        {
            List<byte> data = new();

            for (int y = 0; y < normals.GetLength(0); y++)
            {
                for (int x = 0; x < normals.GetLength(1); x++)
                {
                    data.AddRange(ByteWriter.ToBytes(normals[y, x].x, typeof(byte)));
                    data.AddRange(ByteWriter.ToBytes(normals[y, x].y, typeof(byte)));
                    data.AddRange(ByteWriter.ToBytes(normals[y, x].z, typeof(byte)));
                }
            }

            var serialized = Encoding.ASCII.GetBytes(GetType().Name)
               .Concat(BitConverter.GetBytes(data.Count))
               .Concat(data).ToArray();
            return serialized;
        }
    }

    public struct Normal
    {
        public byte x;
        public byte y;
        public byte z;
    }
}