using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace todo.Models
{
    public class TodoListContext : DbContext
    {
        public TodoListContext(DbContextOptions<TodoListContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Title).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.Property(e => e.CreateAt).HasDefaultValueSql("getdate()").HasColumnType("datetime");
                entity.Property(e => e.UpdateAt).HasDefaultValueSql("getdate()").HasColumnType("datetime");

                entity.Property(e => e.FinishAt).HasColumnType("datetime");
                entity.Property(e => e.DeleteAt).HasColumnType("datetime");
                entity.Property(e => e.UserId).IsRequired();

            });

            modelBuilder.Entity<Todo>()
                .HasOne(t => t.User)
                .WithMany(u => u.Todos)
                .HasForeignKey(t => t.UserId);



            //User
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
            });


            //UserInfo
            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd().UseIdentityColumn();
                entity.Property(e => e.Address).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Birthday).IsRequired();
                entity.Property(e => e.Phone).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
            });
           
           modelBuilder.Entity<User>()
                .HasOne(i => i.UserInfo)
                .WithOne(u => u.User)
                .HasForeignKey<UserInfo>(i => i.UserId);


        }
        public DbSet<Todo> Todo { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
    }

   
}