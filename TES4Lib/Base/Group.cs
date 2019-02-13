﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Utility;

namespace TES4Lib.Structures.Base
{
    public class Group
    {
        readonly public string Name;
        public int Size { get; set; }
        public string Label { get; set; }
        public int Type { get; set; }
        public int Stamp { get; set; }
        public byte[] Data { get; set; }
        private byte[] RawData { get; set; }

        private List<Record> records = new List<Record>();

        public List<Record> Records
        {
            get { return records; }
        }

        private List<Group> groups = new List<Group>();

        public List<Group> Groups
        {
            get { return groups; }
        }

        public Group(byte [] rawData)
        {
            RawData = rawData;
            var reader = new ByteReader();
            Name = reader.ReadBytes<string>(RawData, 4);
            Size = reader.ReadBytes<int>(RawData);
            var labelRaw = reader.ReadBytes<byte[]>(RawData, 4);
            Type = reader.ReadBytes<int>(RawData);
            Label = GenerateLabel(Type, labelRaw);
            Stamp = reader.ReadBytes<int>(RawData);
            Data = reader.ReadBytes<byte[]>(RawData, RawData.Length - 20);
            BuildRecords();
        }

        private string GenerateLabel(int type, byte[] rawLabel)
        {
            switch (type)
            {
                case 0:
                    return Encoding.ASCII.GetString(rawLabel);
                case 1:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    return BitConverter.ToString(rawLabel).Replace("-", "");
                case 2:
                case 3:
                    return "Exterior block (unsupported)";
                case 4:
                case 5:
                    return $"Block number {BitConverter.ToInt32(rawLabel, 0)}";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Builds Records or Groups
        /// </summary>
        private void BuildRecords()
        {
            if (Data.Length == 0) return; //group has no records or subgroups

            var reader = new ByteReader();
            while (Data.Length!=reader.offset)
            {
                var name = reader.ReadBytes<string>(Data, 4);
                var size = reader.ReadBytes<int>(Data);
                var type = reader.ReadBytes<string>(Data,4);
                reader.offset -= 12;

                if (!name.Equals("GRUP"))
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    var rawRecord = reader.ReadBytes<byte[]>(Data, size + 20);
                    Record record = assembly
                        .CreateInstance($"TES4Lib.Records.{name}", false, BindingFlags.Default, null, new object[] { rawRecord }, null, null) as Record;
                    Records.Add(record);
                }
                else
                {
                    Groups.Add(new Group(reader.ReadBytes<byte[]>(Data, size)));
                }
            }
        }
    }
}
