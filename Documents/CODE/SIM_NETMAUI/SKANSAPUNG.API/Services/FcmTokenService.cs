using Microsoft.EntityFrameworkCore;
using SKANSAPUNG.API.Data;
using SKANSAPUNG.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SKANSAPUNG.API.Services
{
    public class FcmTokenService : IFcmTokenService
    {
        private readonly AppDbContext _context;

        public FcmTokenService(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegisterTokenAsync(string token, string deviceId, string platform)
        {
            var existingToken = await _context.FcmTokens.FirstOrDefaultAsync(t => t.DeviceId == deviceId);

            if (existingToken != null)
            {
                existingToken.Token = token;
                existingToken.Platform = platform;
                existingToken.IsActive = true;
                existingToken.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var newToken = new FcmToken
                {
                    Token = token,
                    DeviceId = deviceId,
                    Platform = platform,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.FcmTokens.Add(newToken);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeactivateTokenAsync(string deviceId)
        {
            var token = await _context.FcmTokens.FirstOrDefaultAsync(t => t.DeviceId == deviceId);
            if (token != null)
            {
                token.IsActive = false;
                token.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
} 