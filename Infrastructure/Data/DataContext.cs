using System;
using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Folder> Folders { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Folder>()
            .HasOne(e => e.AppUser)
            .WithMany(e => e.Folders)
            .HasForeignKey(e => e.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
