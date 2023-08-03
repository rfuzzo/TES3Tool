﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Utility;
using Utility.Attributes;

namespace TES3Lib.Base
{
    /// <summary>
    /// Base class for TES3 Subrecord
    /// </summary>
    public abstract class Subrecord
    {
        /// <summary>
        /// 4 letter subrecord name
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Subrecord size minus header
        /// </summary>
        protected int Size { get; set; }

        /// <summary>
        /// Subrecord data without header with length defnined in Size property
        /// </summary>
        protected byte[] Data { get; set; }

        /// <summary>
        /// Raw bytes of record (header+data)
        /// </summary>
        private byte[] RawData { get; set; }

        /// <summary>
        /// Will be removed, simple check if subrecord is implemented, if not it will use RawData
        /// when serialized back to bytes
        /// </summary>
        protected bool IsImplemented = true;

        /// <summary>
        /// Used for loading subrecord data from ESM/ESP
        /// </summary>
        /// <param name="rawData">raw byte array from ESP/ESM</param>
        public Subrecord(byte[] rawData)
        {
            RawData = rawData;
            var reader = new ByteReader();
            Name = reader.ReadBytes<string>(RawData, 4);
            Size = reader.ReadBytes<int>(RawData);
            Data = reader.ReadBytes<byte[]>(RawData, Size);
        }

        public Subrecord()
        {
            Name = GetType().Name;
        }

        /// <summary>
        /// Serializes Subrecord into byte array
        /// Overwrite when subrecords needs specific serialization functions
        /// </summary>
        /// <returns>Byte array with serialized subrecord</returns>
        public virtual byte[] SerializeSubrecord()
        {
            if (!IsImplemented)
            {
                return RawData;
            }

            var properties = GetType()
                .GetProperties(BindingFlags.Public |
                               BindingFlags.Instance |
                               BindingFlags.DeclaredOnly)
                               .OrderBy(x => x.MetadataToken)
                               .ToList();

            List<byte> data = new();
            foreach (var property in properties)
            {
                var value = property.GetValue(this);
                var sizeAttribute = property.GetCustomAttributes<SizeInBytesAttribute>().FirstOrDefault();

                //used for flags in subrecords
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(HashSet<>))
                {
                    var enumType = property.PropertyType.GetGenericArguments()[0];
                    var enumValueType = Enum.GetUnderlyingType(enumType);

                    data.AddRange(ByteWriter.ToBytes(SerializeFlag(value), enumValueType));
                    continue;
                }

                data.AddRange(ByteWriter.ToBytes(value, property.PropertyType, sizeAttribute));
            }

            var serialized = Encoding.ASCII.GetBytes(GetType().Name)
               .Concat(BitConverter.GetBytes(data.Count))
               .Concat(data).ToArray();
            return serialized;
        }

        protected static uint SerializeFlag(object value)
        {
            uint flag = 0;
            foreach (Enum flagElement in value as IEnumerable)
            {
                flag |= Convert.ToUInt32(flagElement);
            }

            return flag;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Subrecord)
            {
                return false;
            }

            var properties = GetType()
                .GetProperties(BindingFlags.Public |
                               BindingFlags.Instance |
                               BindingFlags.DeclaredOnly)
                               .OrderBy(x => x.MetadataToken)
                               .ToList();

            foreach (var property in properties)
            {
                var thisValue = property.GetValue(this);
                var otherValue = property.GetValue(obj);
                if (thisValue is null && otherValue is null)
                {
                    continue;
                }
                else if (thisValue is null && otherValue is not null)
                {
                    return false;
                }
                else if (thisValue is not null && otherValue is null)
                {
                    return false;
                }
                else
                {
                    var objValue = obj is not null ? otherValue : null;
                    if (!thisValue.Equals(objValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public byte[] GetRawLoadedBytes()
        {
            return RawData;
        }
    }
}
