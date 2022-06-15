using System.Collections.Generic;
using TES3Lib.Base;
using TES3Lib.Enums;
using TES3Lib.Enums.Flags;
using Utility;

namespace TES3Lib.Subrecords.BODY
{
    public class BYDT : Subrecord
    {
        public BodyPart BodyPart { get; set; }

        public byte IsVampire { get; set; }

        public HashSet<BodyPartFlag> Flags { get; set; }

        public BodyPartType PartType { get; set; }

        public BYDT()
        {
            BodyPart = BodyPart.Ankle;
            IsVampire = 0;
            Flags = new HashSet<BodyPartFlag> { BodyPartFlag.Playable };
            PartType = BodyPartType.Skin;
        }

        public BYDT(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            BodyPart = reader.ReadBytes<BodyPart>(Data);
            IsVampire = reader.ReadBytes<byte>(Data);
            Flags = reader.ReadFlagBytes<BodyPartFlag>(Data);
            PartType = reader.ReadBytes<BodyPartType>(Data);
        }
    }
}
