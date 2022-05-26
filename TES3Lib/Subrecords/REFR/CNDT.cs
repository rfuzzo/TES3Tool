using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.REFR
{
    /// <summary>
    /// Position data
    /// </summary>
    public class CNDT : Subrecord
    {
        public int GridX { get; set; }

        public int GridY { get; set; }

        public CNDT()
        {
        }

        public CNDT(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            GridX = reader.ReadBytes<int>(base.Data);
            GridY = reader.ReadBytes<int>(base.Data);
        }
    }
}
