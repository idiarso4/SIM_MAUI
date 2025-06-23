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
    public class SubjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubjectsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/subjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
        {
            var subjects = await _context.Subjects
                .OrderBy(s => s.NamaMataPelajaran)
                .ToListAsync();
            return Ok(subjects);
        }

        // GET: api/subjects/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetSubject(long id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
                return NotFound(new { message = "Subject not found" });
            return Ok(subject);
        }

        // GET: api/subjects/department/{departmentId}
        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsByDepartment(long departmentId)
        {
            var subjects = await _context.Subjects
                .Where(s => s.DepartmentId == departmentId)
                .OrderBy(s => s.NamaMataPelajaran)
                .ToListAsync();
            return Ok(subjects);
        }

        // GET: api/subjects/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Subject>>> SearchSubjects([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { message = "Search query is required" });

            var subjects = await _context.Subjects
                .Where(s => s.NamaMataPelajaran.Contains(query) || s.KodeMataPelajaran.Contains(query))
                .OrderBy(s => s.NamaMataPelajaran)
                .ToListAsync();
            return Ok(subjects);
        }

        // POST: api/subjects
        [HttpPost]
        public async Task<ActionResult<Subject>> CreateSubject([FromBody] Subject subject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            subject.CreatedAt = DateTime.UtcNow;
            subject.UpdatedAt = DateTime.UtcNow;
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSubject), new { id = subject.Id }, subject);
        }

        // PUT: api/subjects/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(long id, [FromBody] Subject subject)
        {
            if (id != subject.Id)
                return BadRequest(new { message = "ID mismatch" });

            var existing = await _context.Subjects.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Subject not found" });

            existing.NamaMataPelajaran = subject.NamaMataPelajaran;
            existing.KodeMataPelajaran = subject.KodeMataPelajaran;
            existing.Deskripsi = subject.Deskripsi;
            existing.DepartmentId = subject.DepartmentId;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/subjects/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(long id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
                return NotFound(new { message = "Subject not found" });

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/subjects/teachers/{subjectId}
        [HttpGet("teachers/{subjectId}")]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachersBySubject(long subjectId)
        {
            var teachers = await _context.TeacherSubjects
                .Include(ts => ts.Teacher)
                .Where(ts => ts.SubjectId == subjectId)
                .Select(ts => ts.Teacher)
                .OrderBy(t => t.NamaLengkap)
                .ToListAsync();
            return Ok(teachers);
        }

        // POST: api/subjects/assign-teacher
        [HttpPost("assign-teacher")]
        public async Task<IActionResult> AssignTeacherToSubject([FromBody] TeacherSubject teacherSubject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if subject exists
            var subject = await _context.Subjects.FindAsync(teacherSubject.SubjectId);
            if (subject == null)
                return BadRequest(new { message = "Subject not found" });

            // Check if teacher exists
            var teacher = await _context.Teachers.FindAsync(teacherSubject.TeacherId);
            if (teacher == null)
                return BadRequest(new { message = "Teacher not found" });

            // Check if assignment already exists
            var existing = await _context.TeacherSubjects
                .FirstOrDefaultAsync(ts => ts.TeacherId == teacherSubject.TeacherId && ts.SubjectId == teacherSubject.SubjectId);
            if (existing != null)
                return BadRequest(new { message = "Teacher is already assigned to this subject" });

            teacherSubject.CreatedAt = DateTime.UtcNow;
            _context.TeacherSubjects.Add(teacherSubject);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Teacher assigned to subject successfully" });
        }

        // DELETE: api/subjects/remove-teacher
        [HttpDelete("remove-teacher")]
        public async Task<IActionResult> RemoveTeacherFromSubject([FromQuery] long teacherId, [FromQuery] long subjectId)
        {
            var assignment = await _context.TeacherSubjects
                .FirstOrDefaultAsync(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId);
            if (assignment == null)
                return NotFound(new { message = "Teacher is not assigned to this subject" });

            _context.TeacherSubjects.Remove(assignment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Teacher removed from subject successfully" });
        }
    }
} 