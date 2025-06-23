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
    public class TeachersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TeachersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            var teachers = await _context.Teachers
                .Include(t => t.Department)
                .OrderBy(t => t.NamaLengkap)
                .ToListAsync();
            return Ok(teachers);
        }

        // GET: api/teachers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(long id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null)
                return NotFound(new { message = "Teacher not found" });
            return Ok(teacher);
        }

        // GET: api/teachers/department/{departmentId}
        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachersByDepartment(long departmentId)
        {
            var teachers = await _context.Teachers
                .Include(t => t.Department)
                .Where(t => t.DepartmentId == departmentId)
                .OrderBy(t => t.NamaLengkap)
                .ToListAsync();
            return Ok(teachers);
        }

        // GET: api/teachers/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Teacher>>> SearchTeachers([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { message = "Search query is required" });
            var teachers = await _context.Teachers
                .Include(t => t.Department)
                .Where(t => t.NamaLengkap.Contains(query) || t.Nip.Contains(query) || t.Email.Contains(query))
                .OrderBy(t => t.NamaLengkap)
                .ToListAsync();
            return Ok(teachers);
        }

        // POST: api/teachers
        [HttpPost]
        public async Task<ActionResult<Teacher>> CreateTeacher([FromBody] Teacher teacher)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            teacher.CreatedAt = DateTime.UtcNow;
            teacher.UpdatedAt = DateTime.UtcNow;
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, teacher);
        }

        // PUT: api/teachers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(long id, [FromBody] Teacher teacher)
        {
            if (id != teacher.Id)
                return BadRequest(new { message = "ID mismatch" });
            var existing = await _context.Teachers.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Teacher not found" });
            existing.NamaLengkap = teacher.NamaLengkap;
            existing.Nip = teacher.Nip;
            existing.Email = teacher.Email;
            existing.NomorTelepon = teacher.NomorTelepon;
            existing.DepartmentId = teacher.DepartmentId;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/teachers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(long id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound(new { message = "Teacher not found" });
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 