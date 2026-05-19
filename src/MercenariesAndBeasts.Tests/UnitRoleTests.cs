using FluentAssertions;
using MercenariesAndBeasts.Domain.Dto;

namespace MercenariesAndBeasts.Tests;

/// <summary>
/// Tests that UnitRole enum values are defined correctly.
/// </summary>
public class UnitRoleTests
{
    [Fact]
    public void UnitRole_Tank_HasValue1()
    {
        ((int)UnitRole.Tank).Should().Be(1);
    }

    [Fact]
    public void UnitRole_Assassin_HasValue6()
    {
        ((int)UnitRole.Assassin).Should().Be(6);
    }

    [Fact]
    public void UnitRole_Bulwark_HasValue2()
    {
        ((int)UnitRole.Bulwark).Should().Be(2);
    }

    [Fact]
    public void UnitRole_Bruiser_HasValue3()
    {
        ((int)UnitRole.Bruiser).Should().Be(3);
    }

    [Fact]
    public void UnitRole_Vanguard_HasValue4()
    {
        ((int)UnitRole.Vanguard).Should().Be(4);
    }

    [Fact]
    public void UnitRole_Juggernaut_HasValue5()
    {
        ((int)UnitRole.Juggernaut).Should().Be(5);
    }

    [Fact]
    public void UnitRole_AllFrontlineValues_AreIn1To5Range()
    {
        var frontlineRoles = new[]
        {
            UnitRole.Tank,
            UnitRole.Bulwark,
            UnitRole.Bruiser,
            UnitRole.Vanguard,
            UnitRole.Juggernaut
        };

        frontlineRoles.Should().AllSatisfy(role =>
            ((int)role).Should().BeInRange(1, 5));
    }

    [Fact]
    public void UnitRole_Enum_ContainsTank()
    {
        Enum.IsDefined(typeof(UnitRole), UnitRole.Tank).Should().BeTrue();
    }

    [Fact]
    public void UnitRole_Enum_ContainsAssassin()
    {
        Enum.IsDefined(typeof(UnitRole), UnitRole.Assassin).Should().BeTrue();
    }

    [Fact]
    public void UnitRole_AllDefined_ValuesAreUnique()
    {
        var values = Enum.GetValues<UnitRole>();
        var distinctValues = values.Select(v => (int)v).Distinct();
        distinctValues.Should().HaveCount(values.Length);
    }

    [Fact]
    public void UnitRole_ParseByName_ReturnsCorrectValue()
    {
        var parsed = Enum.Parse<UnitRole>("Tank");
        parsed.Should().Be(UnitRole.Tank);
        ((int)parsed).Should().Be(1);
    }

    [Fact]
    public void UnitRole_ParseAssassin_ReturnsValue6()
    {
        var parsed = Enum.Parse<UnitRole>("Assassin");
        ((int)parsed).Should().Be(6);
    }
}
