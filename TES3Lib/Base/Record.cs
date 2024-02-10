﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TES3Lib.Enums.Flags;
using TES3Lib.Subrecords.REFR;
using TES3Lib.Subrecords.Shared;
using Utility;
using static Utility.Common;

namespace TES3Lib.Base
{
    /// <summary>
    /// Base class for TES3 Record
    /// </summary>
    abstract public class Record
    {
        #region Fields

        /// <summary>
        /// Record name (4 bytes)
        /// </summary>
        readonly public string Name;

        /// <summary>
        /// Records data size (4 bytes)
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Not known (4 bytes)
        /// </summary>
        public int Header { get; set; }

        /// <summary>
        /// Record flags (4 bytes)
        ///  0x00002000 = Blocked
		///	 0x00000400 = Persistant
        /// </summary>
        public HashSet<RecordFlag> Flags { get; set; }

        public DELE DELE { get; set; }

        /// <summary>
        /// Raw bytes of records data (variable)
        /// </summary>
        protected byte[] Data { get; set; }

        /// <summary>
        /// Just a switch to say from what source serialize
        /// </summary>
        protected bool IsImplemented = true;

        /// <summary>
        /// Raw bytes of record (record)
        /// </summary>
        private byte[] RawData { get; set; }

        #endregion

        public Record()
        {
            Header = 0;
            Flags = new HashSet<RecordFlag>();
        }

        /// <summary>
        /// Reads record from raw bytes
        /// </summary>
        /// <param name="rawData">Byte array of record data</param>
        public Record(byte[] rawData)
        {
            RawData = rawData;
            var readerHeader = new ByteReader();
            Name = readerHeader.ReadBytes<string>(RawData, 4);
            Size = readerHeader.ReadBytes<int>(RawData);
            Header = readerHeader.ReadBytes<int>(RawData);
            Flags = readerHeader.ReadFlagBytes<RecordFlag>(RawData);
            Data = readerHeader.ReadBytes<byte[]>(RawData, (int)Size);
        }

        public virtual void BuildSubrecords()
        {
            if (!IsImplemented) return;

            var reader = new ByteReader();
            while (Data.Length != reader.offset)
            {
                string subrecordName = GetRecordName(reader);
                int subrecordSize = GetRecordSize(reader);
                try
                {
                    ReadSubrecords(reader, subrecordName, subrecordSize);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"error in building {GetType()} on {subrecordName} either not implemented or borked {e}");
                    break;
                }
            }
        }

        protected void ReadSubrecords(ByteReader readerData, string subrecordName, int subrecordSize)
        {
            PropertyInfo subrecordProp = GetType().GetProperty(subrecordName);
            if (subrecordProp.PropertyType.IsGenericType)
            {
                var listType = subrecordProp.PropertyType.GetGenericArguments()[0];
                if (subrecordProp.GetValue(this) is null)
                {
                    var IListRef = typeof(List<>);
                    Type[] IListParam = { listType };
                    object subRecordList = Activator.CreateInstance(IListRef.MakeGenericType(IListParam));
                    subrecordProp.SetValue(this, subRecordList);
                }
                object sub = Activator.CreateInstance(listType, new object[] { readerData.ReadBytes<byte[]>(Data, subrecordSize) });

                subrecordProp.GetValue(this).GetType().GetMethod("Add").Invoke(subrecordProp.GetValue(this), new[] { sub });
                return;
            }
            object subrecord = Activator.CreateInstance(subrecordProp.PropertyType, new object[] { readerData.ReadBytes<byte[]>(Data, subrecordSize) });
            subrecordProp.SetValue(this, subrecord);
        }

        public virtual byte[] SerializeRecord()
        {
            if (!IsImplemented) return RawData;

            var properties = GetType()
                .GetProperties(BindingFlags.Public |
                               BindingFlags.Instance |
                               BindingFlags.DeclaredOnly)
                               .OrderBy(x => x.MetadataToken)
                               .ToList();

            if (properties.Any(x => x.Name.Equals("NAME")))
            {
                var index = properties.FindIndex(x => x.Name.Equals("NAME"));
                properties.Insert(++index, GetType().GetProperty("DELE"));
            }

            List<byte> data = new();
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType.IsGenericType)
                {
                    var subrecordList = property.GetValue(this) as IEnumerable;
                    if (subrecordList is null) continue;
                    foreach (var sub in subrecordList)
                    {
                        data.AddRange((sub as Subrecord).SerializeSubrecord());
                    }
                    continue;
                }

                if (property.GetValue(this) is Subrecord subrecord)
                {
                    var subbytes = subrecord.SerializeSubrecord();
                    data.AddRange(subbytes);
                }
            }

            return Encoding.ASCII.GetBytes(GetType().Name)
                .Concat(BitConverter.GetBytes(data.Count))
                .Concat(BitConverter.GetBytes(Header))
                .Concat(BitConverter.GetBytes(SerializeFlag()))
                .Concat(data).ToArray();
        }

        protected uint SerializeFlag()
        {
            uint flagSerialized = 0;
            foreach (RecordFlag flagElement in Flags)
            {
                flagSerialized |= (uint)flagElement;
            }
            return flagSerialized;
        }

        protected string GetRecordName(ByteReader reader)
        {
            var name = reader.ReadBytes<string>(Data, 4);
            reader.ShiftBackBy(4);
            return name;
        }

        protected int GetRecordSize(ByteReader reader)
        {
            reader.ShiftForwardBy(4);
            var size = reader.ReadBytes<int>(Data) + 8;
            reader.ShiftBackBy(8);
            return size;
        }

        protected bool IsEndOfData(ByteReader reader) => (reader.offset == Data.Length);

        /// <summary>
        /// Get EditorId of record if exists
        /// </summary>
        public virtual string GetEditorId()
        {
            PropertyInfo name = GetType().GetProperty("NAME");
            if (name is not null)
            {
                var NAME = (NAME)name.GetValue(this);
                return NAME?.EditorId;
            }

            return null;
        }

        public byte[] GetRawLoadedBytes()
        {
            return RawData;
        }

        /// <summary>
        /// Compares EditorId of records if NAME subrecord is present
        /// </summary>
        public override bool Equals(object obj)
        {
            PropertyInfo name = GetType().GetProperty("NAME");
            if (name is not null)
            {
                var NAME1 = (NAME)name.GetValue(this);
                var NAME2 = (NAME)obj.GetType().GetProperty("NAME").GetValue(obj);

                return NAME1.EditorId.Equals(NAME2.EditorId);
            }

            return base.Equals(obj);
        }

        public bool DeepEquals(Record other)
        {
            var values = new List<object>();
            var properties = GetType()
                .GetProperties(BindingFlags.Public |
                               BindingFlags.Instance |
                               BindingFlags.DeclaredOnly)
                               .OrderBy(x => x.MetadataToken)
                               .ToList();
            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(this);
                var otherValue = property.GetValue(other);
                if (value is Subrecord s && otherValue is Subrecord sOther) {
                    if (!value.Equals(sOther))
                    {
                        return false;
                    }
                }
                else if (value is not null)
                {
                    if (value is IList list1 && otherValue is IList list2)
                    {
                        var l1 = list1.Cast<object>().ToList();
                        var l2 = list2.Cast<object>().ToList();
                        var same = l1.SequenceEqual(l2);
                        if (!same)
                        {
                            return false;
                        }
                    }
                    else if (!value.Equals(otherValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;

            var properties = GetType()
                .GetProperties(BindingFlags.Public |
                               BindingFlags.Instance |
                               BindingFlags.DeclaredOnly)
                               .OrderBy(x => x.MetadataToken)
                               .ToList();

            int i = 1;
            foreach (PropertyInfo property in properties)
            {
                var value = property.GetValue(this);
                if (value is not null)
                {
                    hash += i * property.GetValue(this).GetHashCode();
                }
                i++;
            }

            return hash;
        }

        public List<string> GetPropertyNames()
        {
            var list = new List<string>();
            var recordProperties = GetType().GetProperties(
               BindingFlags.Public |
               BindingFlags.Instance |
               BindingFlags.DeclaredOnly).ToList();
            foreach (PropertyInfo prop in recordProperties)
            {
                var v = prop.GetValue(this);

                v ??= Activator.CreateInstance(prop.PropertyType);

                if (v is Subrecord subrecord)
                {
                    var subRecordProperties = subrecord.GetType().GetProperties(
                        BindingFlags.Public |
                        BindingFlags.Instance |
                        BindingFlags.DeclaredOnly).ToList();
                    foreach (PropertyInfo subProp in subRecordProperties)
                    {
                        if (list.Contains(subProp.Name))
                        {
                            list.Add($"{subrecord.Name}.{subProp.Name}");
                        }
                        else
                        {
                            list.Add(subProp.Name);
                        }
                        
                    }
                }
                else
                {
                    list.Add(prop.Name);
                }
            }

            return list;
        }
    }
}
