using FluentAssertions;
using MercenariesAndBeasts.Domain.Enums;

namespace MercenariesAndBeasts.Tests;

/// <summary>
/// Tests item category logic using the ItemEquipSlot enum, which is the
/// primary item categorization mechanism in the MercenariesAndBeasts domain.
/// ItemEquipSlot distinguishes Mercenary slots (1-99) from Beast slots (100-199)
/// and contains a special None value (0) for non-equippable items.
/// </summary>
public class ItemCategoryTests
{
    // ── ItemEquipSlot: None (non-equippable) ─────────────────────────────────

    [Fact]
    public void ItemEquipSlot_None_HasValue0()
    {
        ((int)ItemEquipSlot.None).Should().Be(0);
    }

    [Fact]
    public void ItemEquipSlot_None_IsDefault()
    {
        var slot = default(ItemEquipSlot);
        slot.Should().Be(ItemEquipSlot.None);
    }

    // ── Mercenary slots (1–99) ────────────────────────────────────────────────

    [Fact]
    public void MercenarySlots_HaveValuesIn1To99Range()
    {
        var mercSlots = Enum.GetValues<ItemEquipSlot>()
            .Where(s => s.ToString().StartsWith("Merc_"))
            .ToList();

        mercSlots.Should().NotBeEmpty();
        mercSlots.Should().AllSatisfy(slot =>
            ((int)slot).Should().BeInRange(1, 99));
    }

    [Fact]
    public void BeastSlots_HaveValuesIn100To199Range()
    {
        var beastSlots = Enum.GetValues<ItemEquipSlot>()
            .Where(s => s.ToString().StartsWith("Beast_"))
            .ToList();

        beastSlots.Should().NotBeEmpty();
        beastSlots.Should().AllSatisfy(slot =>
            ((int)slot).Should().BeInRange(100, 199));
    }

    [Fact]
    public void MercenarySlots_And_BeastSlots_DoNotOverlap()
    {
        var mercValues = Enum.GetValues<ItemEquipSlot>()
            .Where(s => s.ToString().StartsWith("Merc_"))
            .Select(s => (int)s)
            .ToHashSet();

        var beastValues = Enum.GetValues<ItemEquipSlot>()
            .Where(s => s.ToString().StartsWith("Beast_"))
            .Select(s => (int)s)
            .ToHashSet();

        mercValues.Intersect(beastValues).Should().BeEmpty();
    }

    // ── Key slot definitions ──────────────────────────────────────────────────

    [Fact]
    public void ItemEquipSlot_Merc_MainHand_HasValue2()
    {
        ((int)ItemEquipSlot.Merc_MainHand).Should().Be(2);
    }

    [Fact]
    public void ItemEquipSlot_Merc_Entity_HasValue1()
    {
        ((int)ItemEquipSlot.Merc_Entity).Should().Be(1);
    }

    [Fact]
    public void ItemEquipSlot_Beast_Entity_HasValue100()
    {
        ((int)ItemEquipSlot.Beast_Entity).Should().Be(100);
    }

    [Fact]
    public void ItemEquipSlot_Beast_Fang_HasValue101()
    {
        ((int)ItemEquipSlot.Beast_Fang).Should().Be(101);
    }

    // ── Helper: category check ────────────────────────────────────────────────

    private static bool IsForMercenary(ItemEquipSlot slot) =>
        (int)slot is >= 1 and <= 99;

    private static bool IsForBeast(ItemEquipSlot slot) =>
        (int)slot is >= 100 and <= 199;

    private static bool IsNonEquippable(ItemEquipSlot slot) =>
        slot == ItemEquipSlot.None;

    [Theory]
    [InlineData(ItemEquipSlot.Merc_MainHand, true)]
    [InlineData(ItemEquipSlot.Merc_Head, true)]
    [InlineData(ItemEquipSlot.Beast_Fang, false)]
    [InlineData(ItemEquipSlot.None, false)]
    public void IsForMercenary_ReturnsCorrectResult(ItemEquipSlot slot, bool expected)
    {
        IsForMercenary(slot).Should().Be(expected);
    }

    [Theory]
    [InlineData(ItemEquipSlot.Beast_Fang, true)]
    [InlineData(ItemEquipSlot.Beast_Core, true)]
    [InlineData(ItemEquipSlot.Merc_Head, false)]
    [InlineData(ItemEquipSlot.None, false)]
    public void IsForBeast_ReturnsCorrectResult(ItemEquipSlot slot, bool expected)
    {
        IsForBeast(slot).Should().Be(expected);
    }

    [Fact]
    public void IsNonEquippable_OnlyTrueForNone()
    {
        IsNonEquippable(ItemEquipSlot.None).Should().BeTrue();
        IsNonEquippable(ItemEquipSlot.Merc_MainHand).Should().BeFalse();
        IsNonEquippable(ItemEquipSlot.Beast_Fang).Should().BeFalse();
    }
}
