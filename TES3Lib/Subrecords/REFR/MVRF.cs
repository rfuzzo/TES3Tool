using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TES3Lib.Base;
using Utility;

namespace TES3Lib.Subrecords.REFR
{
    public class MVRF : Subrecord
    {
        public int ObjectIndex { get; set; }

        public MVRF()
        {

        }

        public MVRF(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            ObjectIndex = reader.ReadBytes<int>(Data);
        }
    }
}
