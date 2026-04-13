using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Domain.Items;
using MercenariesAndBeasts.Domain.Progress;
using MercenariesAndBeasts.Domain.Enums;
using static MercenariesAndBeasts.Domain.Items.ItemTemplate;

namespace MercenariesAndBeasts.Infrastructure;


  /*
  dotnet ef migrations remove  \
  -p src/MercenariesAndBeasts.Infrastructure \
  -s src/MercenariesAndBeasts.Web

    dotnet ef migrations add AddImagePathToEntities \
  -p src/MercenariesAndBeasts.Infrastructure \
  -s src/MercenariesAndBeasts.Web

dotnet ef database update \
  -p src/MercenariesAndBeasts.Infrastructure \
  -s src/MercenariesAndBeasts.Web
  */
public class MercenariesAndBeastsDbContext : IdentityDbContext<AppUser>
{
    public MercenariesAndBeastsDbContext(DbContextOptions<MercenariesAndBeastsDbContext> options)
        : base(options) { }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<LocalizedText> LocalizedTexts => Set<LocalizedText>();
    public DbSet<ItemUpgradeResource> ItemUpgradeResources => Set<ItemUpgradeResource>();

    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Dungeon> Dungeons => Set<Dungeon>();
    public DbSet<PlayerItemPieces> Pieces => Set<PlayerItemPieces>();

    public DbSet<PlayerProfile> Players => Set<PlayerProfile>();

    public DbSet<MercenaryTemplate> MercenaryTemplates => Set<MercenaryTemplate>();
    public DbSet<MonsterTemplate> MonsterTemplates => Set<MonsterTemplate>();

    public DbSet<PlayerMercenary> PlayerMercenaries => Set<PlayerMercenary>();
    public DbSet<PlayerMonster> PlayerMonsters => Set<PlayerMonster>();

    public DbSet<ItemTemplate> ItemTemplates => Set<ItemTemplate>();
    public DbSet<PlayerItem> PlayerItems => Set<PlayerItem>();

    public DbSet<ExpeditionStage> ExpeditionStages => Set<ExpeditionStage>();
    public DbSet<DungeonStage> DungeonStages => Set<DungeonStage>();

    public DbSet<PlayerExpeditionProgress> PlayerExpeditionProgresses => Set<PlayerExpeditionProgress>();
    public DbSet<PlayerDungeonProgress> PlayerDungeonProgresses => Set<PlayerDungeonProgress>();

    public DbSet<ExpeditionAchievementDefinition> ExpeditionAchievements => Set<ExpeditionAchievementDefinition>();
    public DbSet<PlayerExpeditionAchievement> PlayerExpeditionAchievementRows => Set<PlayerExpeditionAchievement>();
public DbSet<PlayerExpeditionAchievements> PlayerExpeditionAchievementSummaries => Set<PlayerExpeditionAchievements>();

    public DbSet<DungeonAchievementDefinition> DungeonAchievements => Set<DungeonAchievementDefinition>();
    public DbSet<PlayerDungeonAchievement> PlayerDungeonAchievements => Set<PlayerDungeonAchievement>();

    public DbSet<MercenaryEquipmentSlot> MercenaryEquipmentSlots => Set<MercenaryEquipmentSlot>();
    public DbSet<BeastEquipmentSlot> BeastEquipmentSlots => Set<BeastEquipmentSlot>();

    public DbSet<PlayerMercenarySlot> PlayerMercenarySlots => Set<PlayerMercenarySlot>();
    public DbSet<PlayerBeastSlot> PlayerBeastSlots => Set<PlayerBeastSlot>();

    public DbSet<PlayerItemDiscovery> PlayerItemDiscoveries => Set<PlayerItemDiscovery>();

    public DbSet<StatScaling> StatScalings => Set<StatScaling>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // ------------------------------------------------------------
    // COUNTRY / PROFILE
    // ------------------------------------------------------------
    modelBuilder.Entity<Country>(e =>
    {
        e.HasKey(x => x.Code);
        e.Property(x => x.Code).HasMaxLength(2);
        e.Property(x => x.Iso3).HasMaxLength(3);

        e.HasIndex(x => x.NameEn);
        e.HasIndex(x => x.Continent);
    });

    modelBuilder.Entity<PlayerProfile>(e =>
    {
        e.Property(x => x.CountryCode).HasMaxLength(2);
        e.HasIndex(x => x.CountryCode);

        // volitelně FK na Countries, pokud chceš:
        // e.HasOne<Country>()
        //  .WithMany()
        //  .HasForeignKey(x => x.CountryCode)
        //  .HasPrincipalKey(c => c.Code)
        //  .OnDelete(DeleteBehavior.SetNull);
    });

    // ------------------------------------------------------------
    // LOCATION / DUNGEON
    // ------------------------------------------------------------
    modelBuilder.Entity<Location>(entity =>
    {
        entity.HasKey(l => l.Id);
        entity.HasIndex(l => l.Code).IsUnique();

        entity.Property(l => l.Code).IsRequired().HasMaxLength(64);
        entity.Property(l => l.NameEn).IsRequired().HasMaxLength(128);

        entity.Property(l => l.Element)
              .HasConversion<string>()
              .HasMaxLength(32);

        entity.Property(l => l.UnlockOrder).IsRequired();
    });

    modelBuilder.Entity<Dungeon>(entity =>
    {
        entity.HasKey(d => d.Id);
        entity.HasIndex(d => d.Code).IsUnique();

        entity.Property(d => d.Code).IsRequired().HasMaxLength(64);
        entity.Property(d => d.NameEn).IsRequired().HasMaxLength(128);

        entity.Property(d => d.Element)
              .HasConversion<string>()
              .HasMaxLength(32);

        entity.Property(d => d.UnlockOrder).IsRequired();
    });

    // ------------------------------------------------------------
    // OWNED TYPES: StatBlock atd.
    // ------------------------------------------------------------
    modelBuilder.Entity<MercenaryTemplate>().OwnsOne(x => x.BaseStats);
    modelBuilder.Entity<MonsterTemplate>().OwnsOne(x => x.BaseStats);

    modelBuilder.Entity<PlayerMercenary>().OwnsOne(x => x.BonusStats);
    modelBuilder.Entity<PlayerMonster>().OwnsOne(x => x.BonusStats);

    modelBuilder.Entity<ItemTemplate>().OwnsOne(x => x.BaseStats);
    modelBuilder.Entity<PlayerItem>().OwnsOne(x => x.BonusStats);

    // ------------------------------------------------------------
    // EQUIPMENT SLOTS
    // ------------------------------------------------------------
    modelBuilder.Entity<MercenaryEquipmentSlot>(e =>
    {
        e.HasKey(x => x.Id);

        e.HasOne<PlayerMercenary>()
            .WithMany(m => m.Equipment)
            .HasForeignKey(x => x.MercenaryInstanceId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.PlayerItem)
            .WithMany()
            .HasForeignKey(x => x.PlayerItemId)
            .OnDelete(DeleteBehavior.SetNull);

        e.HasIndex(x => new { x.MercenaryInstanceId, x.Slot }).IsUnique();
    });

    modelBuilder.Entity<BeastEquipmentSlot>(e =>
    {
        e.HasKey(x => x.Id);

        e.HasOne<PlayerMonster>()
            .WithMany(b => b.Equipment)
            .HasForeignKey(x => x.BeastInstanceId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.PlayerItem)
            .WithMany()
            .HasForeignKey(x => x.PlayerItemId)
            .OnDelete(DeleteBehavior.SetNull);

        e.HasIndex(x => new { x.BeastInstanceId, x.Slot }).IsUnique();
    });

    // ------------------------------------------------------------
    // PARTY SLOTS
    // ------------------------------------------------------------
    modelBuilder.Entity<PlayerMercenarySlot>(e =>
    {
        e.HasOne(s => s.Mercenary)
            .WithMany()
            .HasForeignKey(s => s.MercenaryInstanceId)
            .OnDelete(DeleteBehavior.SetNull);
    });

    modelBuilder.Entity<PlayerBeastSlot>(e =>
    {
        e.HasOne(s => s.Beast)
            .WithMany()
            .HasForeignKey(s => s.BeastInstanceId)
            .OnDelete(DeleteBehavior.SetNull);
    });

    // ------------------------------------------------------------
    // DISCOVERY
    // ------------------------------------------------------------
    modelBuilder.Entity<PlayerItemDiscovery>(e =>
    {
        e.HasIndex(x => new { x.PlayerId, x.TemplateId }).IsUnique();
    });

    // ------------------------------------------------------------
    // STAT SCALING
    // ------------------------------------------------------------
    modelBuilder.Entity<StatScaling>(e =>
    {
        e.ToTable("StatScaling");
        e.HasKey(x => x.Id);
    });

    modelBuilder.Entity<ItemTemplate>(e =>
    {
        e.HasOne(x => x.Scaling)
            .WithMany()
            .HasForeignKey(x => x.ScalingId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    });

    // ------------------------------------------------------------
    // PROGRESS: Expedition / Dungeon (unikátní progress pro hráče+mapu)
    // ------------------------------------------------------------
    modelBuilder.Entity<PlayerExpeditionProgress>(e =>
    {
        e.HasIndex(p => new { p.PlayerId, p.LocationId }).IsUnique();
    });

    modelBuilder.Entity<PlayerDungeonProgress>(e =>
    {
        e.HasIndex(p => new { p.PlayerId, p.DungeonId }).IsUnique();
    });

    // ------------------------------------------------------------
    // ACHIEVEMENTS DEFINITIONS (doporučené, ať máš FK pravidla)
    // ------------------------------------------------------------
    modelBuilder.Entity<ExpeditionAchievementDefinition>(e =>
    {
        e.HasKey(x => x.Id);
        e.Property(x => x.Code).HasMaxLength(64);
        e.Property(x => x.NameEn).HasMaxLength(128);
        e.Property(x => x.DescriptionEn).HasMaxLength(2048);

        e.HasIndex(x => new { x.LocationId, x.Index }).IsUnique();
        e.HasIndex(x => new { x.LocationId, x.Code }).IsUnique();

        e.HasOne(x => x.Location)
            .WithMany()
            .HasForeignKey(x => x.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<DungeonAchievementDefinition>(e =>
    {
        e.HasKey(x => x.Id);
        e.Property(x => x.Code).HasMaxLength(64);
        e.Property(x => x.NameEn).HasMaxLength(128);
        e.Property(x => x.DescriptionEn).HasMaxLength(2048);

        e.HasIndex(x => new { x.DungeonId, x.Index }).IsUnique();
        e.HasIndex(x => new { x.DungeonId, x.Code }).IsUnique();

        e.HasOne(x => x.Dungeon)
            .WithMany()
            .HasForeignKey(x => x.DungeonId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    // ------------------------------------------------------------
    // ACHIEVEMENTS: Progress -> Many Achievements
    // ------------------------------------------------------------

    // Expedition: pokud máš navigaci PlayerExpeditionProgress.Achievements, použij ji.
    modelBuilder.Entity<PlayerExpeditionAchievement>()
        .HasOne(a => a.Progress)
        .WithMany() // ✅ Progress nemá kolekci navigaci
        .HasForeignKey(a => a.ProgressId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<PlayerDungeonProgress>()
        .HasMany(p => p.Achievements)
        .WithOne(a => a.Progress)
        .HasForeignKey(a => a.ProgressId)
        .OnDelete(DeleteBehavior.Cascade);

    // PlayerExpeditionAchievement (ROWS) – vlastní tabulka + správné unikátní indexy
    modelBuilder.Entity<PlayerExpeditionAchievement>(e =>
    {
        e.ToTable("PlayerExpeditionAchievement");
        e.HasKey(x => x.Id);

        e.HasOne(x => x.Player)
            .WithMany()
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Progress)
            .WithMany() // pokud nemáš navigaci, dej .WithMany()
            .HasForeignKey(x => x.ProgressId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Achievement)
            .WithMany()
            .HasForeignKey(x => x.AchievementId)
            .OnDelete(DeleteBehavior.Restrict);

        // ✅ správně: unikátní v rámci progressu na definici achievementu
        e.HasIndex(x => new { x.ProgressId, x.AchievementId }).IsUnique();
        // volitelně: ještě pojistka pro PlayerId
        e.HasIndex(x => new { x.PlayerId, x.ProgressId, x.AchievementId }).IsUnique();
    });

    // PlayerDungeonAchievement (ROWS)
    modelBuilder.Entity<PlayerDungeonAchievement>(e =>
    {
        e.ToTable("PlayerDungeonAchievement");
        e.HasKey(x => x.Id);

        e.HasOne(x => x.Player)
            .WithMany()
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Progress)
            .WithMany(p => p.Achievements) // pokud nemáš navigaci, dej .WithMany()
            .HasForeignKey(x => x.ProgressId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Achievement)
            .WithMany()
            .HasForeignKey(x => x.AchievementId)
            .OnDelete(DeleteBehavior.Restrict);

        // ✅ správně: více achievementů per progress
        e.HasIndex(x => new { x.ProgressId, x.AchievementId }).IsUnique();
        e.HasIndex(x => new { x.PlayerId, x.ProgressId, x.AchievementId }).IsUnique();
    });

    // ------------------------------------------------------------
    // EXPEDITION SUMMARY (jiná tabulka než ROWS)
    // ------------------------------------------------------------
    modelBuilder.Entity<PlayerExpeditionAchievements>(e =>
    {
        e.ToTable("PlayerExpeditionAchievementSummary");
        e.HasKey(x => x.Id);

        e.HasOne(x => x.Player)
            .WithMany()
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Location)
            .WithMany()
            .HasForeignKey(x => x.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasIndex(x => new { x.PlayerId, x.LocationId }).IsUnique();
    });

    // ------------------------------------------------------------
    // LOCALIZATION
    // ------------------------------------------------------------
    modelBuilder.Entity<LocalizedText>(e =>
    {
        e.HasKey(x => x.Id);

        e.Property(x => x.EntityType).HasMaxLength(64).IsRequired();
        e.Property(x => x.Culture).HasMaxLength(12).IsRequired();
        e.Property(x => x.Name).HasMaxLength(256).IsRequired();

        e.HasIndex(x => new { x.EntityType, x.EntityId, x.Culture }).IsUnique();
        e.HasIndex(x => new { x.EntityType, x.EntityId });
        e.HasIndex(x => x.Culture);
    });

    // ------------------------------------------------------------
    // ITEM UPGRADE RESOURCES
    // ------------------------------------------------------------
    modelBuilder.Entity<ItemUpgradeResource>(e =>
    {
        e.HasKey(x => x.Id);

        e.HasOne(x => x.ItemTemplate)
            .WithMany(t => t.UpgradeResources)
            .HasForeignKey(x => x.ItemTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasIndex(x => new { x.ItemTemplateId, x.Type }).IsUnique();

        e.Property(x => x.NameEn).HasMaxLength(128).IsRequired();
        e.Property(x => x.DescriptionEn).HasMaxLength(2048);
    });
    modelBuilder.Entity<PlayerMercenary>()
    .OwnsOne(x => x.BonusStats, sb =>
    {
        sb.Property(p => p.Element)
          .HasConversion<string>()
          .HasMaxLength(32);
    });

modelBuilder.Entity<PlayerMonster>()
    .OwnsOne(x => x.BonusStats, sb =>
    {
        sb.Property(p => p.Element)
          .HasConversion<string>()
          .HasMaxLength(32);
    });

modelBuilder.Entity<PlayerItem>()
    .OwnsOne(x => x.BonusStats, sb =>
    {
        sb.Property(p => p.Element)
          .HasConversion<string>()
          .HasMaxLength(32);
    });

// stejně i BaseStats, pokud ho máš a obsahuje Element:
modelBuilder.Entity<MercenaryTemplate>()
    .OwnsOne(x => x.BaseStats, sb =>
    {
        sb.Property(p => p.Element)
          .HasConversion<string>()
          .HasMaxLength(32);
    });

modelBuilder.Entity<MonsterTemplate>()
    .OwnsOne(x => x.BaseStats, sb =>
    {
        sb.Property(p => p.Element)
          .HasConversion<string>()
          .HasMaxLength(32);
    });

modelBuilder.Entity<ItemTemplate>()
    .OwnsOne(x => x.BaseStats, sb =>
    {
        sb.Property(p => p.Element)
          .HasConversion<string>()
          .HasMaxLength(32);
    });
    modelBuilder.Entity<PlayerItem>(e =>
{
    e.Property(x => x.BadgeTier).HasConversion<int>();
    e.Property(x => x.Wins).IsRequired();
});
modelBuilder.Entity<PlayerMercenarySlot>()
    .HasOne(s => s.PlayerProfile)
    .WithMany(p => p.MercenarySlots)
    .HasForeignKey(s => s.PlayerProfileId);

modelBuilder.Entity<PlayerMercenarySlot>()
    .HasOne(s => s.Mercenary)
    .WithMany()
    .HasForeignKey(s => s.MercenaryInstanceId);
}
}