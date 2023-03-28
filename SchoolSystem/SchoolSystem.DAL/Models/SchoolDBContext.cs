using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SchoolSystem.DAL.Models
{
    public partial class SchoolDBContext : DbContext
    {
        public SchoolDBContext()
        {
        }

        public SchoolDBContext(DbContextOptions<SchoolDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<CoursesSection> CoursesSections { get; set; } = null!;
        public virtual DbSet<File> Files { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<QuestionsAnswer> QuestionsAnswers { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Test> Tests { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UsersTest> UsersTests { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SchoolDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasMany(d => d.Users)
                    .WithMany(p => p.Courses)
                    .UsingEntity<Dictionary<string, object>>(
                        "UsersCourse",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UsersCour__UserI__4AB81AF0"),
                        r => r.HasOne<Course>().WithMany().HasForeignKey("CourseId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__UsersCour__Cours__49C3F6B7"),
                        j =>
                        {
                            j.HasKey("CourseId", "UserId").HasName("PK__UsersCou__1855FD637F582BCD");

                            j.ToTable("UsersCourses");

                            j.IndexerProperty<string>("CourseId").HasMaxLength(36).IsUnicode(false);

                            j.IndexerProperty<string>("UserId").HasMaxLength(36).IsUnicode(false);
                        });
            });

            modelBuilder.Entity<CoursesSection>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CourseId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CoursesSections)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CoursesSe__Cours__4E88ABD4");

                entity.HasMany(d => d.Files)
                    .WithMany(p => p.CourseSections)
                    .UsingEntity<Dictionary<string, object>>(
                        "CoursesSectionsFile",
                        l => l.HasOne<File>().WithMany().HasForeignKey("FileId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__CoursesSe__FileI__5629CD9C"),
                        r => r.HasOne<CoursesSection>().WithMany().HasForeignKey("CourseSectionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__CoursesSe__Cours__5535A963"),
                        j =>
                        {
                            j.HasKey("CourseSectionId", "FileId").HasName("PK__CoursesS__2C44FD1890B04E59");

                            j.ToTable("CoursesSectionsFiles");

                            j.IndexerProperty<string>("CourseSectionId").HasMaxLength(36).IsUnicode(false);

                            j.IndexerProperty<string>("FileId").HasMaxLength(36).IsUnicode(false);
                        });

                entity.HasMany(d => d.Tests)
                    .WithMany(p => p.CourseSections)
                    .UsingEntity<Dictionary<string, object>>(
                        "CoursesSectionsTest",
                        l => l.HasOne<Test>().WithMany().HasForeignKey("TestId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__CoursesSe__TestI__52593CB8"),
                        r => r.HasOne<CoursesSection>().WithMany().HasForeignKey("CourseSectionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__CoursesSe__Cours__5165187F"),
                        j =>
                        {
                            j.HasKey("CourseSectionId", "TestId").HasName("PK__CoursesS__D278378504DFC37F");

                            j.ToTable("CoursesSectionsTests");

                            j.IndexerProperty<string>("CourseSectionId").HasMaxLength(36).IsUnicode(false);

                            j.IndexerProperty<string>("TestId").HasMaxLength(36).IsUnicode(false);
                        });
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Filename)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DateOfCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(1024)
                    .IsUnicode(false);

                entity.HasMany(d => d.Tests)
                    .WithMany(p => p.Questions)
                    .UsingEntity<Dictionary<string, object>>(
                        "QuestionsTest",
                        l => l.HasOne<Test>().WithMany().HasForeignKey("TestId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Questions__TestI__4316F928"),
                        r => r.HasOne<Question>().WithMany().HasForeignKey("QuestionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Questions__Quest__4222D4EF"),
                        j =>
                        {
                            j.HasKey("QuestionId", "TestId").HasName("PK__Question__150C5CBAEF40D074");

                            j.ToTable("QuestionsTests");

                            j.IndexerProperty<string>("QuestionId").HasMaxLength(36).IsUnicode(false);

                            j.IndexerProperty<string>("TestId").HasMaxLength(36).IsUnicode(false);
                        });
            });

            modelBuilder.Entity<QuestionsAnswer>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.AnswerId })
                    .HasName("PK__Question__50884AACB82DB4C0");

                entity.Property(e => e.QuestionId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.AnswerId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.QuestionsAnswers)
                    .HasForeignKey(d => d.AnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Questions__Answe__46E78A0C");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionsAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Questions__Quest__45F365D3");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DateOfCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DateOfCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfCreation).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FileId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.HasOne(d => d.File)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.FileId)
                    .HasConstraintName("FK__Users__FileId__3B75D760");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__RoleId__3A81B327");
            });

            modelBuilder.Entity<UsersTest>(entity =>
            {
                entity.HasKey(e => new { e.TestId, e.UserId })
                    .HasName("PK__UsersTes__5DBBBDA4FB129E0F");

                entity.Property(e => e.TestId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.UsersTests)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UsersTest__TestI__3E52440B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersTests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UsersTest__UserI__3F466844");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
