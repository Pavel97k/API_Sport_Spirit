using Microsoft.EntityFrameworkCore;

namespace API_Sport_Spirit.Model;

public partial class SportSpiritDatebaseContext : DbContext
{
    public SportSpiritDatebaseContext()
    {
    }

    public SportSpiritDatebaseContext(DbContextOptions<SportSpiritDatebaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<CollectionServer> CollectionServers { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<ExerciseCriterion> ExerciseCriteria { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=PAVEL-ARDOR\\SERVER_SQL_ARDOR;Initial Catalog=Sport_Spirit_Datebase;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.IdAdministrator).HasName("PK_Administrator");

            entity.HasIndex(e => e.Login, "UQ_Login").IsUnique();

            entity.Property(e => e.IdAdministrator).HasColumnName("ID_Administrator");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password).IsUnicode(false);
        });

        modelBuilder.Entity<CollectionServer>(entity =>
        {
            entity.HasKey(e => e.IdCollectionServer);

            entity.ToTable("Collection_Server");

            entity.Property(e => e.IdCollectionServer).HasColumnName("ID_Collection_Server");
            entity.Property(e => e.AvailabilityBasicEquipment).HasColumnName("Availability_basic_equipment");
            entity.Property(e => e.CollectionServerMultiplicity)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Collection_Server_multiplicity");
            entity.Property(e => e.CollectionServerName)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("Collection_Server_name");
            entity.Property(e => e.CollectionServerType)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Collection_Server_type");
            entity.Property(e => e.GenderId).HasColumnName("Gender_ID");
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.IdExercise);

            entity.ToTable("Exercise");

            entity.Property(e => e.IdExercise).HasColumnName("ID_Exercise");
            entity.Property(e => e.CollectionServerId).HasColumnName("Collection_Server_ID");
            entity.Property(e => e.ExerciseCriteriaId).HasColumnName("Exercise_criteria_ID");
            entity.Property(e => e.ExerciseDescriptions)
                .IsUnicode(false)
                .HasColumnName("Exercise_descriptions");
            entity.Property(e => e.ExerciseName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Exercise_name");
            entity.Property(e => e.MuscleGroup)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("Muscle_group");
        });

        modelBuilder.Entity<ExerciseCriterion>(entity =>
        {
            entity.HasKey(e => e.IdExerciseCriteria);

            entity.ToTable("Exercise_criteria");

            entity.Property(e => e.IdExerciseCriteria).HasColumnName("ID_Exercise_criteria");
            entity.Property(e => e.ExecutionTime).HasColumnName("Execution_time");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.IdGender);

            entity.ToTable("Gender");

            entity.HasIndex(e => e.NameGender, "Name_Gender").IsUnique();

            entity.Property(e => e.IdGender).HasColumnName("ID_Gender");
            entity.Property(e => e.NameGender)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Name_Gender");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
