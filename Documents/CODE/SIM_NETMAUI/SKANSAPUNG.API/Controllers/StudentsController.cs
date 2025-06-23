using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKANSAPUNG.API.Data;
using SKANSAPUNG.API.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace SKANSAPUNG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            try
            {
                var students = await _context.Students
                    .Include(s => s.StudentDetail)
                    .Include(s => s.ClassRoom)
                    .ThenInclude(c => c.Department)
                    .ToListAsync();

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving students", error = ex.Message });
            }
        }

        // GET: api/students/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(long id)
        {
            try
            {
                var student = await _context.Students
                    .Include(s => s.StudentDetail)
                    .Include(s => s.ClassRoom)
                    .ThenInclude(c => c.Department)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (student == null)
                {
                    return NotFound(new { message = "Student not found" });
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving student", error = ex.Message });
            }
        }

        // POST: api/students
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent([FromBody] Student student)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                student.CreatedAt = DateTime.UtcNow;
                student.UpdatedAt = DateTime.UtcNow;

                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating student", error = ex.Message });
            }
        }

        // PUT: api/students/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(long id, [FromBody] Student student)
        {
            try
            {
                if (id != student.Id)
                {
                    return BadRequest(new { message = "ID mismatch" });
                }

                var existingStudent = await _context.Students.FindAsync(id);
                if (existingStudent == null)
                {
                    return NotFound(new { message = "Student not found" });
                }

                existingStudent.NamaLengkap = student.NamaLengkap;
                existingStudent.NamaPanggilan = student.NamaPanggilan;
                existingStudent.Nis = student.Nis;
                existingStudent.Nisn = student.Nisn;
                existingStudent.TempatLahir = student.TempatLahir;
                existingStudent.TanggalLahir = student.TanggalLahir;
                existingStudent.JenisKelamin = student.JenisKelamin;
                existingStudent.Agama = student.Agama;
                existingStudent.Alamat = student.Alamat;
                existingStudent.NomorTelepon = student.NomorTelepon;
                existingStudent.Email = student.Email;
                existingStudent.Status = student.Status;
                existingStudent.ClassRoomId = student.ClassRoomId;
                existingStudent.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating student", error = ex.Message });
            }
        }

        // DELETE: api/students/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    return NotFound(new { message = "Student not found" });
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting student", error = ex.Message });
            }
        }

        // GET: api/students/classroom/{classroomId}
        [HttpGet("classroom/{classroomId}")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByClassroom(long classroomId)
        {
            try
            {
                var students = await _context.Students
                    .Include(s => s.StudentDetail)
                    .Where(s => s.ClassRoomId == classroomId)
                    .ToListAsync();

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving students by classroom", error = ex.Message });
            }
        }

        // GET: api/students/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Student>>> SearchStudents([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest(new { message = "Search query is required" });
                }

                var students = await _context.Students
                    .Include(s => s.StudentDetail)
                    .Include(s => s.ClassRoom)
                    .Where(s => s.NamaLengkap.Contains(query) || 
                               s.Nis.Contains(query) || 
                               s.Nisn.Contains(query))
                    .ToListAsync();

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error searching students", error = ex.Message });
            }
        }

        // POST: api/students/sync
        [HttpPost("sync")]
        public async Task<IActionResult> SyncStudents([FromBody] List<StudentDto> students)
        {
            try
            {
                var syncedIds = new List<object>();

                foreach (var studentDto in students)
                {
                    if (studentDto.Id > 0)
                    {
                        // Update existing student
                        var existingStudent = await _context.Students.FindAsync(studentDto.Id);
                        if (existingStudent != null)
                        {
                            existingStudent.NamaLengkap = studentDto.NamaLengkap;
                            existingStudent.NamaPanggilan = studentDto.NamaPanggilan;
                            existingStudent.Nis = studentDto.Nis;
                            existingStudent.Nisn = studentDto.Nisn;
                            existingStudent.TempatLahir = studentDto.TempatLahir;
                            existingStudent.TanggalLahir = studentDto.TanggalLahir;
                            existingStudent.JenisKelamin = studentDto.JenisKelamin;
                            existingStudent.Agama = studentDto.Agama;
                            existingStudent.Alamat = studentDto.Alamat;
                            existingStudent.NomorTelepon = studentDto.NomorTelepon;
                            existingStudent.Email = studentDto.Email;
                            existingStudent.Status = studentDto.Status;
                            existingStudent.ClassRoomId = studentDto.ClassRoomId;
                            existingStudent.UpdatedAt = DateTime.UtcNow;

                            syncedIds.Add(new { LocalId = studentDto.LocalId, ServerId = existingStudent.Id });
                        }
                    }
                    else
                    {
                        // Create new student
                        var newStudent = new Student
                        {
                            NamaLengkap = studentDto.NamaLengkap,
                            NamaPanggilan = studentDto.NamaPanggilan,
                            Nis = studentDto.Nis,
                            Nisn = studentDto.Nisn,
                            TempatLahir = studentDto.TempatLahir,
                            TanggalLahir = studentDto.TanggalLahir,
                            JenisKelamin = studentDto.JenisKelamin,
                            Agama = studentDto.Agama,
                            Alamat = studentDto.Alamat,
                            NomorTelepon = studentDto.NomorTelepon,
                            Email = studentDto.Email,
                            Status = studentDto.Status,
                            ClassRoomId = studentDto.ClassRoomId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        _context.Students.Add(newStudent);
                        await _context.SaveChangesAsync();

                        syncedIds.Add(new { LocalId = studentDto.LocalId, ServerId = newStudent.Id });
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { SyncedIds = syncedIds, Message = $"Successfully synced {students.Count} students" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error syncing students", error = ex.Message });
            }
        }
    }
} 