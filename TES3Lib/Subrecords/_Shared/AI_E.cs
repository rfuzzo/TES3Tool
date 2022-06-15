using TES3Lib.Base;
using Utility;
using Utility.Attributes;

namespace TES3Lib.Subrecords.Shared
{
    /// <summary>
    /// AI Escort Package
    /// </summary>
    public class AI_E : Subrecord, IAIPackage
    {
        public float DestinationX { get; set; }

        public float DestinationY { get; set; }

        public float DestinationZ { get; set; }

        public short Duration { get; set; }

        /// <summary>
        /// Always 32 bytes, if EditorId is less, then its padded
        /// with memory junk after null terminator
        /// </summary>
        [SizeInBytes(32)]
        public string TargetEditorId { get; set; }

        /// <summary>
        /// Unknown (0100?)
        /// </summary>
        public short Unknown { get; set; }

        public AI_E()
        {
            DestinationX = 0x7F7FFFFF;
            DestinationY = 0x7F7FFFFF;
            DestinationZ = 0x7F7FFFFF;
            Unknown = 1;
        }

        public AI_E(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            DestinationX = reader.ReadBytes<float>(Data);
            DestinationY = reader.ReadBytes<float>(Data);
            DestinationZ = reader.ReadBytes<float>(Data);
            Duration = reader.ReadBytes<short>(Data);
            TargetEditorId = reader.ReadBytes<string>(Data, 32);
            Unknown = reader.ReadBytes<short>(Data);
        }
    }
}