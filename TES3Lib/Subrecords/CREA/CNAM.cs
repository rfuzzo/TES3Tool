using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.CREA
{
    /// <summary>
    /// Sound set creature uses 
    /// </summary>
    public class CNAM : Subrecord, IStringView
    {
        public string Text
        {
            get => SoundGen;
            set => SoundGen = value;
        }
        public string SoundGen { get; set; }

        public CNAM()
        {

        }

        public CNAM(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            SoundGen = reader.ReadBytes<string>(Data, Size);
        }
    }
}