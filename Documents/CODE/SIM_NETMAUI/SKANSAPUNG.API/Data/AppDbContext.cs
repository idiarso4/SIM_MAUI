using Microsoft.EntityFrameworkCore;
using SKANSAPUNG.API.Models;

namespace SKANSAPUNG.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tambahkan DbSet untuk setiap model yang ingin Anda jadikan tabel
        public DbSet<FcmToken> FcmTokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentDetail> StudentDetails { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<SchoolYear> SchoolYears { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<StudentScore> StudentScores { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Extracurricular> Extracurriculars { get; set; }
        public DbSet<ScheduleItem> ScheduleItems { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<StudentExtracurricular> StudentExtracurriculars { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<SchoolEvent> SchoolEvents { get; set; }
        
        // New models
        public DbSet<Office> Offices { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Announcement> Announcements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurasi model bisa ditambahkan di sini
            modelBuilder.Entity<FcmToken>().ToTable("fcm_tokens");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Student>().ToTable("students");
            modelBuilder.Entity<StudentDetail>().ToTable("student_details");
            modelBuilder.Entity<ClassRoom>().ToTable("class_rooms");
            modelBuilder.Entity<Department>().ToTable("departments");
            modelBuilder.Entity<SchoolYear>().ToTable("school_years");
            modelBuilder.Entity<Assessment>().ToTable("assessments");
            modelBuilder.Entity<StudentScore>().ToTable("student_scores");
            modelBuilder.Entity<Attendance>().ToTable("attendances");
            modelBuilder.Entity<Extracurricular>().ToTable("extracurriculars");
            modelBuilder.Entity<ScheduleItem>().ToTable("schedule_items");
            modelBuilder.Entity<Subject>().ToTable("subjects");
            modelBuilder.Entity<Teacher>().ToTable("teachers");
            modelBuilder.Entity<TeacherSubject>().ToTable("teacher_subjects")
                .HasKey(ts => new { ts.TeacherId, ts.SubjectId });
            modelBuilder.Entity<Schedule>().ToTable("schedules");
            modelBuilder.Entity<StudentExtracurricular>().ToTable("student_extracurriculars")
                .HasKey(se => new { se.StudentId, se.ExtracurricularId });
            modelBuilder.Entity<Gallery>().ToTable("galleries");
            modelBuilder.Entity<Notification>().ToTable("notifications");
            modelBuilder.Entity<Document>().ToTable("documents");
            modelBuilder.Entity<Event>().ToTable("events");
            modelBuilder.Entity<SchoolEvent>().ToTable("school_events");
            
            // New model configurations
            modelBuilder.Entity<Office>().ToTable("offices");
            modelBuilder.Entity<Shift>().ToTable("shifts");
            modelBuilder.Entity<Role>().ToTable("roles");
            modelBuilder.Entity<Permission>().ToTable("permissions");
            modelBuilder.Entity<Announcement>().ToTable("announcements");

            modelBuilder.Entity<StudentDetail>()
                .Property(s => s.Latitude)
                .HasColumnType("decimal(10, 8)");

            modelBuilder.Entity<StudentDetail>()
                .Property(s => s.Longitude)
                .HasColumnType("decimal(11, 8)");

            // Configure the one-to-one relationship between ClassRoom and User (as HomeroomTeacher)
            // This specifies that a ClassRoom has one HomeroomTeacher, and the foreign key is HomeroomTeacherId.
            // .WithOne() without a navigation property tells EF Core that there's no corresponding navigation property on the User entity for this relationship,
            // which resolves the ambiguity with the User.ClassRoom property.
            modelBuilder.Entity<ClassRoom>()
                .HasOne(c => c.HomeroomTeacher)
                .WithOne()
                .HasForeignKey<ClassRoom>(c => c.HomeroomTeacherId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 