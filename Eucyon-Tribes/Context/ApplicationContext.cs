using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;

namespace Eucyon_Tribes.Context
{
    public class ApplicationContext : DbContext
    {
        public virtual DbSet<Kingdom> Kingdoms { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<World> Worlds { get; set; }
        public virtual DbSet<Army> Armies { get; set; }
        public virtual DbSet<Battle> Battles { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Kingdom)
                .WithOne(k => k.User)
                .HasForeignKey<Kingdom>(k => k.UserId);

            modelBuilder.Entity<Kingdom>()
                .HasOne(k => k.Location)
                .WithOne(l => l.Kingdom)
                .HasForeignKey<Location>(l => l.KingdomId);

            modelBuilder.Entity<Kingdom>()
                .HasMany(k => k.Buildings)
                .WithOne(b => b.Kingdom)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Kingdom>()
                .HasMany(k => k.Resources)
                .WithOne(b => b.Kingdom)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<World>()
                .HasMany(w => w.Kingdoms)
                .WithOne(k => k.World)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<World>()
                .HasMany(w => w.Locations)
                .WithOne(k => k.World)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Building>()
                .HasDiscriminator<string>("Building Type")
                .HasValue<Barracks>("Barracks")
                .HasValue<Farm>("Farm")
                .HasValue<Mine>("Mine")
                .HasValue<Sawmill>("Sawmill")
                .HasValue<TownHall>("TownHall");

            modelBuilder.Entity<Resource>()
               .HasDiscriminator<string>("Resource Type")
               .HasValue<Soldier>("Soldier")
               .HasValue<Food>("Food")
               .HasValue<Gold>("Gold")
               .HasValue<Wood>("Wood")
               .HasValue<People>("People");

            modelBuilder.Entity<Army>()
               .HasMany(a => a.Soldiers)
               .WithOne(s => s.Army)
               .HasForeignKey(s => s.ArmyId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Kingdom>()
                .HasMany(k => k.Armies)
                .WithOne(a => a.Kingdom)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Kingdom>()
                .HasMany(b => b.AttackBattles)
                .WithOne(b => b.Attacker)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Kingdom>()
                .HasMany(b => b.DefendBattles)
                .WithOne(b => b.Defender)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}