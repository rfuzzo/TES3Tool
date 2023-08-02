﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TES3Lib.Base;
using TES3Lib.Enums;
using Utility;
using Attribute = TES3Lib.Enums.Attribute;

namespace TES3Lib.Subrecords.SKILL
{
    public class SKDT : Subrecord
    {
        public Attribute Attribute { get; set; }
        public Specialization Specialization { get; set; }
        public float[] UseValue { get; set; }

        public SKDT(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            Attribute = (Attribute)reader.ReadBytes<int>(Data);
            Specialization = (Specialization)reader.ReadBytes<int>(Data);

            UseValue = new float[4];
            for (int i = 0; i < UseValue.Length; i++)
            {
                UseValue[i] = reader.ReadBytes<float>(Data);
            }
        }

        public SKDT()
        {
        }

        public override byte[] SerializeSubrecord()
        {
            var properties = GetType()
                .GetProperties(System.Reflection.BindingFlags.Public |
                               System.Reflection.BindingFlags.Instance |
                               System.Reflection.BindingFlags.DeclaredOnly)
                               .OrderBy(x => x.MetadataToken)
                               .ToList();

            List<byte> data = new();
            data.AddRange(ByteWriter.ToBytes(Attribute, typeof(int)));
            data.AddRange(ByteWriter.ToBytes(Specialization, typeof(Specialization)));

            for (int i = 0; i < UseValue.Length; i++)
            {
                data.AddRange(ByteWriter.ToBytes(UseValue[i], typeof(float)));
            }

            var serialized = Encoding.ASCII.GetBytes(GetType().Name)
               .Concat(BitConverter.GetBytes(data.Count))
               .Concat(data).ToArray();
            return serialized;
        }
    }
}