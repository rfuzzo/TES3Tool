using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.REFR
{
    /// <summary>
    /// Position data
    /// </summary>
    public class DATA : Subrecord
    {
        public float XPos { get; set; }
        public float YPos { get; set; }
        public float ZPos { get; set; }
        public float XRotate { get; set; }
        public float YRotate { get; set; }
        public float ZRotate { get; set; }

        public DATA()
        {

        }

        public DATA(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            XPos = reader.ReadBytes<float>(Data);
            YPos = reader.ReadBytes<float>(Data);
            ZPos = reader.ReadBytes<float>(Data);
            XRotate = reader.ReadBytes<float>(Data);
            YRotate = reader.ReadBytes<float>(Data);
            ZRotate = reader.ReadBytes<float>(Data);
        }
    }
}
