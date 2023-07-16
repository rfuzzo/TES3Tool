using System.Diagnostics;
using TES3Lib.Base;
using TES3Lib.Interfaces;
using Utility;

namespace TES3Lib.Subrecords.ARMO
{
    /// <summary>
    /// Male part name
    /// </summary>
    [DebuggerDisplay("{MalePartName}")]
    public class BNAM : Subrecord, IStringView
    {
        public string Text
        {
            get => MalePartName;
            set => MalePartName = value;
        }
        /// <summary>
        /// Male tagged bodpart id
        /// </summary>
        public string MalePartName { get; set; }

        public BNAM()
        {
        }

        public BNAM(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            MalePartName = reader.ReadBytes<string>(Data, Size);
        }
    }
}
