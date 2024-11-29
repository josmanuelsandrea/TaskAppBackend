using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TaskApp.Infrastructure.Persistence.PostgreSQL.Models;

namespace TaskApp.Infrastructure.Persistence.PostgreSQL;

public partial class TaskappContext : DbContext
{
    public TaskappContext()
    {
    }

    public TaskappContext(DbContextOptions<TaskappContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TaskM> TasksM { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskM>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tasks_pkey");

            entity.ToTable("tasks");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'pending'::text")
                .HasColumnName("status");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.TasksM)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("tasks_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Username).HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
