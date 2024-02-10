using System;
using System.Diagnostics;
using TES3Lib.Base;
using TES3Lib.Subrecords.LAND;

namespace TES3Lib.Records
{
    /// <summary>
    /// Landscape Record
    /// </summary>
    /// 
    [DebuggerDisplay("LAND:{INTV.CellX} x {INTV.CellY}")]
    public class LAND : Record
    {
        /// <summary>
        /// Coordinates
        /// </summary>
        public INTV INTV { get; set; }

        /// <summary>
        /// Unknown
        /// </summary>
        public DATA DATA { get; set; }

        /// <summary>
        /// Vertex normals
        /// </summary>
        public VNML VNML { get; set; }

        /// <summary>
        /// Vertex height map
        /// </summary>
        public VHGT VHGT { get; set; }

        /// <summary>
        /// Vertex low LOD height map
        /// </summary>
        public WNAM WNAM { get; set; }

        /// <summary>
        /// Vertex colors
        /// </summary>
        public VCLR VCLR { get; set; }

        /// <summary>
        /// Texture indices
        /// </summary>
        public VTEX VTEX { get; set; }

        public LAND(byte[] rawData) : base(rawData)
        {
            BuildSubrecords();
        }

        public LAND()
        {

        }

        public override bool Equals(object obj)
        {
            if (obj is LAND)
            {
                var land2 = obj as LAND;
                return INTV.CellX == land2.INTV.CellX && INTV.CellY == land2.INTV.CellY;

            }

            return false;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(INTV.CellX, INTV.CellY).GetHashCode();
        }
    }
}
