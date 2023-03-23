using AW_lab10.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AW_lab10.Data
{
    public class ArticleDbContext : IdentityDbContext
    {
        public ArticleDbContext(DbContextOptions<ArticleDbContext> options): base(options)
        {

        }
        public DbSet<Article> Article { get; set; }
        public DbSet<Category> Category { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Articles)
                .WithOne(a => a.Category)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.CategoryId);
        }
    }

}
