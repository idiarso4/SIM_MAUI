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
    public class ScheduleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/schedule
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
        {
            var schedules = await _context.Schedules
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .Include(s => s.ClassRoom)
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
            return Ok(schedules);
        }

        // GET: api/schedule/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Schedule>> GetSchedule(long id)
        {
            var schedule = await _context.Schedules
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .Include(s => s.ClassRoom)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (schedule == null)
                return NotFound(new { message = "Schedule not found" });
            return Ok(schedule);
        }

        // GET: api/schedule/class/{classId}
        [HttpGet("class/{classId}")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedulesByClass(long classId)
        {
            var schedules = await _context.Schedules
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .Where(s => s.ClassRoomId == classId)
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
            return Ok(schedules);
        }

        // GET: api/schedule/teacher/{teacherId}
        [HttpGet("teacher/{teacherId}")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedulesByTeacher(long teacherId)
        {
            var schedules = await _context.Schedules
                .Include(s => s.Subject)
                .Include(s => s.ClassRoom)
                .Where(s => s.TeacherId == teacherId)
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
            return Ok(schedules);
        }

        // GET: api/schedule/day/{dayOfWeek}
        [HttpGet("day/{dayOfWeek}")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedulesByDay(string dayOfWeek)
        {
            var schedules = await _context.Schedules
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .Include(s => s.ClassRoom)
                .Where(s => s.DayOfWeek.ToLower() == dayOfWeek.ToLower())
                .OrderBy(s => s.StartTime)
                .ToListAsync();
            return Ok(schedules);
        }

        // POST: api/schedule
        [HttpPost]
        public async Task<ActionResult<Schedule>> CreateSchedule([FromBody] Schedule schedule)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            schedule.CreatedAt = DateTime.UtcNow;
            schedule.UpdatedAt = DateTime.UtcNow;
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSchedule), new { id = schedule.Id }, schedule);
        }

        // PUT: api/schedule/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(long id, [FromBody] Schedule schedule)
        {
            if (id != schedule.Id)
                return BadRequest(new { message = "ID mismatch" });
            var existing = await _context.Schedules.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Schedule not found" });
            existing.SubjectId = schedule.SubjectId;
            existing.TeacherId = schedule.TeacherId;
            existing.ClassRoomId = schedule.ClassRoomId;
            existing.DayOfWeek = schedule.DayOfWeek;
            existing.StartTime = schedule.StartTime;
            existing.EndTime = schedule.EndTime;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/schedule/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(long id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
                return NotFound(new { message = "Schedule not found" });
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 