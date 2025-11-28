using MercenariesAndBeasts.Domain.Enums;

namespace MercenariesAndBeasts.Domain.Combat
{
    /// <summary>
    /// Základní staty jednotky nebo itemu (bonus).
    /// Mapped as owned type v EF Core.
    /// </summary>
    /// 
    //[Owned]
        public class StatBlock
    {
        public int MaxHp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }

        /// <summary>
        /// Kritická šance v procentech (0–100).
        /// </summary>
        public double CritChance { get; set; }

        /// <summary>
        /// Kritický násobič (např. 1.5 = 150% dmg).
        /// </summary>
        public double CritMultiplier { get; set; }

        public ElementType Element { get; set; }

        /// <summary>
        /// Volitelné procentuální bonusy (např. dmg vs element).
        /// Tohle klidně rozsekáš později na detailnější hodnoty.
        /// </summary>
        public double ElementalDamageBonus { get; set; }
        public double ElementalResistance { get; set; }

        public static StatBlock Zero => new()
        {
            MaxHp = 0,
            Attack = 0,
            Defense = 0,
            Speed = 0,
            CritChance = 0,
            CritMultiplier = 1.5,
            Element = ElementType.None,
            ElementalDamageBonus = 0,
            ElementalResistance = 0
        };

    }
}