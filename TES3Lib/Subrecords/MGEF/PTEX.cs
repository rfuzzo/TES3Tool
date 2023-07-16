using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.MGEF
{
    /// <summary>
    /// Particle texture path (32 characters max!)
    /// </summary>
    public class PTEX : Subrecord, IStringView
    {
        public string Text
        {
            get => ParticleTexturePath;
            set => ParticleTexturePath = value;
        }
        public string ParticleTexturePath { get; set; }

        public PTEX(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            ParticleTexturePath = reader.ReadBytes<string>(Data, Size);
        }
    }
}
