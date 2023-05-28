using Core.Domain.Entities;
using Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContext
{
   public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
   {
      public DbSet<Room> Room { get; set; }
      public DbSet<Location> Location { get; set; }
      public DbSet<Image> Image { get; set; }
      public DbSet<Reservation> Reservation { get; set; }
      public DbSet<Review> Review { get; set; }
      public DbSet<Person> Person { get; set; }
      public ApplicationDbContext(DbContextOptions options) : base(options)
      {
      }
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);
      }
   }
}