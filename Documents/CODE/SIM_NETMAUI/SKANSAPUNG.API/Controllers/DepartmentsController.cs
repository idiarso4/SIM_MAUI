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
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            var departments = await _context.Departments
                .OrderBy(d => d.NamaJurusan)
                .ToListAsync();
            return Ok(departments);
        }

        // GET: api/departments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(long id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return NotFound(new { message = "Department not found" });
            return Ok(department);
        }

        // GET: api/departments/{id}/classes
        [HttpGet("{id}/classes")]
        public async Task<ActionResult<IEnumerable<ClassRoom>>> GetClassesByDepartment(long id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return NotFound(new { message = "Department not found" });

            var classes = await _context.ClassRooms
                .Where(c => c.DepartmentId == id)
                .OrderBy(c => c.NamaKelas)
                .ToListAsync();
            return Ok(classes);
        }

        // GET: api/departments/{id}/teachers
        [HttpGet("{id}/teachers")]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachersByDepartment(long id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return NotFound(new { message = "Department not found" });

            var teachers = await _context.Teachers
                .Where(t => t.DepartmentId == id)
                .OrderBy(t => t.NamaLengkap)
                .ToListAsync();
            return Ok(teachers);
        }

        // GET: api/departments/{id}/subjects
        [HttpGet("{id}/subjects")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsByDepartment(long id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return NotFound(new { message = "Department not found" });

            var subjects = await _context.Subjects
                .Where(s => s.DepartmentId == id)
                .OrderBy(s => s.NamaMataPelajaran)
                .ToListAsync();
            return Ok(subjects);
        }

        // GET: api/departments/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Department>>> SearchDepartments([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { message = "Search query is required" });

            var departments = await _context.Departments
                .Where(d => d.NamaJurusan.Contains(query) || d.KodeJurusan.Contains(query))
                .OrderBy(d => d.NamaJurusan)
                .ToListAsync();
            return Ok(departments);
        }

        // POST: api/departments
        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment([FromBody] Department department)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            department.CreatedAt = DateTime.UtcNow;
            department.UpdatedAt = DateTime.UtcNow;
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
        }

        // PUT: api/departments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(long id, [FromBody] Department department)
        {
            if (id != department.Id)
                return BadRequest(new { message = "ID mismatch" });

            var existing = await _context.Departments.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Department not found" });

            existing.NamaJurusan = department.NamaJurusan;
            existing.KodeJurusan = department.KodeJurusan;
            existing.Deskripsi = department.Deskripsi;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/departments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(long id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return NotFound(new { message = "Department not found" });

            // Check if there are any classes in this department
            var hasClasses = await _context.ClassRooms.AnyAsync(c => c.DepartmentId == id);
            if (hasClasses)
                return BadRequest(new { message = "Cannot delete department with existing classes" });

            // Check if there are any teachers in this department
            var hasTeachers = await _context.Teachers.AnyAsync(t => t.DepartmentId == id);
            if (hasTeachers)
                return BadRequest(new { message = "Cannot delete department with existing teachers" });

            // Check if there are any subjects in this department
            var hasSubjects = await _context.Subjects.AnyAsync(s => s.DepartmentId == id);
            if (hasSubjects)
                return BadRequest(new { message = "Cannot delete department with existing subjects" });

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/departments/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetDepartmentStatistics()
        {
            var departments = await _context.Departments.ToListAsync();
            var result = new List<object>();

            foreach (var dept in departments)
            {
                var classCount = await _context.ClassRooms.CountAsync(c => c.DepartmentId == dept.Id);
                var teacherCount = await _context.Teachers.CountAsync(t => t.DepartmentId == dept.Id);
                var subjectCount = await _context.Subjects.CountAsync(s => s.DepartmentId == dept.Id);
                var studentCount = await _context.Students
                    .Include(s => s.ClassRoom)
                    .CountAsync(s => s.ClassRoom.DepartmentId == dept.Id);

                result.Add(new
                {
                    Department = new { dept.Id, dept.NamaJurusan, dept.KodeJurusan },
                    ClassCount = classCount,
                    TeacherCount = teacherCount,
                    SubjectCount = subjectCount,
                    StudentCount = studentCount
                });
            }

            return Ok(result);
        }
    }
} 