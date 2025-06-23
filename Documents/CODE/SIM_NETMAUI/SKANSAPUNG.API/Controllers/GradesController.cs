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
    public class GradesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GradesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/grades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentScore>>> GetGrades()
        {
            try
            {
                var grades = await _context.StudentScores
                    .Include(ss => ss.Student)
                    .Include(ss => ss.Assessment)
                    .ThenInclude(a => a.Subject)
                    .OrderByDescending(ss => ss.CreatedAt)
                    .ToListAsync();

                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving grades", error = ex.Message });
            }
        }

        // GET: api/grades/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentScore>> GetGrade(long id)
        {
            try
            {
                var grade = await _context.StudentScores
                    .Include(ss => ss.Student)
                    .Include(ss => ss.Assessment)
                    .ThenInclude(a => a.Subject)
                    .FirstOrDefaultAsync(ss => ss.Id == id);

                if (grade == null)
                {
                    return NotFound(new { message = "Grade not found" });
                }

                return Ok(grade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving grade", error = ex.Message });
            }
        }

        // POST: api/grades
        [HttpPost]
        public async Task<ActionResult<StudentScore>> CreateGrade([FromBody] StudentScore grade)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validate if student exists
                var student = await _context.Students.FindAsync(grade.StudentId);
                if (student == null)
                {
                    return BadRequest(new { message = "Student not found" });
                }

                // Validate if assessment exists
                var assessment = await _context.Assessments.FindAsync(grade.AssessmentId);
                if (assessment == null)
                {
                    return BadRequest(new { message = "Assessment not found" });
                }

                // Check if grade already exists for this student and assessment
                var existingGrade = await _context.StudentScores
                    .FirstOrDefaultAsync(ss => ss.StudentId == grade.StudentId && ss.AssessmentId == grade.AssessmentId);

                if (existingGrade != null)
                {
                    return BadRequest(new { message = "Grade already exists for this student and assessment" });
                }

                grade.CreatedAt = DateTime.UtcNow;
                grade.UpdatedAt = DateTime.UtcNow;

                _context.StudentScores.Add(grade);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetGrade), new { id = grade.Id }, grade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating grade", error = ex.Message });
            }
        }

        // PUT: api/grades/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrade(long id, [FromBody] StudentScore grade)
        {
            try
            {
                if (id != grade.Id)
                {
                    return BadRequest(new { message = "ID mismatch" });
                }

                var existingGrade = await _context.StudentScores.FindAsync(id);
                if (existingGrade == null)
                {
                    return NotFound(new { message = "Grade not found" });
                }

                existingGrade.Score = grade.Score;
                existingGrade.Comments = grade.Comments;
                existingGrade.Status = grade.Status;
                existingGrade.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating grade", error = ex.Message });
            }
        }

        // DELETE: api/grades/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(long id)
        {
            try
            {
                var grade = await _context.StudentScores.FindAsync(id);
                if (grade == null)
                {
                    return NotFound(new { message = "Grade not found" });
                }

                _context.StudentScores.Remove(grade);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting grade", error = ex.Message });
            }
        }

        // GET: api/grades/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<StudentScore>>> GetGradesByStudent(long studentId)
        {
            try
            {
                var grades = await _context.StudentScores
                    .Include(ss => ss.Assessment)
                    .ThenInclude(a => a.Subject)
                    .Where(ss => ss.StudentId == studentId)
                    .OrderByDescending(ss => ss.CreatedAt)
                    .ToListAsync();

                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving student grades", error = ex.Message });
            }
        }

        // GET: api/grades/assessment/{assessmentId}
        [HttpGet("assessment/{assessmentId}")]
        public async Task<ActionResult<IEnumerable<StudentScoreDetailDto>>> GetGradesForAssessment(long assessmentId)
        {
            try
            {
                var grades = await _context.StudentScores
                    .Include(ss => ss.Student)
                    .Where(ss => ss.AssessmentId == assessmentId)
                    .Select(ss => new StudentScoreDetailDto
                    {
                        StudentId = ss.StudentId,
                        StudentName = ss.Student.NamaLengkap,
                        Score = ss.Score,
                        Status = ss.Status,
                        Comments = ss.Comments
                    })
                    .OrderBy(g => g.StudentName)
                    .ToListAsync();

                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving assessment grades", error = ex.Message });
            }
        }

        // GET: api/grades/subject/{subjectId}
        [HttpGet("subject/{subjectId}")]
        public async Task<ActionResult<IEnumerable<StudentScore>>> GetGradesBySubject(long subjectId)
        {
            try
            {
                var grades = await _context.StudentScores
                    .Include(ss => ss.Student)
                    .Include(ss => ss.Assessment)
                    .Where(ss => ss.Assessment.SubjectId == subjectId)
                    .OrderByDescending(ss => ss.CreatedAt)
                    .ToListAsync();

                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving subject grades", error = ex.Message });
            }
        }

        // GET: api/grades/statistics/student/{studentId}
        [HttpGet("statistics/student/{studentId}")]
        public async Task<ActionResult<object>> GetStudentGradeStatistics(long studentId)
        {
            try
            {
                var grades = await _context.StudentScores
                    .Include(ss => ss.Assessment)
                    .ThenInclude(a => a.Subject)
                    .Where(ss => ss.StudentId == studentId && ss.Score.HasValue)
                    .ToListAsync();

                if (!grades.Any())
                {
                    return Ok(new { 
                        AverageScore = 0, 
                        TotalAssessments = 0, 
                        SubjectBreakdown = new List<object>() 
                    });
                }

                var averageScore = grades.Average(g => g.Score.Value);
                var subjectBreakdown = grades
                    .GroupBy(g => g.Assessment.Subject.NamaMataPelajaran)
                    .Select(g => new
                    {
                        Subject = g.Key,
                        AverageScore = g.Average(ss => ss.Score.Value),
                        AssessmentCount = g.Count()
                    })
                    .ToList();

                return Ok(new
                {
                    AverageScore = Math.Round(averageScore, 2),
                    TotalAssessments = grades.Count,
                    SubjectBreakdown = subjectBreakdown
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving student statistics", error = ex.Message });
            }
        }

        // GET: api/grades/statistics/classroom/{classroomId}
        [HttpGet("statistics/classroom/{classroomId}")]
        public async Task<ActionResult<object>> GetClassroomGradeStatistics(long classroomId)
        {
            try
            {
                var grades = await _context.StudentScores
                    .Include(ss => ss.Student)
                    .Include(ss => ss.Assessment)
                    .ThenInclude(a => a.Subject)
                    .Where(ss => ss.Student.ClassRoomId == classroomId && ss.Score.HasValue)
                    .ToListAsync();

                if (!grades.Any())
                {
                    return Ok(new { 
                        AverageScore = 0, 
                        TotalStudents = 0, 
                        SubjectBreakdown = new List<object>() 
                    });
                }

                var averageScore = grades.Average(g => g.Score.Value);
                var totalStudents = grades.Select(g => g.StudentId).Distinct().Count();
                var subjectBreakdown = grades
                    .GroupBy(g => g.Assessment.Subject.NamaMataPelajaran)
                    .Select(g => new
                    {
                        Subject = g.Key,
                        AverageScore = Math.Round(g.Average(ss => ss.Score.Value), 2),
                        StudentCount = g.Select(ss => ss.StudentId).Distinct().Count()
                    })
                    .ToList();

                return Ok(new
                {
                    AverageScore = Math.Round(averageScore, 2),
                    TotalStudents = totalStudents,
                    SubjectBreakdown = subjectBreakdown
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving classroom statistics", error = ex.Message });
            }
        }

        // POST: api/grades/bulk
        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulkGrades([FromBody] List<StudentScore> grades)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdGrades = new List<StudentScore>();

                foreach (var grade in grades)
                {
                    // Validate if student exists
                    var student = await _context.Students.FindAsync(grade.StudentId);
                    if (student == null)
                    {
                        return BadRequest(new { message = $"Student with ID {grade.StudentId} not found" });
                    }

                    // Validate if assessment exists
                    var assessment = await _context.Assessments.FindAsync(grade.AssessmentId);
                    if (assessment == null)
                    {
                        return BadRequest(new { message = $"Assessment with ID {grade.AssessmentId} not found" });
                    }

                    // Check if grade already exists
                    var existingGrade = await _context.StudentScores
                        .FirstOrDefaultAsync(ss => ss.StudentId == grade.StudentId && ss.AssessmentId == grade.AssessmentId);

                    if (existingGrade != null)
                    {
                        return BadRequest(new { message = $"Grade already exists for student {grade.StudentId} and assessment {grade.AssessmentId}" });
                    }

                    grade.CreatedAt = DateTime.UtcNow;
                    grade.UpdatedAt = DateTime.UtcNow;

                    _context.StudentScores.Add(grade);
                    createdGrades.Add(grade);
                }

                await _context.SaveChangesAsync();

                return Ok(new { 
                    Message = $"Successfully created {createdGrades.Count} grades",
                    CreatedCount = createdGrades.Count 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating bulk grades", error = ex.Message });
            }
        }

        // POST: api/grades/sync
        [HttpPost("sync")]
        public async Task<IActionResult> SyncGrades([FromBody] List<StudentScoreDetailDto> grades)
        {
            try
            {
                var syncedIds = new List<object>();

                foreach (var gradeDto in grades)
                {
                    if (gradeDto.Id > 0)
                    {
                        // Update existing grade
                        var existingGrade = await _context.StudentScores.FindAsync(gradeDto.Id);
                        if (existingGrade != null)
                        {
                            existingGrade.Score = gradeDto.Score;
                            existingGrade.Status = gradeDto.Status;
                            existingGrade.Comments = gradeDto.Comments;
                            existingGrade.UpdatedAt = DateTime.UtcNow;

                            syncedIds.Add(new { LocalId = gradeDto.LocalId, ServerId = existingGrade.Id });
                        }
                    }
                    else
                    {
                        // Create new grade
                        var newGrade = new StudentScore
                        {
                            StudentId = gradeDto.StudentId,
                            AssessmentId = gradeDto.AssessmentId,
                            Score = gradeDto.Score,
                            Status = gradeDto.Status,
                            Comments = gradeDto.Comments,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        _context.StudentScores.Add(newGrade);
                        await _context.SaveChangesAsync();

                        syncedIds.Add(new { LocalId = gradeDto.LocalId, ServerId = newGrade.Id });
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { SyncedIds = syncedIds, Message = $"Successfully synced {grades.Count} grades" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error syncing grades", error = ex.Message });
            }
        }

        // GET: api/grades/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<StudentScore>>> SearchGrades([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest(new { message = "Search query is required" });
                }

                var grades = await _context.StudentScores
                    .Include(ss => ss.Student)
                    .Include(ss => ss.Assessment)
                    .ThenInclude(a => a.Subject)
                    .Where(ss => ss.Student.NamaLengkap.Contains(query) || 
                               ss.Assessment.NamaPenilaian.Contains(query) ||
                               ss.Assessment.Subject.NamaMataPelajaran.Contains(query))
                    .OrderByDescending(ss => ss.CreatedAt)
                    .ToListAsync();

                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error searching grades", error = ex.Message });
            }
        }
    }
} 