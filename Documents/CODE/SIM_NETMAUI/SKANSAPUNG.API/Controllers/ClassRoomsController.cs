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
    public class ClassRoomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClassRoomsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/classrooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassRoom>>> GetClassRooms()
        {
            var classRooms = await _context.ClassRooms
                .Include(c => c.Department)
                .OrderBy(c => c.NamaKelas)
                .ToListAsync();
            return Ok(classRooms);
        }

        // GET: api/classrooms/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassRoom>> GetClassRoom(long id)
        {
            var classRoom = await _context.ClassRooms
                .Include(c => c.Department)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (classRoom == null)
                return NotFound(new { message = "Class room not found" });
            return Ok(classRoom);
        }

        // GET: api/classrooms/department/{departmentId}
        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<ClassRoom>>> GetClassRoomsByDepartment(long departmentId)
        {
            var classRooms = await _context.ClassRooms
                .Include(c => c.Department)
                .Where(c => c.DepartmentId == departmentId)
                .OrderBy(c => c.NamaKelas)
                .ToListAsync();
            return Ok(classRooms);
        }

        // GET: api/classrooms/{id}/students
        [HttpGet("{id}/students")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByClassRoom(long id)
        {
            var classRoom = await _context.ClassRooms.FindAsync(id);
            if (classRoom == null)
                return NotFound(new { message = "Class room not found" });

            var students = await _context.Students
                .Where(s => s.ClassRoomId == id)
                .OrderBy(s => s.NamaLengkap)
                .ToListAsync();
            return Ok(students);
        }

        // GET: api/classrooms/{id}/schedule
        [HttpGet("{id}/schedule")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetScheduleByClassRoom(long id)
        {
            var classRoom = await _context.ClassRooms.FindAsync(id);
            if (classRoom == null)
                return NotFound(new { message = "Class room not found" });

            var schedule = await _context.Schedules
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .Where(s => s.ClassRoomId == id)
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
            return Ok(schedule);
        }

        // GET: api/classrooms/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ClassRoom>>> SearchClassRooms([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { message = "Search query is required" });

            var classRooms = await _context.ClassRooms
                .Include(c => c.Department)
                .Where(c => c.NamaKelas.Contains(query))
                .OrderBy(c => c.NamaKelas)
                .ToListAsync();
            return Ok(classRooms);
        }

        // POST: api/classrooms
        [HttpPost]
        public async Task<ActionResult<ClassRoom>> CreateClassRoom([FromBody] ClassRoom classRoom)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if department exists
            var department = await _context.Departments.FindAsync(classRoom.DepartmentId);
            if (department == null)
                return BadRequest(new { message = "Department not found" });

            classRoom.CreatedAt = DateTime.UtcNow;
            classRoom.UpdatedAt = DateTime.UtcNow;
            _context.ClassRooms.Add(classRoom);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClassRoom), new { id = classRoom.Id }, classRoom);
        }

        // PUT: api/classrooms/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClassRoom(long id, [FromBody] ClassRoom classRoom)
        {
            if (id != classRoom.Id)
                return BadRequest(new { message = "ID mismatch" });

            var existing = await _context.ClassRooms.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Class room not found" });

            // Check if department exists
            var department = await _context.Departments.FindAsync(classRoom.DepartmentId);
            if (department == null)
                return BadRequest(new { message = "Department not found" });

            existing.NamaKelas = classRoom.NamaKelas;
            existing.DepartmentId = classRoom.DepartmentId;
            existing.Kapasitas = classRoom.Kapasitas;
            existing.Lokasi = classRoom.Lokasi;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/classrooms/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassRoom(long id)
        {
            var classRoom = await _context.ClassRooms.FindAsync(id);
            if (classRoom == null)
                return NotFound(new { message = "Class room not found" });

            // Check if there are any students in this class
            var hasStudents = await _context.Students.AnyAsync(s => s.ClassRoomId == id);
            if (hasStudents)
                return BadRequest(new { message = "Cannot delete class room with existing students" });

            // Check if there are any schedules for this class
            var hasSchedules = await _context.Schedules.AnyAsync(s => s.ClassRoomId == id);
            if (hasSchedules)
                return BadRequest(new { message = "Cannot delete class room with existing schedules" });

            _context.ClassRooms.Remove(classRoom);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/classrooms/{id}/statistics
        [HttpGet("{id}/statistics")]
        public async Task<ActionResult<object>> GetClassRoomStatistics(long id)
        {
            var classRoom = await _context.ClassRooms
                .Include(c => c.Department)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (classRoom == null)
                return NotFound(new { message = "Class room not found" });

            var studentCount = await _context.Students.CountAsync(s => s.ClassRoomId == id);
            var scheduleCount = await _context.Schedules.CountAsync(s => s.ClassRoomId == id);
            
            // Get attendance statistics
            var attendanceStats = await _context.Attendances
                .Include(a => a.Student)
                .Where(a => a.Student.ClassRoomId == id)
                .GroupBy(a => a.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            // Get grade statistics if available
            var averageScore = 0.0;
            var hasGrades = await _context.StudentScores
                .Include(ss => ss.Student)
                .AnyAsync(ss => ss.Student.ClassRoomId == id && ss.Score.HasValue);
            
            if (hasGrades)
            {
                averageScore = await _context.StudentScores
                    .Include(ss => ss.Student)
                    .Where(ss => ss.Student.ClassRoomId == id && ss.Score.HasValue)
                    .AverageAsync(ss => ss.Score.Value);
            }

            return Ok(new
            {
                ClassRoom = new { classRoom.Id, classRoom.NamaKelas, Department = classRoom.Department?.NamaJurusan },
                StudentCount = studentCount,
                ScheduleCount = scheduleCount,
                AttendanceStatistics = attendanceStats,
                AverageScore = Math.Round(averageScore, 2)
            });
        }
    }
} 