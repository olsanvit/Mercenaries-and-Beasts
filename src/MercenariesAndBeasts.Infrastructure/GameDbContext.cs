using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Domain.Units;
using MercenariesAndBeasts.Domain.Items;
using MercenariesAndBeasts.Domain.Progress;

namespace MercenariesAndBeasts.Infrastructure;

public class GameDbContext : IdentityDbContext<AppUser>
{
    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options) { }

        public DbSet<Location> Locations => Set<Location>();
    public DbSet<PlayerProfile> Players => Set<PlayerProfile>();
        public DbSet<MercenaryTemplate> MercenaryTemplates => Set<MercenaryTemplate>();
        public DbSet<MonsterTemplate> MonsterTemplates => Set<MonsterTemplate>();
        public DbSet<PlayerMercenary> PlayerMercenaries => Set<PlayerMercenary>();
        public DbSet<PlayerMonster> PlayerMonsters => Set<PlayerMonster>();
        public DbSet<ItemTemplate> ItemTemplates => Set<ItemTemplate>();
        public DbSet<PlayerItem> PlayerItems => Set<PlayerItem>();

        public DbSet<ExpeditionStage> ExpeditionStages => Set<ExpeditionStage>();
        public DbSet<Dungeon> Dungeons => Set<Dungeon>();
        public DbSet<DungeonStage> DungeonStages => Set<DungeonStage>();

        public DbSet<PlayerExpeditionProgress> PlayerExpeditionProgresses => Set<PlayerExpeditionProgress>();
        public DbSet<PlayerDungeonProgress> PlayerDungeonProgresses => Set<PlayerDungeonProgress>();
        public DbSet<ExpeditionAchievementDefinition> ExpeditionAchievements => Set<ExpeditionAchievementDefinition>();
        public DbSet<PlayerExpeditionAchievement> PlayerExpeditionAchievements => Set<PlayerExpeditionAchievement>();
        public DbSet<DungeonAchievementDefinition> DungeonAchievements => Set<DungeonAchievementDefinition>();
        public DbSet<PlayerDungeonAchievement> PlayerDungeonAchievements => Set<PlayerDungeonAchievement>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // DŮLEŽITÉ: nechá Identity nastavit tabulky

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.HasIndex(l => l.Code).IsUnique();
            entity.Property(l => l.Code).IsRequired().HasMaxLength(64);
            entity.Property(l => l.NameEn).IsRequired().HasMaxLength(128);
            entity.Property(l => l.Element).IsRequired().HasMaxLength(32);
            entity.Property(l => l.UnlockOrder).IsRequired();
        });

        modelBuilder.Entity<Dungeon>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.HasIndex(d => d.Code).IsUnique();
            entity.Property(d => d.Code).IsRequired().HasMaxLength(64);
            entity.Property(d => d.NameEn).IsRequired().HasMaxLength(128);
            entity.Property(d => d.Element).IsRequired().HasMaxLength(32);
            entity.Property(d => d.UnlockOrder).IsRequired();
        });
         modelBuilder.Entity<MercenaryTemplate>()
                .OwnsOne(x => x.BaseStats);

            modelBuilder.Entity<MonsterTemplate>()
                .OwnsOne(x => x.BaseStats);

            modelBuilder.Entity<PlayerMercenary>()
                .OwnsOne(x => x.BonusStats);

            modelBuilder.Entity<PlayerMonster>()
                .OwnsOne(x => x.BonusStats);

            modelBuilder.Entity<ItemTemplate>()
                .OwnsOne(x => x.BaseStats);

            modelBuilder.Entity<PlayerItem>()
                .OwnsOne(x => x.BonusStats);
    }
}


public class ExpeditionLocation
{
}