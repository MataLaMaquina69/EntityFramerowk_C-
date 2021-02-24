using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using System;
using Microsoft.Extensions.Logging;

namespace SamuraiApp.Data
{
    //Dbcontect: proides logic for EF core to interact with ur db
    //when theresnt a dbset of a table that matches the conventionf naming it map it from the class name but it can be used the to table method to match the convention 
    public class SamuraiContext: DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        //the snapshot is used to dedtermine howw to migrate from one model version ot the next 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=SamuraiAppData"
                ).LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name,
                DbLoggerCategory.Database.Transaction.Name},
                LogLevel.Debug);
                //.EnableSensitiveDataLogging();
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)
                .UsingEntity<SamuraiBattle>
                (bs => bs.HasOne<Battle>().WithMany(),
                bs => bs.HasOne<Samurai>().WithMany())
                .Property(bs => bs.DateJoined)
                .HasDefaultValueSql("getdate()");

            //in case it changes the name of the table so the migration doesnt drop the table and all its records just like it happended to me
            modelBuilder.Entity<Horse>().ToTable("Horses");
            modelBuilder.Entity<SamuraiBattle>().ToTable("BattleSamurai");
        }
    }
}
