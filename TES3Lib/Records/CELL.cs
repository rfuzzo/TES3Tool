using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TES3Lib.Base;
using TES3Lib.Enums.Flags;
using TES3Lib.Subrecords.CELL;
using TES3Lib.Subrecords.Shared;
using Utility;

namespace TES3Lib.Records
{
    /// <summary>
    /// Cell record
    /// </summary>
    public class CELL : Record
    {
        /// <summary>
        /// EditorId (Cell name)
        /// </summary>
        public NAME NAME { get; set; }

        /// <summary>
        /// Cell grid data and flags
        /// </summary>
        public DATA DATA { get; set; }

        /// <summary>
        /// Cell region
        /// </summary>
        public RGNN RGNN { get; set; }

        /// <summary>
        /// Water level (?)
        /// </summary>
        public INTV INTV { get; set; }

        /// <summary>
        /// Map color
        /// Dont really know what this does
        /// </summary>
        public NAM5 NAM5 { get; set; }

        /// <summary>
        /// Interior water level
        /// </summary>
        public WHGT WHGT { get; set; }

        /// <summary>
        /// Light setting
        /// </summary>
        public AMBI AMBI { get; set; }

        /// <summary>
        /// Number of references in cell
        /// </summary>
        public NAM0 NAM0 { get; set; }

        /// <summary>
        /// Cell references
        /// </summary>
        public List<REFR> REFR { get; set; }

        public int unParsedBytesLen = 0;

        public CELL()
        {
            REFR = new List<REFR>();
        }

        public CELL(byte[] rawData) : base(rawData)
        {
            BuildSubrecords();
        }

        public override void BuildSubrecords()
        {
            var readerData = new ByteReader();
            REFR = new List<REFR>();
            while (Data.Length != readerData.offset)
            {
                var subrecordName = GetRecordName(readerData);
                var subrecordSize = GetRecordSize(readerData);
                try
                {
                    // if FRMR or MVRF then we are at the reference list
                    // skip parsing references and read as raw bytes
                    if (subrecordName.Equals("FRMR") || subrecordName.Equals("MVRF"))
                    {
                        // read to end
                        unParsedBytesLen = Data.Length - readerData.offset;
                        readerData.ShiftForwardBy(unParsedBytesLen);

                        //buffer = readerData.ReadBytes<byte[]>(Data, rest);
                    }
                    //if (subrecordName.Equals("FRMR") || subrecordName.Equals("MVRF"))
                    //{
                    //    var refrListType = GetType().GetProperty("REFR");
                    //    var reflist = (List<REFR>)refrListType.GetValue(this);
                    //    reflist.Add(new REFR(Data, readerData));
                    //}
                    else
                    {
                        var subrecordProp = GetType().GetProperty(subrecordName);
                        var subrecord = Activator.CreateInstance(subrecordProp.PropertyType, new object[] { readerData.ReadBytes<byte[]>(Data, subrecordSize) });
                        subrecordProp.SetValue(this, subrecord);
                    }
                }
                catch (Exception e)
                {
                    if (NAME is not null)
                    {
                        Console.WriteLine(NAME.EditorId);
                    }
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
            foreach (var property in properties)
            {
                if (property.Name == "REFR")
                {
                    continue;
                }

                var subrecord = (Subrecord)property.GetValue(this);
                if (subrecord is null)
                {
                    continue;
                }

                data.AddRange(subrecord.SerializeSubrecord());
            }

            if (REFR.Count > 0)
            {
                List<byte> cellReferences = new();
                foreach (var refr in REFR)
                {
                    cellReferences.AddRange(refr.SerializeRecord());
                }
                data.AddRange(cellReferences.ToArray());
            }

            // add unparsed buffer
            var raw = GetRawLoadedBytes();
            var buffer = raw.Skip(raw.Length - unParsedBytesLen).ToArray();
            if (REFR.Count == 0 && buffer is not null && buffer.Length > 0)
            {
                data.AddRange(buffer);
            }

            return Encoding.ASCII.GetBytes(GetType().Name)
                .Concat(BitConverter.GetBytes(data.Count))
                .Concat(BitConverter.GetBytes(Header))
                .Concat(BitConverter.GetBytes(SerializeFlag()))
                .Concat(data).ToArray();
        }

        public override bool Equals(object obj)
        {
            return obj is CELL cell && (cell.DATA.Flags.Contains(CellFlag.IsInteriorCell)
                ? NAME.EditorId.Equals(cell.NAME.EditorId)
                : DATA.GridX.Equals(cell.DATA.GridX) && DATA.GridY.Equals(cell.DATA.GridY));
        }

        public override int GetHashCode()
        {
            return DATA.Flags.Contains(CellFlag.IsInteriorCell)
                ? NAME.EditorId.GetHashCode()
                : Tuple.Create(DATA.GridX, DATA.GridY).GetHashCode();
        }

        public override string GetEditorId()
        {
            return DATA.Flags.Contains(CellFlag.IsInteriorCell) ? (NAME?.EditorId) : $"({DATA.GridX},{DATA.GridY})";
        }

        public byte[] SerializeRecordForMerge()
        {
            var properties = GetType()
                .GetProperties(BindingFlags.Public |
                               BindingFlags.Instance |
                               BindingFlags.DeclaredOnly).OrderBy(x => x.MetadataToken).ToList();

            List<byte> data = new();
            foreach (var property in properties)
            {
                if (property.Name == "REFR")
                {
                    continue;
                }

                var subrecord = (Subrecord)property.GetValue(this);
                if (subrecord is null)
                {
                    continue;
                }

                data.AddRange(subrecord.SerializeSubrecord());
            }

            // don't serialize references for merging: skip to end

            return Encoding.ASCII.GetBytes(GetType().Name)
                .Concat(BitConverter.GetBytes(data.Count))
                .Concat(BitConverter.GetBytes(Header))
                .Concat(BitConverter.GetBytes(SerializeFlag()))
                .Concat(data).ToArray();
        }


    }
}
