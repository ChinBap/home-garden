using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HomeGarden.Models;

public partial class HomeGardenDbContext : DbContext
{
    public HomeGardenDbContext()
    {
    }

    public HomeGardenDbContext(DbContextOptions<HomeGardenDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alert> Alerts { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<HealthDefinition> HealthDefinitions { get; set; }

    public virtual DbSet<Plant> Plants { get; set; }

    public virtual DbSet<PlantLog> PlantLogs { get; set; }

    public virtual DbSet<PlantResourceUsage> PlantResourceUsages { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<ResourceType> ResourceTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<StatusDefinition> StatusDefinitions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableDetailedErrors()
                     .EnableSensitiveDataLogging(); // Chỉ dùng trong development
        var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("MyCnn");
        optionsBuilder.UseSqlServer(ConnectionString);
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey(e => e.AlertId).HasName("PK__alerts__4B8FB03AE54D1A27");

            entity.ToTable("alerts");

            entity.Property(e => e.AlertId).HasColumnName("alert_id");
            entity.Property(e => e.AlertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("alert_date");
            entity.Property(e => e.AlertType)
                .HasMaxLength(100)
                .HasColumnName("alert_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .HasColumnName("message");
            entity.Property(e => e.PlantId).HasColumnName("plant_id");
            entity.Property(e => e.Resolved)
                .HasDefaultValue(false)
                .HasColumnName("resolved");
            entity.Property(e => e.ResolvedAt)
                .HasColumnType("datetime")
                .HasColumnName("resolved_at");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Plant).WithMany(p => p.Alerts)
                .HasForeignKey(d => d.PlantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__alerts__plant_id__7A672E12");

            entity.HasOne(d => d.Status).WithMany(p => p.Alerts)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__alerts__status_i__7B5B524B");
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__areas__985D6D6B239D401E");

            entity.ToTable("areas");

            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Status).WithMany(p => p.Areas)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__areas__status_id__5EBF139D");

            entity.HasOne(d => d.User).WithMany(p => p.Areas)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__areas__user_id__5DCAEF64");
        });

        modelBuilder.Entity<HealthDefinition>(entity =>
        {
            entity.HasKey(e => e.HealthId).HasName("PK__health_d__189960E39E5FD07E");

            entity.ToTable("health_definitions");

            entity.Property(e => e.HealthId).HasColumnName("health_id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Plant>(entity =>
        {
            entity.HasKey(e => e.PlantId).HasName("PK__plants__A576B3B458207BB9");

            entity.ToTable("plants");

            entity.Property(e => e.PlantId).HasColumnName("plant_id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.HealthId).HasColumnName("health_id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.PlantedDate).HasColumnName("planted_date");
            entity.Property(e => e.Species)
                .HasMaxLength(100)
                .HasColumnName("species");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Area).WithMany(p => p.Plants)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__plants__area_id__6383C8BA");

            entity.HasOne(d => d.Health).WithMany(p => p.Plants)
                .HasForeignKey(d => d.HealthId)
                .HasConstraintName("FK__plants__health_i__656C112C");

            entity.HasOne(d => d.Status).WithMany(p => p.Plants)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__plants__status_i__6477ECF3");
        });

        modelBuilder.Entity<PlantLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__plant_lo__9E2397E05E0A3742");

            entity.ToTable("plant_logs");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.Activity)
                .HasMaxLength(100)
                .HasColumnName("activity");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.HealthId).HasColumnName("health_id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.LogDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("log_date");
            entity.Property(e => e.PlantId).HasColumnName("plant_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Health).WithMany(p => p.PlantLogs)
                .HasForeignKey(d => d.HealthId)
                .HasConstraintName("FK__plant_log__healt__73BA3083");

            entity.HasOne(d => d.Plant).WithMany(p => p.PlantLogs)
                .HasForeignKey(d => d.PlantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__plant_log__plant__71D1E811");

            entity.HasOne(d => d.Status).WithMany(p => p.PlantLogs)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__plant_log__statu__72C60C4A");
        });

        modelBuilder.Entity<PlantResourceUsage>(entity =>
        {
            entity.HasKey(e => e.UsageId).HasName("PK__plant_re__B6B13A02021D9E92");

            entity.ToTable("plant_resource_usage");

            entity.Property(e => e.UsageId).HasColumnName("usage_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.PlantId).HasColumnName("plant_id");
            entity.Property(e => e.QuantityUsed)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("quantity_used");
            entity.Property(e => e.ResourceId).HasColumnName("resource_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UsedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("used_at");

            entity.HasOne(d => d.Plant).WithMany(p => p.PlantResourceUsages)
                .HasForeignKey(d => d.PlantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__plant_res__plant__09A971A2");

            entity.HasOne(d => d.Resource).WithMany(p => p.PlantResourceUsages)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__plant_res__resou__0A9D95DB");

            entity.HasOne(d => d.Status).WithMany(p => p.PlantResourceUsages)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__plant_res__statu__0B91BA14");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("PK__resource__4985FC730B20AE78");

            entity.ToTable("resources");

            entity.Property(e => e.ResourceId).HasColumnName("resource_id");
            entity.Property(e => e.Cost)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cost");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("quantity");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasDefaultValue("đơn vị")
                .HasColumnName("unit");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Status).WithMany(p => p.Resources)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resources__statu__03F0984C");

            entity.HasOne(d => d.Type).WithMany(p => p.Resources)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resources__type___02FC7413");

            entity.HasOne(d => d.User).WithMany(p => p.Resources)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__resources__user___02084FDA");
        });

        modelBuilder.Entity<ResourceType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__resource__2C0005985FC3A50F");

            entity.ToTable("resource_types");

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__roles__760965CCBC9A0AB9");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "UQ__roles__783254B1C41E1C9E").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Status).WithMany(p => p.Roles)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__roles__status_id__52593CB8");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__schedule__C46A8A6FB956B174");

            entity.ToTable("schedules");

            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Frequency)
                .HasMaxLength(50)
                .HasColumnName("frequency");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.LastDone)
                .HasColumnType("datetime")
                .HasColumnName("last_done");
            entity.Property(e => e.NextDue)
                .HasColumnType("datetime")
                .HasColumnName("next_due");
            entity.Property(e => e.PlantId).HasColumnName("plant_id");
            entity.Property(e => e.Reminder)
                .HasDefaultValue(true)
                .HasColumnName("reminder");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TaskType)
                .HasMaxLength(100)
                .HasColumnName("task_type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Plant).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.PlantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__schedules__plant__6B24EA82");

            entity.HasOne(d => d.Status).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__schedules__statu__6C190EBB");
        });

        modelBuilder.Entity<StatusDefinition>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__status_d__3683B53120DA08E7");

            entity.ToTable("status_definitions");

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Entity)
                .HasMaxLength(50)
                .HasColumnName("entity");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370F5B3BA653");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E616447E67C59").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(100)
                .HasColumnName("fullname");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__role_id__5812160E");

            entity.HasOne(d => d.Status).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__status_id__59063A47");
        });

        ApplySoftDeleteFilters(modelBuilder);
        OnModelCreatingPartial(modelBuilder);

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    private void ApplySoftDeleteFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasQueryFilter(x => x.IsDeleted == false || x.IsDeleted == null);
        modelBuilder.Entity<Role>().HasQueryFilter(x => x.IsDeleted == false || x.IsDeleted == null);
        modelBuilder.Entity<Area>().HasQueryFilter(x => x.IsDeleted == false || x.IsDeleted == null);
        modelBuilder.Entity<Plant>().HasQueryFilter(x => x.IsDeleted == false || x.IsDeleted == null);
        modelBuilder.Entity<Schedule>().HasQueryFilter(x => x.IsDeleted == false || x.IsDeleted == null);
        modelBuilder.Entity<PlantLog>().HasQueryFilter(x => x.IsDeleted == false || x.IsDeleted == null);
        modelBuilder.Entity<Alert>().HasQueryFilter(x => x.IsDeleted == false || x.IsDeleted == null);
        modelBuilder.Entity<Resource>().HasQueryFilter(x => x.IsDeleted == false || x.IsDeleted == null);
        modelBuilder.Entity<PlantResourceUsage>().HasQueryFilter(x => x.IsDeleted == false || x.IsDeleted == null);
    }


}
