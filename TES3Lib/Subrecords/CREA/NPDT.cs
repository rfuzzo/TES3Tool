using TES3Lib.Base;
using TES3Lib.Enums;
using Utility;

namespace TES3Lib.Subrecords.CREA
{
    public class NPDT : Subrecord
    {
        public CreatureType CreatureType { get; set; }

        public int Level { get; set; }

        public int Strength { get; set; }

        public int Intelligence { get; set; }

        public int Willpower { get; set; }

        public int Agility { get; set; }

        public int Speed { get; set; }

        public int Endurance { get; set; }

        public int Personality { get; set; }

        public int Luck { get; set; }

        public int Health { get; set; }

        public int SpellPts { get; set; }

        public int Fatigue { get; set; }

        public int Soul { get; set; }

        public int Combat { get; set; }

        public int Magic { get; set; }

        public int Stealth { get; set; }

        public int AttackMin1 { get; set; }

        public int AttackMax1 { get; set; }

        public int AttackMin2 { get; set; }

        public int AttackMax2 { get; set; }

        public int AttackMin3 { get; set; }

        public int AttackMax3 { get; set; }

        public int Gold { get; set; }

        public NPDT()
        {

        }

        public NPDT(byte[] rawData) : base(rawData)
        {
            var reader = new ByteReader();
            CreatureType = (CreatureType)reader.ReadBytes<int>(Data);
            Level = reader.ReadBytes<int>(Data);
            Strength = reader.ReadBytes<int>(Data);
            Intelligence = reader.ReadBytes<int>(Data);
            Willpower = reader.ReadBytes<int>(Data);
            Agility = reader.ReadBytes<int>(Data);
            Speed = reader.ReadBytes<int>(Data);
            Endurance = reader.ReadBytes<int>(Data);
            Personality = reader.ReadBytes<int>(Data);
            Luck = reader.ReadBytes<int>(Data);
            Health = reader.ReadBytes<int>(Data);
            SpellPts = reader.ReadBytes<int>(Data);
            Fatigue = reader.ReadBytes<int>(Data);
            Soul = reader.ReadBytes<int>(Data);
            Combat = reader.ReadBytes<int>(Data);
            Magic = reader.ReadBytes<int>(Data);
            Stealth = reader.ReadBytes<int>(Data);
            AttackMin1 = reader.ReadBytes<int>(Data);
            AttackMax1 = reader.ReadBytes<int>(Data);
            AttackMin2 = reader.ReadBytes<int>(Data);
            AttackMax2 = reader.ReadBytes<int>(Data);
            AttackMin3 = reader.ReadBytes<int>(Data);
            AttackMax3 = reader.ReadBytes<int>(Data);
            Gold = reader.ReadBytes<int>(Data);
        }
    }
}
