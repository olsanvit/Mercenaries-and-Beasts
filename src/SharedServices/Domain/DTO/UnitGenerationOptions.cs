
using MercenariesAndBeasts.Domain.Enums;

namespace MercenariesAndBeasts.Domain.Dto;
public enum UnitRole
{
    // -------------------------
    // FRONTLINE / DURABILITY
    // -------------------------
    Tank = 1,              // drží aggro, vydrží
    Bulwark = 2,           // tank + štíty/mitigace
    Bruiser = 3,           // vydrží a dává dmg
    Vanguard = 4,          // start fight pressure, tempo
    Juggernaut = 5,        // pomalý, extrémně odolný, ramp-up

    // -------------------------
    // DIRECT DAMAGE
    // -------------------------
    Assassin = 6,          // single-target burst, execute
    Skirmisher = 7,        // hit&run, přepínání cílů
    Duelist = 8,           // 1v1 dominance, parry/precision
    Striker = 9,           // čistý single-target DPS
    Ravager = 10,          // AoE physical blender

    // -------------------------
    // MAGIC / RANGED / TECH
    // -------------------------
    Caster = 11,           // spell DPS
    Artillery = 12,        // ranged AoE, pomalejší
    Sniper = 13,           // ranged single-target burst/crit
    Battlemage = 14,       // caster + frontline hybrid
    Summoner = 15,         // spawn adds / pets / echoes

    // -------------------------
    // CONTROL / DISRUPTION
    // -------------------------
    Controller = 16,       // stuns/roots/silence, deny
    Disruptor = 17,        // turn-meter, cooldown, debuff break
    Saboteur = 18,         // armor shred, traps, debuff stacking

    // -------------------------
    // SUPPORT / TEAMPLAY
    // -------------------------
    Support = 19,          // buff/cleanse
    Healer = 20,           // sustain/heal focus
    Buffer = 21,           // team steroids (atk/speed/crit)
    Debuffer = 22          // weaken enemies (def down, vuln, etc.)
}
public sealed class UnitGenerationOptions
{
    public bool IsMercenary { get; set; }

    public ElementType Element { get; set; }

    public ExpeditionStageType? ExpeditionStageType { get; set; }
    public DungeonStageType? DungeonStageType { get; set; }

    public UnitRole Role { get; set; }

    public int TargetLevel { get; set; }
}