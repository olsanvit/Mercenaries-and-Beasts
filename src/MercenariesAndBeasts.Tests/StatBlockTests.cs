using FluentAssertions;
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Enums;

namespace MercenariesAndBeasts.Tests;

/// <summary>
/// Tests StatBlock properties and the StatBlockExtensions.Plus() method.
/// </summary>
public class StatBlockTests
{
    [Fact]
    public void StatBlock_MaxHp_DefaultsToZero()
    {
        var sb = new StatBlock();
        sb.MaxHp.Should().Be(0f);
    }

    [Fact]
    public void StatBlock_Attack_DefaultsToZero()
    {
        var sb = new StatBlock();
        sb.Attack.Should().Be(0f);
    }

    [Fact]
    public void StatBlock_MaxHp_CannotBeNegative_WhenSetToValidValue()
    {
        var sb = new StatBlock { MaxHp = 100f };
        sb.MaxHp.Should().BeGreaterOrEqualTo(0f);
    }

    [Fact]
    public void StatBlock_Attack_CannotBeNegative_WhenSetToValidValue()
    {
        var sb = new StatBlock { Attack = 50f };
        sb.Attack.Should().BeGreaterOrEqualTo(0f);
    }

    [Fact]
    public void StatBlock_CriticalChance_DefaultsToZero()
    {
        var sb = new StatBlock();
        sb.CriticalChance.Should().Be(0.0);
    }

    [Fact]
    public void StatBlock_CriticalChance_WithinValidRange()
    {
        var sb = new StatBlock { CriticalChance = 0.25 };
        sb.CriticalChance.Should().BeInRange(0.0, 1.0);
    }

    [Fact]
    public void StatBlock_CriticalChance_MaxValidValue_IsOne()
    {
        var sb = new StatBlock { CriticalChance = 1.0 };
        sb.CriticalChance.Should().BeInRange(0.0, 1.0);
    }

    [Fact]
    public void StatBlock_Zero_HasDefaultCriticalMultiplier_1Point5()
    {
        var sb = StatBlock.Zero;
        sb.CriticalMultiplier.Should().Be(1.5);
    }

    [Fact]
    public void StatBlock_Zero_HasAccuracy_1Point0()
    {
        var sb = StatBlock.Zero;
        sb.Accuracy.Should().Be(1.0);
    }

    [Fact]
    public void StatBlock_Zero_ElementIsNone()
    {
        var sb = StatBlock.Zero;
        sb.Element.Should().Be(ElementType.None.ToString());
    }

    // ── Plus() method tests ────────────────────────────────────────────────────

    [Fact]
    public void Plus_AddsMaxHp_Correctly()
    {
        var a = new StatBlock { MaxHp = 100f };
        var b = new StatBlock { MaxHp = 50f };

        var result = a.Plus(b);

        result.MaxHp.Should().Be(150f);
    }

    [Fact]
    public void Plus_AddsAttack_Correctly()
    {
        var a = new StatBlock { Attack = 30f };
        var b = new StatBlock { Attack = 20f };

        var result = a.Plus(b);

        result.Attack.Should().Be(50f);
    }

    [Fact]
    public void Plus_AddsCriticalChance_Correctly()
    {
        var a = new StatBlock { CriticalChance = 0.1 };
        var b = new StatBlock { CriticalChance = 0.15 };

        var result = a.Plus(b);

        result.CriticalChance.Should().BeApproximately(0.25, 0.0001);
    }

    [Fact]
    public void Plus_DoesNotMutateOriginals()
    {
        var a = new StatBlock { MaxHp = 100f, Attack = 30f };
        var b = new StatBlock { MaxHp = 50f, Attack = 20f };

        var result = a.Plus(b);

        a.MaxHp.Should().Be(100f);
        a.Attack.Should().Be(30f);
        b.MaxHp.Should().Be(50f);
        b.Attack.Should().Be(20f);
    }

    [Fact]
    public void Plus_WithNullB_ReturnsCloneOfA()
    {
        var a = new StatBlock { MaxHp = 100f, Attack = 40f };

        var result = a.Plus(null);

        result.MaxHp.Should().Be(100f);
        result.Attack.Should().Be(40f);
        result.Should().NotBeSameAs(a);
    }

    [Fact]
    public void Plus_AddsAllCoreStats_Together()
    {
        var a = new StatBlock { MaxHp = 100f, Armor = 20f, Defense = 15f, Speed = 10f };
        var b = new StatBlock { MaxHp = 50f, Armor = 10f, Defense = 5f, Speed = 5f };

        var result = a.Plus(b);

        result.MaxHp.Should().Be(150f);
        result.Armor.Should().Be(30f);
        result.Defense.Should().Be(20f);
        result.Speed.Should().Be(15f);
    }

    [Fact]
    public void Plus_InheritsElementFromB_WhenAIsNone()
    {
        var a = StatBlock.Zero; // Element = "None"
        var b = new StatBlock { Element = ElementType.Fire.ToString() };

        var result = a.Plus(b);

        result.Element.Should().Be(ElementType.Fire.ToString());
    }
}
