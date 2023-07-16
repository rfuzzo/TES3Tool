using TES3Lib.Base;
using TES3Lib.Enums;
using TES3Lib.Interfaces;
using Utility;
using Utility.Attributes;
using Attribute = TES3Lib.Enums.Attribute;

namespace TES3Lib.Subrecords.Shared
{
    namespace Item
    {
        /// <summary>
        /// Enchantment ID string
        /// </summary>
        public class ENAM : Subrecord, IStringView
        {
            public string Text
            {
                get => EnchantmentId;
                set => EnchantmentId = value;
            }

            public string EnchantmentId { get; set; }

            public ENAM()
            {

            }

            public ENAM(byte[] rawData) : base(rawData)
            {
                var reader = new ByteReader();
                EnchantmentId = reader.ReadBytes<string>(Data, Size);
            }
        }
    }

    namespace Castable
    {
        /// <summary>
        /// Alchemy data
        /// </summary>
        public class ENAM : Subrecord
        {
            [SizeInBytes(2)]
            public MagicEffect MagicEffect { get; set; }

            /// <summary>
            /// for skill related effects, 0xFFFFFFFF otherwise
            /// </summary>
            [SizeInBytes(1)]
            public Skill Skill { get; set; }

            /// <summary>
            /// for attribute related effects, 0xFFFFFFFF otherwise
            /// </summary>
            [SizeInBytes(1)]
            public Attribute Attribute { get; set; }

            /// <summary>
            /// This is typically not used. It cannot be defined in the CS, but will function in-game.
            /// </summary>
            public SpellRange SpellRange { get; set; }

            /// <summary>
            /// This is typically not used. It cannot be defined in the CS, but will function in-game.
            /// </summary>
            public int Area { get; set; }

            public int Duration { get; set; }

            /// <summary>
            /// The minimum magnitude. For effects with non-variable magnitudes, use <see cref="Magnitude"/>.
            /// 
            /// This is typically not used. It cannot be defined in the CS, but will function in-game.
            /// </summary>
            public int MinMagnitude { get; set; }


            /// <summary>
            /// The magnitude. For effects with variable magnitudes, this is the maximum magnitude. See also: <seealso cref="MinMagnitude"/>.
            /// </summary>
            public int Magnitude { get; set; }

            public ENAM()
            {
            }

            public ENAM(byte[] rawData) : base(rawData)
            {
                var reader = new ByteReader();
                MagicEffect = reader.ReadBytes<MagicEffect>(Data, 2);
                Skill = reader.ReadBytes<Skill>(Data, 1);
                Attribute = reader.ReadBytes<Attribute>(Data, 1);
                SpellRange = reader.ReadBytes<SpellRange>(Data);
                Area = reader.ReadBytes<int>(Data);
                Duration = reader.ReadBytes<int>(Data);
                MinMagnitude = reader.ReadBytes<int>(Data);
                Magnitude = reader.ReadBytes<int>(Data);
            }
        }
    }
}
