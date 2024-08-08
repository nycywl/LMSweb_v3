using LMSwebDB.Models;
using LMSwebDB.Helper;
using Microsoft.EntityFrameworkCore;

namespace LMSwebDB.Data;

public partial class LMSContext : DbContext
{
    public LMSContext()
    {
    }

    public LMSContext(DbContextOptions<LMSContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Teacher> Teachers { get; set; }
    public virtual DbSet<Assistant> Assistants { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Material> Materials { get; set; }
    public virtual DbSet<Manage> Manages { get; set; }
    public virtual DbSet<QnA> QnAs { get; set; }
    public virtual DbSet<UserQALog> UserQALogs { get; set; }
    public virtual DbSet<StudentCourse> StudentCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.CourseId)
                .HasMaxLength(128)
                .HasColumnName("CourseID");
            entity.Property(e => e.CourseName)
                .HasMaxLength(128)
                .HasColumnName("CourseName");
            entity.Property(e => e.SystemPrompt)
                .HasColumnType("ntext")
                .HasColumnName("SystemPrompt");
            entity.Property(e => e.UserPrompt)
                .HasColumnType("ntext")
                .HasColumnName("UserPrompt");
            entity.Property(e => e.GreetingMessage)
                .HasColumnType("ntext")
                .HasColumnName("GreetingMessage");
            entity.Property(e => e.Temperature).HasColumnType("float").HasDefaultValue(0);
            entity.Property(e => e.IsNeedContext).HasColumnType("bit").HasDefaultValue(false);
            entity.Property(e => e.LLMModel)
                .HasMaxLength(128)
                .HasColumnName("LLMModel");
            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.TeacherId)
                .HasMaxLength(128)
                .HasColumnName("TeacherID");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Courses)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Courses_Teachers");

            entity.HasMany(d => d.Manages).WithOne(p => p.Course)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Courses_Manage");

            entity.HasMany(d => d.StudentCourses).WithOne(p => p.Course)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Courses_StudentCourses");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK_Student");

            entity.Property(e => e.StudentId)
                .HasMaxLength(128)
                .HasColumnName("StudentID");
            entity.Property(e => e.StudentName).HasDefaultValue("");

            entity.HasOne(d => d.StudentNavigation).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.StudentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Students_Users");

            entity.HasMany(d => d.StudentCourses).WithOne(p => p.Student)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Students_StudentCourses");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK_Teacher");
            entity.Property(e => e.TeacherId)
                .HasMaxLength(128)
                .HasColumnName("TeacherID");
            entity.Property(e => e.TeacherName).HasDefaultValue("");
        });

        modelBuilder.Entity<Assistant>(entity =>
        {
            entity.Property(e => e.AssistantId)
                .HasMaxLength(128)
                .HasColumnName("AssistantID");
            entity.Property(e => e.AssistantName).HasDefaultValue("");

            entity.HasOne(d => d.AssistantNavigation).WithOne(p => p.Assistant)
                .HasForeignKey<Assistant>(d => d.AssistantId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Assistants_Users");

            entity.HasMany(d => d.Manage).WithOne(p => p.Assistant)
                .HasForeignKey(d => d.AssistantId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Assistants_Manage");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId)
                .HasMaxLength(128)
                .HasColumnName("UserID");
            entity.Property(e => e.Name).HasMaxLength(128);
            entity.Property(e => e.RoleName).HasMaxLength(128);
            entity.Property(e => e.Upassword)
                .HasMaxLength(128)
                .HasColumnName("UPassword");

            entity.HasOne(d => d.Teacher).WithOne(p => p.TeacherNavigation)
                .HasForeignKey<Teacher>(d => d.TeacherId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Teachers_Users");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK_Material");
            entity.Property(e => e.MaterialId)
                .HasMaxLength(128)
                .HasColumnName("MaterialID");
            entity.Property(e => e.CourseID)
                .HasMaxLength(128)
                .HasColumnName("CourseID");
            entity.Property(e => e.FileName).HasMaxLength(128);
            entity.Property(e => e.UploadTime).HasColumnType("datetime");
            entity.HasOne(d => d.Course).WithMany(p => p.Materials)
                .HasForeignKey(d => d.CourseID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Material_Course");
        });

        modelBuilder.Entity<Manage>(entity =>
        {
            entity.HasKey(e => new { e.AssistantId, e.CourseId });
            entity.Property(e => e.AssistantId)
                .HasMaxLength(128)
                .HasColumnName("AssistantID");
            entity.Property(e => e.CourseId)
                .HasMaxLength(128)
                .HasColumnName("CourseID");
        });

        modelBuilder.Entity<QnA>(entity =>
        {
            entity.Property(e => e.QnAId)
                .HasMaxLength(128)
                .HasColumnName("QnAID");
            entity.Property(e => e.Answer).HasDefaultValue("");
            entity.Property(e => e.Embeddings).HasDefaultValue("");
            entity.Property(e => e.CourseId)
                .HasMaxLength(128)
                .HasColumnName("CourseID");
            entity.Property(e => e.MaterialId)
                .HasMaxLength(128)
                .HasColumnName("MaterialID");

            entity.HasOne(d => d.Material).WithMany(p => p.QnAs)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_QnA_Material");
        });

        modelBuilder.Entity<UserQALog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK_Log");
            entity.Property(e => e.LogId)
                .HasMaxLength(128)
                .HasColumnName("LogID");
            entity.Property(e => e.UserId)
                .HasMaxLength(128)
                .HasColumnName("StudentID");
            entity.Property(e => e.CourseId)
                .HasMaxLength(128)
                .HasColumnName("CourseID");
            entity.Property(e => e.MaterialId)
                .HasMaxLength(128)
                .HasColumnName("MaterialID");
            entity.Property(e => e.UserQuestion).HasDefaultValue("");
            entity.Property(e => e.AnswerFromGPT).HasDefaultValue("");
            entity.Property(e => e.Score).HasDefaultValue(0f);
            entity.Property(e => e.SystemPrompt).HasDefaultValue("");
            entity.Property(e => e.UserPrompt).HasDefaultValue("");
            entity.Property(e => e.PromptToken).HasDefaultValue(0);
            entity.Property(e => e.CompletionToken).HasDefaultValue(0);
            entity.Property(e => e.TotalToken).HasDefaultValue(0);
        });

        modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.StudentId, sc.CourseId });

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.CourseId);

        /* 測試資料 */
        var test_users = new List<User>
        {
            new User
            {
                UserId = "S001",
                Upassword = HashHelper.SHA256Hash("S001"),
                Name = "林小楷",
                RoleName = "Student"
            },
            new User
            {
                UserId = "S002",
                Upassword = HashHelper.SHA256Hash("S002"),
                Name = "李阿禎",
                RoleName = "Student"
            },
            new User
            {
                UserId = "S003",
                Upassword = HashHelper.SHA256Hash("S003"),
                Name = "許小琪",
                RoleName = "Student"
            },
            new User
            {
                UserId = "S004",
                Upassword = HashHelper.SHA256Hash("S004"),
                Name = "Kevin",
                RoleName = "Student"
            },
            new User
            {
                UserId = "S005",
                Upassword = HashHelper.SHA256Hash("S005"),
                Name = "Vivian",
                RoleName = "Student"
            },
            new User
            {
                UserId = "S006",
                Upassword = HashHelper.SHA256Hash("S006"),
                Name = "Amy",
                RoleName = "Student"
            },
            new User
            {
                UserId = "T001",
                Upassword = HashHelper.SHA256Hash("T00001"),
                Name = "林廣學",
                RoleName = "Teacher"
            },
            new User
            {
                UserId = "T002",
                Upassword = HashHelper.SHA256Hash("T002"),
                Name = "洪子秀 老師",
                RoleName = "Teacher"
            },
            new User
            {
                UserId = "T003",
                Upassword = HashHelper.SHA256Hash("T003"),
                Name = "曾秋蓉 老師",
                RoleName = "Teacher"
            },
            new User
            {
                UserId = "T004",
                Upassword = HashHelper.SHA256Hash("T004"),
                Name = "李偉 老師",
                RoleName = "Teacher"
            },
            new User
            {
                UserId = "T005",
                Upassword = HashHelper.SHA256Hash("T005"),
                Name = "蔡老師",
                RoleName = "Teacher"
            }
        };
        modelBuilder.Entity<User>().HasData(test_users);

        var test_students = new List<Student>
        {
            new Student
            {
                StudentId = "S001",
                StudentName = "林小楷"
            },
            new Student
            {
                StudentId = "S002",
                StudentName = "李阿禎"
            },
            new Student
            {
                StudentId = "S003",
                StudentName = "許小琪"
            },
            new Student
            {
                StudentId = "S004",
                StudentName = "Kevin"
            },
            new Student
            {
                StudentId = "S005",
                StudentName = "Vivian"
            },
            new Student
            {
                StudentId = "S006",
                StudentName = "Amy"
            }
        };
        modelBuilder.Entity<Student>().HasData(test_students);

        var test_teachers = new List<Teacher>
        {
            new Teacher
            {
                TeacherId = "T001",
                TeacherName = "陳立維"
            },
            new Teacher
            {
                TeacherId = "T002",
                TeacherName = "曾老師"
            },
            new Teacher
            {
                TeacherId = "T003",
                TeacherName = "李偉老師"
            },
            new Teacher
            {
                TeacherId = "T004",
                TeacherName = "焰超老師"
            },
            new Teacher
            {
                TeacherId = "T005",
                TeacherName = "蔡老師"
            }
        };
        modelBuilder.Entity<Teacher>().HasData(test_teachers);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
