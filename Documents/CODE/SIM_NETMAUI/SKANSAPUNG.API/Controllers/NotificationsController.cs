using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKANSAPUNG.API.Data;
using SKANSAPUNG.API.Models;
using SKANSAPUNG.API.Services;
using System.Collections.Generic;
using System.Linq;

namespace SKANSAPUNG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly IFcmTokenService _fcmTokenService;

        public NotificationsController(AppDbContext context, INotificationService notificationService, IFcmTokenService fcmTokenService)
        {
            _context = context;
            _notificationService = notificationService;
            _fcmTokenService = fcmTokenService;
        }

        // GET: api/notifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            var notifications = await _context.Notifications.OrderByDescending(n => n.CreatedAt).ToListAsync();
            return Ok(notifications);
        }

        // GET: api/notifications/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(long id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
                return NotFound(new { message = "Notification not found" });
            return Ok(notification);
        }

        // GET: api/notifications/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByUser(long userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return Ok(notifications);
        }

        // POST: api/notifications
        [HttpPost]
        public async Task<ActionResult<Notification>> CreateNotification([FromBody] Notification notification)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            notification.CreatedAt = DateTime.UtcNow;
            notification.IsRead = false;
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNotification), new { id = notification.Id }, notification);
        }

        // PUT: api/notifications/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(long id, [FromBody] Notification notification)
        {
            if (id != notification.Id)
                return BadRequest(new { message = "ID mismatch" });
            var existing = await _context.Notifications.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = "Notification not found" });
            existing.Title = notification.Title;
            existing.Body = notification.Body;
            existing.IsRead = notification.IsRead;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/notifications/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(long id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
                return NotFound(new { message = "Notification not found" });
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/notifications/send
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest notificationRequest)
        {
            if (notificationRequest == null)
                return BadRequest("Invalid notification request.");
            await _notificationService.SendNotificationAsync(notificationRequest);
            return Ok(new { message = "Notification sent" });
        }

        // POST: api/notifications/register-token
        [HttpPost("register-token")]
        public async Task<IActionResult> RegisterToken([FromBody] RegisterTokenRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.DeviceId))
                return BadRequest("Invalid token registration request.");
            await _fcmTokenService.RegisterTokenAsync(request.Token, request.DeviceId, request.Platform);
            return Ok(new { message = "Token registered" });
        }

        // POST: api/notifications/unregister-token
        [HttpPost("unregister-token")]
        public async Task<IActionResult> UnregisterToken([FromBody] RegisterTokenRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.DeviceId))
                return BadRequest("Invalid token unregister request.");
            await _fcmTokenService.UnregisterTokenAsync(request.DeviceId);
            return Ok(new { message = "Token unregistered" });
        }

        // POST: api/notifications/mark-as-read/{id}
        [HttpPost("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAsRead(long id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
                return NotFound(new { message = "Notification not found" });
            notification.IsRead = true;
            notification.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Notification marked as read" });
        }
    }
} 