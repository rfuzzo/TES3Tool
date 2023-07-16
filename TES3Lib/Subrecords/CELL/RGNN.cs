using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.CELL
{
    public class RGNN : Subrecord, IStringView
    {
        public string Text
        {
            get => RegionName;
            set => RegionName = value;
        }


        /// <summary>
        /// Cells region name
        /// </summary>
        public string RegionName { get; set; }

        public RGNN()
        {

        }

        public RGNN(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            RegionName = reader.ReadBytes<string>(Data, Size);
        }
    }
}
