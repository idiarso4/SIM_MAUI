using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKANSAPUNG.API.Data;
using SKANSAPUNG.API.Models;
using System.Collections.Generic;
using System.Linq;

namespace SKANSAPUNG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/reports/summary
        [HttpGet("summary")]
        public async Task<ActionResult<ReportSummaryDto>> GetSummary()
        {
            try
            {
                var totalStudents = await _context.Students.CountAsync();
                var totalTeachers = await _context.Teachers.CountAsync();
                var totalClasses = await _context.ClassRooms.CountAsync();
                
                // Calculate today's attendance
                var today = DateTime.Today;
                var attendanceToday = await _context.Attendances
                    .Where(a => a.Tanggal.Date == today && a.Status == "hadir")
                    .CountAsync();

                var summary = new ReportSummaryDto
                {
                    TotalStudents = totalStudents,
                    TotalTeachers = totalTeachers,
                    TotalClasses = totalClasses,
                    AttendanceToday = attendanceToday
                };

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving summary", error = ex.Message });
            }
        }

        // GET: api/reports/attendance/{classroomId}
        [HttpGet("attendance/{classroomId}")]
        public async Task<ActionResult<object>> GetAttendanceReport(long classroomId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var start = startDate ?? DateTime.Today.AddDays(-30);
                var end = endDate ?? DateTime.Today;

                var attendanceData = await _context.Attendances
                    .Include(a => a.Student)
                    .Where(a => a.Student.ClassRoomId == classroomId && 
                               a.Tanggal >= start && a.Tanggal <= end)
                    .GroupBy(a => new { a.Tanggal, a.Status })
                    .Select(g => new
                    {
                        Date = g.Key.Tanggal,
                        Status = g.Key.Status,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Date)
                    .ToListAsync();

                var totalStudents = await _context.Students
                    .Where(s => s.ClassRoomId == classroomId)
                    .CountAsync();

                var summary = new
                {
                    ClassroomId = classroomId,
                    StartDate = start,
                    EndDate = end,
                    TotalStudents = totalStudents,
                    AttendanceData = attendanceData,
                    TotalDays = (end - start).Days + 1
                };

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving attendance report", error = ex.Message });
            }
        }

        // GET: api/reports/grades/{classroomId}
        [HttpGet("grades/{classroomId}")]
        public async Task<ActionResult<object>> GetGradesReport(long classroomId, [FromQuery] long? subjectId = null)
        {
            try
            {
                var query = _context.StudentScores
                    .Include(ss => ss.Student)
                    .Include(ss => ss.Assessment)
                    .ThenInclude(a => a.Subject)
                    .Where(ss => ss.Student.ClassRoomId == classroomId && ss.Score.HasValue);

                if (subjectId.HasValue)
                {
                    query = query.Where(ss => ss.Assessment.SubjectId == subjectId.Value);
                }

                var grades = await query.ToListAsync();

                if (!grades.Any())
                {
                    return Ok(new { 
                        ClassroomId = classroomId,
                        SubjectId = subjectId,
                        AverageScore = 0,
                        TotalAssessments = 0,
                        StudentBreakdown = new List<object>()
                    });
                }

                var averageScore = grades.Average(g => g.Score.Value);
                var studentBreakdown = grades
                    .GroupBy(g => new { g.StudentId, g.Student.NamaLengkap })
                    .Select(g => new
                    {
                        StudentId = g.Key.StudentId,
                        StudentName = g.Key.NamaLengkap,
                        AverageScore = Math.Round(g.Average(ss => ss.Score.Value), 2),
                        AssessmentCount = g.Count(),
                        HighestScore = g.Max(ss => ss.Score.Value),
                        LowestScore = g.Min(ss => ss.Score.Value)
                    })
                    .OrderByDescending(x => x.AverageScore)
                    .ToList();

                var subjectBreakdown = grades
                    .GroupBy(g => g.Assessment.Subject.NamaMataPelajaran)
                    .Select(g => new
                    {
                        Subject = g.Key,
                        AverageScore = Math.Round(g.Average(ss => ss.Score.Value), 2),
                        AssessmentCount = g.Count()
                    })
                    .ToList();

                return Ok(new
                {
                    ClassroomId = classroomId,
                    SubjectId = subjectId,
                    AverageScore = Math.Round(averageScore, 2),
                    TotalAssessments = grades.Count,
                    StudentBreakdown = studentBreakdown,
                    SubjectBreakdown = subjectBreakdown
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving grades report", error = ex.Message });
            }
        }

        // GET: api/reports/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<object>> GetStudentReport(long studentId)
        {
            try
            {
                var student = await _context.Students
                    .Include(s => s.ClassRoom)
                    .ThenInclude(c => c.Department)
                    .FirstOrDefaultAsync(s => s.Id == studentId);

                if (student == null)
                {
                    return NotFound(new { message = "Student not found" });
                }

                // Get attendance data
                var attendanceData = await _context.Attendances
                    .Where(a => a.StudentId == studentId)
                    .GroupBy(a => a.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToListAsync();

                // Get grades data
                var gradesData = await _context.StudentScores
                    .Include(ss => ss.Assessment)
                    .ThenInclude(a => a.Subject)
                    .Where(ss => ss.StudentId == studentId && ss.Score.HasValue)
                    .ToListAsync();

                var averageScore = gradesData.Any() ? gradesData.Average(g => g.Score.Value) : 0;
                var subjectBreakdown = gradesData
                    .GroupBy(g => g.Assessment.Subject.NamaMataPelajaran)
                    .Select(g => new
                    {
                        Subject = g.Key,
                        AverageScore = Math.Round(g.Average(ss => ss.Score.Value), 2),
                        AssessmentCount = g.Count()
                    })
                    .ToList();

                return Ok(new
                {
                    Student = new
                    {
                        student.Id,
                        student.NamaLengkap,
                        student.Nis,
                        student.Nisn,
                        ClassRoom = student.ClassRoom?.NamaKelas,
                        Department = student.ClassRoom?.Department?.NamaJurusan
                    },
                    AttendanceSummary = attendanceData,
                    GradesSummary = new
                    {
                        AverageScore = Math.Round(averageScore, 2),
                        TotalAssessments = gradesData.Count,
                        SubjectBreakdown = subjectBreakdown
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving student report", error = ex.Message });
            }
        }

        // GET: api/reports/classroom/{classroomId}
        [HttpGet("classroom/{classroomId}")]
        public async Task<ActionResult<object>> GetClassroomReport(long classroomId)
        {
            try
            {
                var classroom = await _context.ClassRooms
                    .Include(c => c.Department)
                    .FirstOrDefaultAsync(c => c.Id == classroomId);

                if (classroom == null)
                {
                    return NotFound(new { message = "Classroom not found" });
                }

                var students = await _context.Students
                    .Where(s => s.ClassRoomId == classroomId)
                    .CountAsync();

                var attendanceData = await _context.Attendances
                    .Include(a => a.Student)
                    .Where(a => a.Student.ClassRoomId == classroomId)
                    .GroupBy(a => a.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToListAsync();

                var gradesData = await _context.StudentScores
                    .Include(ss => ss.Student)
                    .Include(ss => ss.Assessment)
                    .ThenInclude(a => a.Subject)
                    .Where(ss => ss.Student.ClassRoomId == classroomId && ss.Score.HasValue)
                    .ToListAsync();

                var averageScore = gradesData.Any() ? gradesData.Average(g => g.Score.Value) : 0;

                return Ok(new
                {
                    Classroom = new
                    {
                        classroom.Id,
                        classroom.NamaKelas,
                        Department = classroom.Department?.NamaJurusan
                    },
                    StudentCount = students,
                    AttendanceSummary = attendanceData,
                    GradesSummary = new
                    {
                        AverageScore = Math.Round(averageScore, 2),
                        TotalAssessments = gradesData.Count
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving classroom report", error = ex.Message });
            }
        }

        // GET: api/reports/department/{departmentId}
        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<object>> GetDepartmentReport(long departmentId)
        {
            try
            {
                var department = await _context.Departments
                    .FirstOrDefaultAsync(d => d.Id == departmentId);

                if (department == null)
                {
                    return NotFound(new { message = "Department not found" });
                }

                var classrooms = await _context.ClassRooms
                    .Where(c => c.DepartmentId == departmentId)
                    .CountAsync();

                var students = await _context.Students
                    .Include(s => s.ClassRoom)
                    .Where(s => s.ClassRoom.DepartmentId == departmentId)
                    .CountAsync();

                var teachers = await _context.Teachers
                    .Where(t => t.DepartmentId == departmentId)
                    .CountAsync();

                var attendanceData = await _context.Attendances
                    .Include(a => a.Student)
                    .ThenInclude(s => s.ClassRoom)
                    .Where(a => a.Student.ClassRoom.DepartmentId == departmentId)
                    .GroupBy(a => a.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToListAsync();

                return Ok(new
                {
                    Department = new
                    {
                        department.Id,
                        department.NamaJurusan
                    },
                    Classrooms = classrooms,
                    Students = students,
                    Teachers = teachers,
                    AttendanceSummary = attendanceData
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving department report", error = ex.Message });
            }
        }

        // GET: api/reports/export/attendance/{classroomId}
        [HttpGet("export/attendance/{classroomId}")]
        public async Task<ActionResult<object>> ExportAttendanceReport(long classroomId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var start = startDate ?? DateTime.Today.AddDays(-30);
                var end = endDate ?? DateTime.Today;

                var attendanceData = await _context.Attendances
                    .Include(a => a.Student)
                    .Where(a => a.Student.ClassRoomId == classroomId && 
                               a.Tanggal >= start && a.Tanggal <= end)
                    .OrderBy(a => a.Tanggal)
                    .ThenBy(a => a.Student.NamaLengkap)
                    .Select(a => new
                    {
                        Date = a.Tanggal.ToString("yyyy-MM-dd"),
                        StudentName = a.Student.NamaLengkap,
                        StudentNis = a.Student.Nis,
                        Status = a.Status,
                        CheckInTime = a.CheckInTime?.ToString("HH:mm"),
                        CheckOutTime = a.CheckOutTime?.ToString("HH:mm")
                    })
                    .ToListAsync();

                return Ok(new
                {
                    ClassroomId = classroomId,
                    StartDate = start.ToString("yyyy-MM-dd"),
                    EndDate = end.ToString("yyyy-MM-dd"),
                    ExportDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Data = attendanceData
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error exporting attendance report", error = ex.Message });
            }
        }

        // GET: api/reports/export/grades/{classroomId}
        [HttpGet("export/grades/{classroomId}")]
        public async Task<ActionResult<object>> ExportGradesReport(long classroomId, [FromQuery] long? subjectId = null)
        {
            try
            {
                var query = _context.StudentScores
                    .Include(ss => ss.Student)
                    .Include(ss => ss.Assessment)
                    .ThenInclude(a => a.Subject)
                    .Where(ss => ss.Student.ClassRoomId == classroomId);

                if (subjectId.HasValue)
                {
                    query = query.Where(ss => ss.Assessment.SubjectId == subjectId.Value);
                }

                var gradesData = await query
                    .OrderBy(ss => ss.Student.NamaLengkap)
                    .ThenBy(ss => ss.Assessment.TanggalPenilaian)
                    .Select(ss => new
                    {
                        StudentName = ss.Student.NamaLengkap,
                        StudentNis = ss.Student.Nis,
                        Subject = ss.Assessment.Subject.NamaMataPelajaran,
                        Assessment = ss.Assessment.NamaPenilaian,
                        AssessmentDate = ss.Assessment.TanggalPenilaian.ToString("yyyy-MM-dd"),
                        Score = ss.Score,
                        Status = ss.Status,
                        Comments = ss.Comments
                    })
                    .ToListAsync();

                return Ok(new
                {
                    ClassroomId = classroomId,
                    SubjectId = subjectId,
                    ExportDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Data = gradesData
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error exporting grades report", error = ex.Message });
            }
        }

        // GET: api/reports/statistics/overview
        [HttpGet("statistics/overview")]
        public async Task<ActionResult<object>> GetOverviewStatistics()
        {
            try
            {
                var totalStudents = await _context.Students.CountAsync();
                var totalTeachers = await _context.Teachers.CountAsync();
                var totalClasses = await _context.ClassRooms.CountAsync();
                var totalSubjects = await _context.Subjects.CountAsync();

                var today = DateTime.Today;
                var attendanceToday = await _context.Attendances
                    .Where(a => a.Tanggal.Date == today)
                    .GroupBy(a => a.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToListAsync();

                var recentGrades = await _context.StudentScores
                    .Where(ss => ss.Score.HasValue && ss.CreatedAt >= DateTime.Today.AddDays(-7))
                    .CountAsync();

                var averageScore = await _context.StudentScores
                    .Where(ss => ss.Score.HasValue)
                    .AverageAsync(ss => ss.Score.Value);

                return Ok(new
                {
                    TotalStudents = totalStudents,
                    TotalTeachers = totalTeachers,
                    TotalClasses = totalClasses,
                    TotalSubjects = totalSubjects,
                    AttendanceToday = attendanceToday,
                    RecentGrades = recentGrades,
                    AverageScore = Math.Round(averageScore, 2)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving overview statistics", error = ex.Message });
            }
        }
    }
} 