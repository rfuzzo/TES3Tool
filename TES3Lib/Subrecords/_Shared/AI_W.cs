using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.Shared
{
    /// <summary>
    /// AI Wander Package
    /// </summary>
    public class AI_W : Subrecord, IAIPackage
    {
        public short Distance { get; set; }

        public short Duration { get; set; }

        public byte TimeOfDay { get; set; }

        /// <summary>
        /// Idle 2-9 chances
        /// </summary>
        public byte[] Idle { get; set; }

        /// <summary>
        /// Default 1?
        /// </summary>
        public byte Unknown { get; set; }

        public AI_W()
        {
            Unknown = 1;
        }

        public AI_W(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            Distance = reader.ReadBytes<short>(Data);
            Duration = reader.ReadBytes<short>(Data);
            TimeOfDay = reader.ReadBytes<byte>(Data);
            Idle = reader.ReadBytes<byte[]>(Data, 8);
            Unknown = reader.ReadBytes<byte>(Data);
        }
    }
}