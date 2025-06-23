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
    public class ExtracurricularController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExtracurricularController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/extracurricular
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Extracurricular>>> GetExtracurriculars()
        {
            var extracurriculars = await _context.Extracurriculars
                .OrderBy(e => e.NamaEkskul)
                .ToListAsync();
            return Ok(extracurriculars);
        }

        // GET: api/extracurricular/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Extracurricular>> GetExtracurricular(long id)
        {
            var extracurricular = await _context.Extracurriculars.FindAsync(id);
            if (extracurricular == null)
                return NotFound(new { message = "Extracurricular not found" });
            return Ok(extracurricular);
        }

        // GET: api/extracurricular/{id}/students
        [HttpGet("{id}/students")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByExtracurricular(long id)
        {
            var extracurricular = await _context.Extracurriculars.FindAsync(id);
            if (extracurricular == null)
                return NotFound(new { message = "Extracurricular not found" });

            var students = await _context.StudentExtracurriculars
                .Include(se => se.Student)
                .ThenInclude(s => s.ClassRoom)
                .Where(se => se.ExtracurricularId == id)
                .Select(se => se.Student)
                .OrderBy(s => s.NamaLengkap)
                .ToListAsync();
            return Ok(students);
        }

        // GET: api/extracurricular/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<Extracurricular>>> GetExtracurricularsByStudent(long studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
                return NotFound(new { message = "Student not found" });

            var extracurriculars = await _context.StudentExtracurriculars
                .Include(se => se.Extracurricular)
                .Where(se => se.StudentId == studentId)
                .Select(se => se.Extracurricular)
                .OrderBy(e => e.NamaEkskul)
                .ToListAsync();
            return Ok(extracurriculars);
        }

        // GET: api/extracurricular/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Extracurricular>>> SearchExtracurriculars([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { message = "Search query is required" });

            var extracurriculars = await _context.Extracurriculars
                .Where(e => e.NamaEkskul.Contains(query) || e.Deskripsi.Contains(query))
                .OrderBy(e => e.NamaEkskul)
                .ToListAsync();
            return Ok(extracurriculars);
        }

        // POST: api/extracurricular
        [HttpPost]
        public async Task<ActionResult<Extracurricular>> CreateExtracurricular([FromBody] Extracurricular extracurricular)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            extracurricular.CreatedAt = DateTime.UtcNow;
            extracurricular.UpdatedAt = DateTime.UtcNow;
            _context.Extracurriculars.Add(extracurricular);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetExtracurricular), new { id = extracurricular.Id }, extracurricular);
        }

        // PUT: api/extracurricular/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExtracurricular(long id, [FromBody] Extracurricular extracurricular)
        {
            if (id != extracurricular.Id)
                return BadRequest(new { message = "ID mismatch" });

            var existing = await _context.Extracurriculars.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Extracurricular not found" });

            existing.NamaEkskul = extracurricular.NamaEkskul;
            existing.Deskripsi = extracurricular.Deskripsi;
            existing.JadwalHari = extracurricular.JadwalHari;
            existing.JadwalWaktu = extracurricular.JadwalWaktu;
            existing.Pembina = extracurricular.Pembina;
            existing.Lokasi = extracurricular.Lokasi;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/extracurricular/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExtracurricular(long id)
        {
            var extracurricular = await _context.Extracurriculars.FindAsync(id);
            if (extracurricular == null)
                return NotFound(new { message = "Extracurricular not found" });

            // Check if there are any students enrolled in this extracurricular
            var hasStudents = await _context.StudentExtracurriculars.AnyAsync(se => se.ExtracurricularId == id);
            if (hasStudents)
                return BadRequest(new { message = "Cannot delete extracurricular with enrolled students" });

            _context.Extracurriculars.Remove(extracurricular);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/extracurricular/enroll
        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudent([FromBody] StudentExtracurricular enrollment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if student exists
            var student = await _context.Students.FindAsync(enrollment.StudentId);
            if (student == null)
                return BadRequest(new { message = "Student not found" });

            // Check if extracurricular exists
            var extracurricular = await _context.Extracurriculars.FindAsync(enrollment.ExtracurricularId);
            if (extracurricular == null)
                return BadRequest(new { message = "Extracurricular not found" });

            // Check if enrollment already exists
            var existing = await _context.StudentExtracurriculars
                .FirstOrDefaultAsync(se => se.StudentId == enrollment.StudentId && se.ExtracurricularId == enrollment.ExtracurricularId);
            if (existing != null)
                return BadRequest(new { message = "Student is already enrolled in this extracurricular" });

            enrollment.CreatedAt = DateTime.UtcNow;
            _context.StudentExtracurriculars.Add(enrollment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Student enrolled successfully" });
        }

        // DELETE: api/extracurricular/unenroll
        [HttpDelete("unenroll")]
        public async Task<IActionResult> UnenrollStudent([FromQuery] long studentId, [FromQuery] long extracurricularId)
        {
            var enrollment = await _context.StudentExtracurriculars
                .FirstOrDefaultAsync(se => se.StudentId == studentId && se.ExtracurricularId == extracurricularId);
            if (enrollment == null)
                return NotFound(new { message = "Student is not enrolled in this extracurricular" });

            _context.StudentExtracurriculars.Remove(enrollment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Student unenrolled successfully" });
        }

        // GET: api/extracurricular/{id}/statistics
        [HttpGet("{id}/statistics")]
        public async Task<ActionResult<object>> GetExtracurricularStatistics(long id)
        {
            var extracurricular = await _context.Extracurriculars.FindAsync(id);
            if (extracurricular == null)
                return NotFound(new { message = "Extracurricular not found" });

            var studentCount = await _context.StudentExtracurriculars.CountAsync(se => se.ExtracurricularId == id);
            
            // Get class distribution
            var classDistribution = await _context.StudentExtracurriculars
                .Include(se => se.Student)
                .ThenInclude(s => s.ClassRoom)
                .Where(se => se.ExtracurricularId == id)
                .GroupBy(se => se.Student.ClassRoom.NamaKelas)
                .Select(g => new { ClassName = g.Key, Count = g.Count() })
                .OrderBy(x => x.ClassName)
                .ToListAsync();

            return Ok(new
            {
                Extracurricular = new { extracurricular.Id, extracurricular.NamaEkskul },
                StudentCount = studentCount,
                ClassDistribution = classDistribution
            });
        }
    }
} 