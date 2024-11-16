using jobSeeker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .ToTable("Posts")
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                 .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Post>()
                .HasMany(p => p.Likes)
                .WithOne(l => l.Post)
                .HasForeignKey(l => l.PostId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Shares)
                .WithOne(s => s.Post)
                .HasForeignKey(s => s.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Post)
                .HasForeignKey(i => i.PostId)
                 .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Story>()
           .HasOne(s => s.User)
           .WithMany() 
           .HasForeignKey(s => s.UserId)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SavedPost>()
                .HasOne(sp => sp.User)
                .WithMany()
                .HasForeignKey(sp => sp.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Experience>()
            .HasOne(e => e.User)
            .WithMany(u => u.Experiences)
            .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Certificate>()
       .HasOne(c => c.User)
       .WithMany(u => u.Certificates)
       .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Education>()
       .HasOne(e => e.User)
       .WithMany(u => u.Educations)
       .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Company>()
        .HasOne(c => c.User)
        .WithOne(u => u.Company)
        .HasForeignKey<Company>(c => c.UserId)
        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobPosting>()
    .HasOne<Company>()
    .WithMany()
    .HasForeignKey(jp => jp.CompanyId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
    .ToTable("Payments")
    .HasOne(p => p.JobPosting)
    .WithMany()
    .HasForeignKey(p => p.JobId)
    .OnDelete(DeleteBehavior.Cascade);
        }

        
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserOTP> UserOTPs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<SavedPost> SavedPosts { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<Payment> Payments { get; set; }

    }
}
