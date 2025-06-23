using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SKANSAPUNG.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Kode = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "extracurriculars",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nama = table.Column<string>(type: "text", nullable: false),
                    Deskripsi = table.Column<string>(type: "text", nullable: false),
                    Hari = table.Column<string>(type: "text", nullable: false),
                    JamMulai = table.Column<TimeSpan>(type: "interval", nullable: false),
                    JamSelesai = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Tempat = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_extracurriculars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fcm_tokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: false),
                    DeviceId = table.Column<string>(type: "text", nullable: false),
                    Platform = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fcm_tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "schedule_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubjectName = table.Column<string>(type: "text", nullable: false),
                    TeacherName = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    DayOfWeek = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule_items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "school_years",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tahun = table.Column<string>(type: "text", nullable: false),
                    Semester = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_school_years", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "assessments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassRoomId = table.Column<long>(type: "bigint", nullable: false),
                    TeacherId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    AssessmentName = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "attendances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ScheduleLatitude = table.Column<double>(type: "double precision", nullable: false),
                    ScheduleLongitude = table.Column<double>(type: "double precision", nullable: false),
                    ScheduleStartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ScheduleEndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    StartLatitude = table.Column<double>(type: "double precision", nullable: false),
                    StartLongitude = table.Column<double>(type: "double precision", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    IsLeave = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndLatitude = table.Column<double>(type: "double precision", nullable: true),
                    EndLongitude = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "class_rooms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    SchoolYearId = table.Column<long>(type: "bigint", nullable: false),
                    HomeroomTeacherId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_class_rooms_departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_rooms_school_years_SchoolYearId",
                        column: x => x.SchoolYearId,
                        principalTable: "school_years",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    EmailVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: false),
                    RememberToken = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: false),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ClassRoomId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_class_rooms_ClassRoomId",
                        column: x => x.ClassRoomId,
                        principalTable: "class_rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "student_details",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Nipd = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    Nisn = table.Column<string>(type: "text", nullable: false),
                    BirthPlace = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Nik = table.Column<string>(type: "text", nullable: false),
                    Religion = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Rt = table.Column<string>(type: "text", nullable: false),
                    Rw = table.Column<string>(type: "text", nullable: false),
                    Dusun = table.Column<string>(type: "text", nullable: false),
                    Kelurahan = table.Column<string>(type: "text", nullable: false),
                    Kecamatan = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    ResidenceType = table.Column<string>(type: "text", nullable: false),
                    Transportation = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    MobilePhone = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Skhun = table.Column<string>(type: "text", nullable: false),
                    KpsRecipient = table.Column<bool>(type: "boolean", nullable: false),
                    KpsNumber = table.Column<string>(type: "text", nullable: false),
                    ClassGroup = table.Column<string>(type: "text", nullable: false),
                    UnNumber = table.Column<string>(type: "text", nullable: false),
                    IjazahNumber = table.Column<string>(type: "text", nullable: false),
                    KipRecipient = table.Column<bool>(type: "boolean", nullable: false),
                    KipNumber = table.Column<string>(type: "text", nullable: false),
                    KipName = table.Column<string>(type: "text", nullable: false),
                    KksNumber = table.Column<string>(type: "text", nullable: false),
                    BirthCertificateNo = table.Column<string>(type: "text", nullable: false),
                    BankName = table.Column<string>(type: "text", nullable: false),
                    BankAccountNumber = table.Column<string>(type: "text", nullable: false),
                    BankAccountHolder = table.Column<string>(type: "text", nullable: false),
                    PipEligible = table.Column<bool>(type: "boolean", nullable: false),
                    PipEligibleReason = table.Column<string>(type: "text", nullable: false),
                    SpecialNeeds = table.Column<string>(type: "text", nullable: false),
                    PreviousSchool = table.Column<string>(type: "text", nullable: false),
                    ChildOrder = table.Column<int>(type: "integer", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric(10,8)", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric(11,8)", nullable: true),
                    KkNumber = table.Column<string>(type: "text", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric", nullable: true),
                    Height = table.Column<decimal>(type: "numeric", nullable: true),
                    HeadCircumference = table.Column<decimal>(type: "numeric", nullable: true),
                    SiblingsCount = table.Column<int>(type: "integer", nullable: true),
                    DistanceToSchool = table.Column<decimal>(type: "numeric", nullable: true),
                    FatherName = table.Column<string>(type: "text", nullable: false),
                    FatherBirthYear = table.Column<int>(type: "integer", nullable: true),
                    FatherEducation = table.Column<string>(type: "text", nullable: false),
                    FatherOccupation = table.Column<string>(type: "text", nullable: false),
                    FatherIncome = table.Column<string>(type: "text", nullable: false),
                    FatherNik = table.Column<string>(type: "text", nullable: false),
                    MotherName = table.Column<string>(type: "text", nullable: false),
                    MotherBirthYear = table.Column<int>(type: "integer", nullable: true),
                    MotherEducation = table.Column<string>(type: "text", nullable: false),
                    MotherOccupation = table.Column<string>(type: "text", nullable: false),
                    MotherIncome = table.Column<string>(type: "text", nullable: false),
                    MotherNik = table.Column<string>(type: "text", nullable: false),
                    GuardianName = table.Column<string>(type: "text", nullable: false),
                    GuardianBirthYear = table.Column<int>(type: "integer", nullable: true),
                    GuardianEducation = table.Column<string>(type: "text", nullable: false),
                    GuardianOccupation = table.Column<string>(type: "text", nullable: false),
                    GuardianIncome = table.Column<string>(type: "text", nullable: false),
                    GuardianNik = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_student_details_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nis = table.Column<string>(type: "text", nullable: false),
                    NamaLengkap = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Telp = table.Column<string>(type: "text", nullable: false),
                    JenisKelamin = table.Column<int>(type: "integer", nullable: false),
                    Agama = table.Column<string>(type: "text", nullable: false),
                    ClassRoomId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_students_class_rooms_ClassRoomId",
                        column: x => x.ClassRoomId,
                        principalTable: "class_rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_students_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "student_scores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssessmentId = table.Column<long>(type: "bigint", nullable: false),
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    Score = table.Column<decimal>(type: "numeric", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_student_scores_assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_scores_students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assessments_ClassRoomId",
                table: "assessments",
                column: "ClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_assessments_TeacherId",
                table: "assessments",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_attendances_UserId",
                table: "attendances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_class_rooms_DepartmentId",
                table: "class_rooms",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_class_rooms_HomeroomTeacherId",
                table: "class_rooms",
                column: "HomeroomTeacherId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_class_rooms_SchoolYearId",
                table: "class_rooms",
                column: "SchoolYearId");

            migrationBuilder.CreateIndex(
                name: "IX_student_details_UserId",
                table: "student_details",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_scores_AssessmentId",
                table: "student_scores",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_student_scores_StudentId",
                table: "student_scores",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_students_ClassRoomId",
                table: "students",
                column: "ClassRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_students_UserId",
                table: "students",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_ClassRoomId",
                table: "users",
                column: "ClassRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_assessments_class_rooms_ClassRoomId",
                table: "assessments",
                column: "ClassRoomId",
                principalTable: "class_rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_assessments_users_TeacherId",
                table: "assessments",
                column: "TeacherId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_attendances_users_UserId",
                table: "attendances",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_class_rooms_users_HomeroomTeacherId",
                table: "class_rooms",
                column: "HomeroomTeacherId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_class_rooms_ClassRoomId",
                table: "users");

            migrationBuilder.DropTable(
                name: "attendances");

            migrationBuilder.DropTable(
                name: "extracurriculars");

            migrationBuilder.DropTable(
                name: "fcm_tokens");

            migrationBuilder.DropTable(
                name: "schedule_items");

            migrationBuilder.DropTable(
                name: "student_details");

            migrationBuilder.DropTable(
                name: "student_scores");

            migrationBuilder.DropTable(
                name: "assessments");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "class_rooms");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "school_years");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
