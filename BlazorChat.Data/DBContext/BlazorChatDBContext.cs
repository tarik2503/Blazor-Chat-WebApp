using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorChat.Data.DBContext
{
    public class BlazorChatDBContext : IdentityDbContext<User>
    {
        public BlazorChatDBContext(DbContextOptions<BlazorChatDBContext> options) : base(options)
        {

        }
        public DbSet<MessageChat> MessageChats { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<Groups> UserGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MessageChat>(entity =>
            {
                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.MessageChatsFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.MessageChatsToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<GroupChat>(entity =>
            {
                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.GroupChatsFromUsers)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupMessages)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<Groups>(entity =>
            {
                entity.HasOne(d => d.CreateGroup)
                    .WithMany(p => p.GroupsByUser)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull);
              
            });

            builder.Entity<IdentityRole>().HasData(
                 new IdentityRole
                 {
                     Id = "bab5ce50-84b3-456c-98f1-d7ff6d40e46e",
                     Name = "Admin",
                     NormalizedName = "ADMIN"
                 },
                 new IdentityRole
                 {
                    Id = "be4a0261-3e09-46cf-896a-74275d5ef8cd",
                    Name = "User",
                    NormalizedName = "USER"
                 });
        }
    }
}
