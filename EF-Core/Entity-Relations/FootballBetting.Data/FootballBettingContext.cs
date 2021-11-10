using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        //for judge/outer connections
        public FootballBettingContext( DbContextOptions options) 
            : base(options)
        {
        }

        public FootballBettingContext()
        {
        }

        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<Color> Colors { get; set; }

        public virtual DbSet<Town> Towns { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Player> Players { get; set; }

        public virtual DbSet<Position> Positions { get; set; }

        public virtual DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public virtual DbSet<Game> Games { get; set; }

        public virtual DbSet<Bet> Bets { get; set; }

        public virtual DbSet<User> Users { get; set; }
        //to configure database relations (DDL)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PlayerStatistic>(e =>
            {
                e.HasKey(ps => new {ps.GameId, ps.PlayerId});
                
            });
            modelBuilder.Entity<Team>(e =>
            {
                e.HasOne(t => t.PrimaryKitColor)
                    .WithMany(c => c.PrimaryKitTeams)
                    .HasForeignKey(t => t.PrimaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict);
                e
                    .HasOne(t => t.SecondaryKitColor)
                    .WithMany(t => t.SecondaryKitTeams)
                    .HasForeignKey(t => t.SecondaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Game>(e =>
            {
                e.HasOne(x => x.AwayTeam)
                    .WithMany(x => x.AwayGames)
                    .HasForeignKey(x => x.AwayTeamId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.HomeTeam)
                    .WithMany(x => x.HomeGames)
                    .HasForeignKey(x => x.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
        
        //connecting to the server/database 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }
    }

  
}
