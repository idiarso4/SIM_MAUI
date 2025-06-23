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
    public class AttendanceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AttendanceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/attendance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendanceRecords()
        {
            try
            {
                var attendanceRecords = await _context.Attendances
                    .Include(a => a.User)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();

                return Ok(attendanceRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving attendance records", error = ex.Message });
            }
        }

        // GET: api/attendance/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> GetAttendanceRecord(long id)
        {
            try
            {
                var attendance = await _context.Attendances
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (attendance == null)
                {
                    return NotFound(new { message = "Attendance record not found" });
                }

                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving attendance record", error = ex.Message });
            }
        }

        // GET: api/attendance/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendanceByUser(long userId)
        {
            try
            {
                var attendanceRecords = await _context.Attendances
                    .Where(a => a.UserId == userId)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();

                return Ok(attendanceRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving user attendance", error = ex.Message });
            }
        }

        // GET: api/attendance/today
        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetTodayAttendance()
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                var attendanceRecords = await _context.Attendances
                    .Include(a => a.User)
                    .Where(a => a.CreatedAt.Date == today)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();

                return Ok(attendanceRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving today's attendance", error = ex.Message });
            }
        }

        // GET: api/attendance/date/{date}
        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendanceByDate(string date)
        {
            try
            {
                if (!DateTime.TryParse(date, out DateTime targetDate))
                {
                    return BadRequest(new { message = "Invalid date format. Use YYYY-MM-DD" });
                }

                var attendanceRecords = await _context.Attendances
                    .Include(a => a.User)
                    .Where(a => a.CreatedAt.Date == targetDate.Date)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();

                return Ok(attendanceRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving attendance by date", error = ex.Message });
            }
        }

        // POST: api/attendance
        [HttpPost]
        public async Task<ActionResult<Attendance>> CreateAttendance([FromBody] Attendance attendance)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validate user exists
                var user = await _context.Users.FindAsync(attendance.UserId);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found" });
                }

                // Set timestamps
                attendance.CreatedAt = DateTime.UtcNow;
                attendance.UpdatedAt = DateTime.UtcNow;

                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAttendanceRecord), new { id = attendance.Id }, attendance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating attendance record", error = ex.Message });
            }
        }

        // PUT: api/attendance/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendance(long id, [FromBody] Attendance attendance)
        {
            try
            {
                if (id != attendance.Id)
                {
                    return BadRequest(new { message = "ID mismatch" });
                }

                var existingAttendance = await _context.Attendances.FindAsync(id);
                if (existingAttendance == null)
                {
                    return NotFound(new { message = "Attendance record not found" });
                }

                // Update fields
                existingAttendance.StartLatitude = attendance.StartLatitude;
                existingAttendance.StartLongitude = attendance.StartLongitude;
                existingAttendance.StartTime = attendance.StartTime;
                existingAttendance.EndLatitude = attendance.EndLatitude;
                existingAttendance.EndLongitude = attendance.EndLongitude;
                existingAttendance.EndTime = attendance.EndTime;
                existingAttendance.IsLeave = attendance.IsLeave;
                existingAttendance.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating attendance record", error = ex.Message });
            }
        }

        // DELETE: api/attendance/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(long id)
        {
            try
            {
                var attendance = await _context.Attendances.FindAsync(id);
                if (attendance == null)
                {
                    return NotFound(new { message = "Attendance record not found" });
                }

                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting attendance record", error = ex.Message });
            }
        }

        // POST: api/attendance/check-in
        [HttpPost("check-in")]
        public async Task<ActionResult<Attendance>> CheckIn([FromBody] AttendanceDto checkInData)
        {
            try
            {
                // Validate user exists
                var user = await _context.Users.FindAsync(checkInData.UserId);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found" });
                }

                // Check if user already checked in today
                var today = DateTime.UtcNow.Date;
                var existingAttendance = await _context.Attendances
                    .Where(a => a.UserId == checkInData.UserId && a.CreatedAt.Date == today)
                    .FirstOrDefaultAsync();

                if (existingAttendance != null)
                {
                    return BadRequest(new { message = "User already checked in today" });
                }

                // Create new attendance record
                var attendance = new Attendance
                {
                    UserId = checkInData.UserId,
                    ScheduleLatitude = checkInData.ScheduleLatitude,
                    ScheduleLongitude = checkInData.ScheduleLongitude,
                    ScheduleStartTime = checkInData.ScheduleStartTime,
                    ScheduleEndTime = checkInData.ScheduleEndTime,
                    StartLatitude = checkInData.StartLatitude,
                    StartLongitude = checkInData.StartLongitude,
                    StartTime = checkInData.StartTime,
                    IsLeave = checkInData.IsLeave,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAttendanceRecord), new { id = attendance.Id }, attendance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error during check-in", error = ex.Message });
            }
        }

        // POST: api/attendance/check-out
        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOut([FromBody] AttendanceDto checkOutData)
        {
            try
            {
                // Find today's attendance record
                var today = DateTime.UtcNow.Date;
                var attendance = await _context.Attendances
                    .Where(a => a.UserId == checkOutData.UserId && a.CreatedAt.Date == today)
                    .FirstOrDefaultAsync();

                if (attendance == null)
                {
                    return BadRequest(new { message = "No check-in record found for today" });
                }

                if (attendance.EndTime.HasValue)
                {
                    return BadRequest(new { message = "User already checked out today" });
                }

                // Update check-out information
                attendance.EndLatitude = checkOutData.EndLatitude;
                attendance.EndLongitude = checkOutData.EndLongitude;
                attendance.EndTime = checkOutData.EndTime;
                attendance.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Check-out successful", attendance });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error during check-out", error = ex.Message });
            }
        }

        // POST: api/attendance/sync
        [HttpPost("sync")]
        public async Task<IActionResult> SyncAttendance([FromBody] List<AttendanceDto> attendanceRecords)
        {
            try
            {
                if (attendanceRecords == null || !attendanceRecords.Any())
                {
                    return BadRequest(new { message = "No attendance records to sync" });
                }

                var syncedIds = new List<object>();

                foreach (var record in attendanceRecords)
                {
                    if (record.Id > 0)
                    {
                        // Update existing record
                        var existingRecord = await _context.Attendances.FindAsync(record.Id);
                        if (existingRecord != null)
                        {
                            existingRecord.StartLatitude = record.StartLatitude;
                            existingRecord.StartLongitude = record.StartLongitude;
                            existingRecord.StartTime = record.StartTime;
                            existingRecord.EndLatitude = record.EndLatitude;
                            existingRecord.EndLongitude = record.EndLongitude;
                            existingRecord.EndTime = record.EndTime;
                            existingRecord.IsLeave = record.IsLeave;
                            existingRecord.UpdatedAt = DateTime.UtcNow;

                            syncedIds.Add(new { LocalId = record.LocalId, ServerId = existingRecord.Id });
                        }
                    }
                    else
                    {
                        // Create new record
                        var newRecord = new Attendance
                        {
                            UserId = record.UserId,
                            ScheduleLatitude = record.ScheduleLatitude,
                            ScheduleLongitude = record.ScheduleLongitude,
                            ScheduleStartTime = record.ScheduleStartTime,
                            ScheduleEndTime = record.ScheduleEndTime,
                            StartLatitude = record.StartLatitude,
                            StartLongitude = record.StartLongitude,
                            StartTime = record.StartTime,
                            EndLatitude = record.EndLatitude,
                            EndLongitude = record.EndLongitude,
                            EndTime = record.EndTime,
                            IsLeave = record.IsLeave,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        _context.Attendances.Add(newRecord);
                        await _context.SaveChangesAsync();

                        syncedIds.Add(new { LocalId = record.LocalId, ServerId = newRecord.Id });
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { SyncedIds = syncedIds, Message = $"Successfully synced {attendanceRecords.Count} attendance records" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error syncing attendance records", error = ex.Message });
            }
        }

        // GET: api/attendance/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetAttendanceStatistics([FromQuery] long? userId = null, [FromQuery] string? dateRange = null)
        {
            try
            {
                var query = _context.Attendances.AsQueryable();

                if (userId.HasValue)
                {
                    query = query.Where(a => a.UserId == userId.Value);
                }

                if (!string.IsNullOrEmpty(dateRange))
                {
                    var dates = dateRange.Split(',');
                    if (dates.Length == 2 && DateTime.TryParse(dates[0], out DateTime startDate) && DateTime.TryParse(dates[1], out DateTime endDate))
                    {
                        query = query.Where(a => a.CreatedAt.Date >= startDate.Date && a.CreatedAt.Date <= endDate.Date);
                    }
                }

                var totalRecords = await query.CountAsync();
                var presentCount = await query.CountAsync(a => !a.IsLeave);
                var absentCount = await query.CountAsync(a => a.IsLeave);
                var lateCount = await query.CountAsync(a => a.StartTime > a.ScheduleStartTime);

                var statistics = new
                {
                    TotalRecords = totalRecords,
                    Present = presentCount,
                    Absent = absentCount,
                    Late = lateCount,
                    AttendanceRate = totalRecords > 0 ? (double)presentCount / totalRecords * 100 : 0
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving attendance statistics", error = ex.Message });
            }
        }
    }
} 