using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using TES3Lib.Base;
using TES3Lib.Base.Interfaces;
using TES3Lib.Enums.Flags;
using TES3Lib.Subrecords.CREA;
using TES3Lib.Subrecords.Shared;
using Utility;
using static Utility.Common;

namespace TES3Lib.Records
{
    /// <summary>
    /// Creature Record
    /// </summary>
    [DebuggerDisplay("{NAME.EditorId}")]
    public class CREA : Record, IActor
    {
        /// <summary>
        /// EditorId
        /// </summary>
        public NAME NAME { get; set; }

        /// <summary>
        /// Model
        /// </summary>
        public MODL MODL { get; set; }

        /// <summary>
        /// Sound set
        /// </summary>
        public CNAM CNAM { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        public FNAM FNAM { get; set; }

        /// <summary>
        /// Script
        /// </summary>
        public SCRI SCRI { get; set; }

        /// <summary>
        /// Stats
        /// </summary>
        public NPDT NPDT { get; set; }

        /// <summary>
        /// Flags
        /// </summary>
        public FLAG FLAG { get; set; }

        /// <summary>
        /// Scale
        /// </summary>
        public XSCL XSCL { get; set; }

        /// <summary>
        /// Items held
        /// </summary>
        public List<NPCO> NPCO { get; set; }

        /// <summary>
        /// Spells
        /// </summary>
        public List<NPCS> NPCS { get; set; }

        /// <summary>
        /// Creature behavior
        /// </summary>
        public AIDT AIDT { get; set; }

        /// <summary>
        /// Travel service destinations
        /// </summary>
        public List<(DODT coordinates, DNAM cell)> TravelService { get; set; }

        /// <summary>
        /// AI Packages
        /// CNDT Subrecord only present in packages:
        /// Follow (AI_F), Escort (AI_E)
        /// </summary>
        public List<(IAIPackage AIPackage, CNDT CNDT)> AIPackages { get; set; }

        public CREA()
        {
            NPCO = new List<NPCO>();
            NPCS = new List<NPCS>();
            AIPackages = new List<(IAIPackage AIPackage, CNDT CNDT)>();
            TravelService = new List<(DODT coordinates, DNAM cell)>();
        }

        public CREA(byte[] rawData) : base(rawData)
        {
            BuildSubrecords();
        }

        public override void BuildSubrecords()
        {
            var readerData = new ByteReader();

            NPCO = new List<NPCO>();
            NPCS = new List<NPCS>();
            TravelService = new List<(DODT coordinates, DNAM cell)>();
            AIPackages = new List<(IAIPackage AIPackage, CNDT CNDT)>();

            while (Data.Length != readerData.offset)
            {
                var subrecordName = GetRecordName(readerData);
                var subrecordSize = GetRecordSize(readerData);

                try
                {
                    if (subrecordName.Equals("DODT"))
                    {
                        TravelService.Add((new DODT(readerData.ReadBytes<byte[]>(Data, subrecordSize)), null));
                        continue;
                    }

                    if (subrecordName.Equals("DNAM"))
                    {
                        TravelService[TravelService.Count - 1] = (TravelService[TravelService.Count - 1].coordinates, new DNAM(readerData.ReadBytes<byte[]>(Data, subrecordSize)));
                        continue;
                    }

                    if (subrecordName.Equals("AI_W") || subrecordName.Equals("AI_T") || subrecordName.Equals("AI_F") || subrecordName.Equals("AI_E") || subrecordName.Equals("AI_A"))
                    {
                        Type packageType = Type.GetType($"TES3Lib.Subrecords.Shared.{subrecordName}");
                        IAIPackage aiPackage = Activator.CreateInstance(packageType, new object[] { readerData.ReadBytes<byte[]>(Data, subrecordSize) }) as IAIPackage;

                        CNDT CNDT = null;
                        if (Data.Length != readerData.offset)
                        {
                            subrecordName = GetRecordName(readerData);
                            subrecordSize = GetRecordSize(readerData);
                            if (subrecordName.Equals("CNDT"))
                                CNDT = new CNDT(readerData.ReadBytes<byte[]>(Data, subrecordSize));
                        }

                        AIPackages.Add((aiPackage, CNDT));
                        continue;
                    }

                    ReadSubrecords(readerData, subrecordName, subrecordSize);
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
                if (property.Name.Equals("TravelService"))
                {
                    if (TravelService.Count > 0)
                    {
                        List<byte> travelDest = new();
                        foreach (var destination in TravelService)
                        {
                            travelDest.AddRange(destination.coordinates.SerializeSubrecord());

                            if (destination.cell is not null)
                                travelDest.AddRange(destination.cell.SerializeSubrecord());

                        }
                        data.AddRange(travelDest.ToArray());
                    }
                    continue;
                }

                if (property.Name.Equals("AIPackages"))
                {
                    if (AIPackages.Count > 0)
                    {
                        List<byte> aiPackagesBytes = new();
                        foreach (var aiPackage in AIPackages)
                        {
                            aiPackagesBytes.AddRange(aiPackage.AIPackage.SerializeSubrecord());

                            if (aiPackage.CNDT is not null)
                                aiPackagesBytes.AddRange(aiPackage.CNDT.SerializeSubrecord());

                        }
                        data.AddRange(aiPackagesBytes.ToArray());
                    }
                    continue;
                }

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
                var subrecord = (Subrecord)property.GetValue(this);
                if (subrecord is null) continue;
                data.AddRange(subrecord.SerializeSubrecord());
            }

            uint flagSerialized = 0;
            foreach (RecordFlag flagElement in Flags)
            {
                flagSerialized |= (uint)flagElement;
            }

            return Encoding.ASCII.GetBytes(GetType().Name)
                .Concat(BitConverter.GetBytes(data.Count))
                .Concat(BitConverter.GetBytes(Header))
                .Concat(BitConverter.GetBytes(flagSerialized))
                .Concat(data).ToArray();
        }

        public override string GetEditorId()
        {
            return NAME?.EditorId;
        }
    }
}
