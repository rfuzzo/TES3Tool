using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TES3Lib.Base;
using TES3Lib.Subrecords.TES3;
using Utility;
using static Utility.Common;

namespace TES3Lib.Records
{
    /// <summary>
    /// TES3 Header record
    /// </summary>
    public class TES3 : Record
    {
        public HEDR HEDR { get; set; }

        public List<(MAST MAST, DATA DATA)> Masters;

        public TES3()
        {
        }

        public TES3(byte[] rawData) : base(rawData)
        {
            BuildSubrecords();
        }

        public override void BuildSubrecords()
        {
            var reader = new ByteReader();

            Masters = new List<(MAST MAST, DATA DATA)>();

            while (Data.Length != reader.offset)
            {
                var subrecordName = GetRecordName(reader);
                var subrecordSize = GetRecordSize(reader);
                try
                {
                    if (subrecordName.Equals("MAST"))
                    {
                        Masters.Add((new MAST(reader.ReadBytes<byte[]>(Data, subrecordSize)), null));
                        continue;
                    }

                    else if (subrecordName.Equals("DATA"))
                    {
                        int index = Masters.Count - 1;
                        Masters[index] = (Masters[index].MAST, new DATA(reader.ReadBytes<byte[]>(Data, subrecordSize)));
                        continue;
                    }

                    var subrecordProp = GetType().GetProperty(subrecordName);
                    var subrecord = Activator.CreateInstance(subrecordProp.PropertyType, new object[] { reader.ReadBytes<byte[]>(Data, subrecordSize) });
                    subrecordProp.SetValue(this, subrecord);

                }
                catch (Exception e)
                {
                    Console.WriteLine($"error in building {GetType()} on {subrecordName} either not implemented or borked {e}");
                    break;
                }
            }
        }

        public override byte[] SerializeRecord()
        {
            var properties = GetType()
                .GetProperties(BindingFlags.Public |
                               BindingFlags.Instance |
                               BindingFlags.DeclaredOnly).OrderBy(x => x.MetadataToken).ToList();

            List<byte> data = new();
            foreach (PropertyInfo property in properties)
            {
                var subrecord = (Subrecord)property.GetValue(this);
                if (subrecord is null) continue;

                data.AddRange(subrecord.SerializeSubrecord());
            }

            if (Masters is not null && Masters.Count > 0)
            {
                foreach (var master in Masters)
                {
                    data.AddRange(master.MAST.SerializeSubrecord());
                    data.AddRange(master.DATA.SerializeSubrecord());
                }
            }

            return Encoding.ASCII.GetBytes(GetType().Name)
                .Concat(BitConverter.GetBytes(data.Count))
                .Concat(BitConverter.GetBytes(Header))
                .Concat(BitConverter.GetBytes(SerializeFlag()))
                .Concat(data).ToArray();
        }
    }
}
