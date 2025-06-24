using SQLite;
using SKANSAPUNG.MAUI.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SKANSAPUNG.MAUI.Services
{
    public class DatabaseService : IDatabaseService
    {
        private static SQLiteAsyncConnection _database;
        private static readonly string DbPath = Path.Combine(FileSystem.AppDataDirectory, "skansapung.db3");
        private static bool _initialized = false;

        public DatabaseService()
        {
            Task.Run(async () => await Init()).Wait();
        }

        private async Task Init()
        {
            if (_initialized) return;
            _database = new SQLiteAsyncConnection(DbPath);
            await _database.CreateTableAsync<LocalUser>();
            await _database.CreateTableAsync<LocalStudent>();
            await _database.CreateTableAsync<LocalAttendance>();
            await _database.CreateTableAsync<LocalAssessment>();
            await _database.CreateTableAsync<LocalStudentScore>();
            await _database.CreateTableAsync<LocalExtracurricular>();
            await _database.CreateTableAsync<ClassRoom>();
            await _database.CreateTableAsync<LocalDepartment>();
            await _database.CreateTableAsync<ScheduleItem>();
            _initialized = true;
        }

        // --- Student ---
        public Task<List<LocalStudent>> GetStudentsAsync() => _database.Table<LocalStudent>().ToListAsync();
        public Task<LocalStudent> GetStudentAsync(int id) => _database.Table<LocalStudent>().Where(s => s.Id == id).FirstOrDefaultAsync();
        public Task<LocalStudent> GetStudentAsync(long id) => _database.Table<LocalStudent>().Where(s => s.UserId == id.ToString()).FirstOrDefaultAsync();
        public Task<LocalStudent> GetStudentByUserIdAsync(long userId) => _database.Table<LocalStudent>().Where(s => s.UserId == userId.ToString()).FirstOrDefaultAsync();
        public Task<int> SaveStudentAsync(LocalStudent student) => student.Id != 0 ? _database.UpdateAsync(student) : _database.InsertAsync(student);
        public Task<int> DeleteStudentAsync(int id) => _database.DeleteAsync<LocalStudent>(id);

        public async Task ClearAndInsertAsync<T>(IEnumerable<T> items) where T : new()
        {
            await Init();
            await _database.RunInTransactionAsync(conn =>
            {
                conn.DeleteAll<T>();
                conn.InsertAll(items);
            });
        }

        public Task<List<LocalStudent>> GetUnsyncedStudentsAsync() => _database.Table<LocalStudent>().Where(s => !s.IsSynced).ToListAsync();
        public async Task MarkStudentsAsSyncedAsync(IEnumerable<int> ids)
        {
            var recordsToUpdate = await _database.Table<LocalStudent>().Where(s => ids.Contains(s.Id)).ToListAsync();
            foreach (var record in recordsToUpdate)
            {
                record.IsSynced = true;
            }
            await _database.UpdateAllAsync(recordsToUpdate);
        }

        // --- Attendance ---
        public Task<List<LocalAttendance>> GetAttendanceHistoryAsync() => _database.Table<LocalAttendance>().ToListAsync();
        public Task<LocalAttendance> GetAttendanceAsync(int id) => _database.Table<LocalAttendance>().Where(a => a.Id == id).FirstOrDefaultAsync();
        public Task<int> SaveAttendanceAsync(LocalAttendance attendance) => attendance.Id != 0 ? _database.UpdateAsync(attendance) : _database.InsertAsync(attendance);
        public Task<int> DeleteAttendanceAsync(int id) => _database.DeleteAsync<LocalAttendance>(id);
        public Task<LocalAttendance> GetLastUnfinishedAttendanceAsync() => _database.Table<LocalAttendance>().Where(a => a.EndTime == null).OrderByDescending(a => a.StartTime).FirstOrDefaultAsync();
        public Task<List<LocalAttendance>> GetUnsyncedAttendancesAsync() => _database.Table<LocalAttendance>().Where(a => !a.IsSynced).ToListAsync();
        public async Task MarkAttendancesAsSyncedAsync(IEnumerable<int> ids)
        {
            var recordsToUpdate = await _database.Table<LocalAttendance>().Where(a => ids.Contains(a.Id)).ToListAsync();
            foreach (var record in recordsToUpdate)
            {
                record.IsSynced = true;
            }
            await _database.UpdateAllAsync(recordsToUpdate);
        }

        // --- Assessments ---
        public Task<int> SaveAssessmentAsync(LocalAssessment assessment) => assessment.Id != 0 ? _database.UpdateAsync(assessment) : _database.InsertAsync(assessment);
        public Task<List<LocalAssessment>> GetAssessmentsAsync() => _database.Table<LocalAssessment>().ToListAsync();
        public Task<List<LocalAssessment>> GetAssessmentsByClassRoomAsync(long classRoomId) => _database.Table<LocalAssessment>().Where(a => a.ClassRoomId == classRoomId).ToListAsync();
        public Task SaveAssessmentsAsync(IEnumerable<Assessment> assessments)
        {
            var localAssessments = assessments.Select(a => new LocalAssessment
            {
                ServerId = a.Id.ToString(),
                ClassRoomId = a.ClassRoomId,
                TeacherId = a.TeacherId,
                Type = a.Type,
                Subject = a.Subject,
                AssessmentName = a.AssessmentName,
                Date = a.Date,
                Description = a.Description,
                IsSynced = true
            });
            return _database.InsertAllAsync(localAssessments);
        }

        // --- StudentScores ---
        public Task<int> SaveStudentScoreAsync(LocalStudentScore score) => score.Id != 0 ? _database.UpdateAsync(score) : _database.InsertAsync(score);
        public Task<List<LocalStudentScore>> GetStudentScoresAsync(long studentId) => _database.Table<LocalStudentScore>().Where(s => s.StudentId == studentId).ToListAsync();
        public Task SaveStudentScoresAsync(IEnumerable<StudentScore> scores)
        {
            var localScores = scores.Select(s => new LocalStudentScore
            {
                ServerId = s.Id.ToString(),
                AssessmentId = s.AssessmentId,
                StudentId = s.StudentId,
                Score = s.Score,
                Status = s.Status,
                IsSynced = true
            });
            return _database.InsertAllAsync(localScores);
        }

        // --- Extracurriculars ---
        public Task<int> SaveExtracurricularAsync(LocalExtracurricular extracurricular) => extracurricular.Id != 0 ? _database.UpdateAsync(extracurricular) : _database.InsertAsync(extracurricular);

        // --- User ---
        public Task<int> SaveUserAsync(LocalUser user) => user.Id != 0 ? _database.UpdateAsync(user) : _database.InsertAsync(user);
        public Task<LocalUser> GetUserAsync(long id) => _database.Table<LocalUser>().FirstOrDefaultAsync(u => u.ServerId == id.ToString());
        public Task<List<LocalUser>> GetUsersAsync(string userType) => _database.Table<LocalUser>().Where(u => u.UserType == userType).ToListAsync();

        // --- Department ---
        public Task<List<LocalDepartment>> GetDepartmentsAsync() => _database.Table<LocalDepartment>().ToListAsync();
        public Task SaveDepartmentsAsync(IEnumerable<Department> departments)
        {
            var localDepartments = departments.Select(d => new LocalDepartment { Id = d.Id, Name = d.Name, Kode = d.Kode });
            return _database.InsertAllAsync(localDepartments);
        }

        // --- Schedule ---
        public Task<List<ScheduleItem>> GetScheduleForClassAsync(long classRoomId)
        {
            return _database.Table<ScheduleItem>().Where(s => s.ClassRoomId == classRoomId).ToListAsync();
        }

        public async Task SaveScheduleAsync(IEnumerable<ScheduleItem> schedule)
        {
            if (schedule.Any())
            {
                var classRoomId = schedule.First().ClassRoomId;
                await _database.Table<ScheduleItem>().Where(s => s.ClassRoomId == classRoomId).DeleteAsync();
            }
            await _database.InsertAllAsync(schedule);
        }

        // --- Sync (Placeholder implementations) ---
        public Task<bool> SyncDataAsync()
        {
            return Task.FromResult(true);
        }

        public Task<bool> SyncStudentsAsync()
        {
            return Task.FromResult(true);
        }

        public Task<bool> SyncAttendanceAsync()
        {
            return Task.FromResult(true);
        }
    }
}
            record.IsSynced = true;
        }
        await _database.UpdateAllAsync(recordsToUpdate);
    }

    // Assessments
    public async Task<int> SaveAssessmentAsync(LocalAssessment assessment)
    {
        await Init();
        if (assessment.Id != 0)
            return await _database.UpdateAsync(assessment);
        else
            return await _database.InsertAsync(assessment);
    }

    public async Task SaveUsersAsync(IEnumerable<User> users)
    {
        await Init();
        foreach (var user in users)
        {
            var localUser = await _database.Table<LocalUser>().FirstOrDefaultAsync(u => u.ServerId == user.Id.ToString());
            if (localUser == null)
            {
                localUser = new LocalUser { ServerId = user.Id.ToString() };
            }

            localUser.Name = user.Name;
            localUser.Email = user.Email;
            localUser.UserType = user.UserType;
            localUser.Role = user.Role;
            localUser.Status = user.Status;
            localUser.Image = user.Image;
            localUser.ClassRoomId = user.ClassRoomId;
            localUser.IsSynced = true;

            await SaveLocalUserAsync(localUser);
        }
    }

    public async Task<LocalUser> GetUserAsync(long id)
    {
        await Init();
        return await _database.Table<LocalUser>().FirstOrDefaultAsync(u => u.ServerId == id.ToString());
    }

    public async Task<List<LocalUser>> GetUsersAsync(string userType)
    {
        await Init();
        return await _database.Table<LocalUser>().Where(u => u.UserType == userType).ToListAsync();
    }

    // Students
    public async Task<int> SaveStudentAsync(LocalStudent student)
    {
        if (student.Id != 0)
            return await _database.UpdateAsync(student);
        else
            return await _database.InsertAsync(student);
    }

    public async Task<LocalStudent> GetStudentAsync(long id)
    {
        await Init();
        return await _database.Table<LocalStudent>().Where(s => s.UserId == id.ToString()).FirstOrDefaultAsync();
    }

    public async Task<List<LocalStudent>> GetStudentsAsync()
    {
        await Init();
        return await _database.Table<LocalStudent>().ToListAsync();
    }

    public async Task<List<LocalStudent>> GetUnsyncedStudentsAsync()
    {
        await Init();
        return await _database.Table<LocalStudent>().Where(s => !s.IsSynced).ToListAsync();
    }

    public async Task MarkStudentsAsSyncedAsync(IEnumerable<int> ids)
    {
        await Init();
        var recordsToUpdate = await _database.Table<LocalStudent>().Where(s => ids.Contains(s.Id)).ToListAsync();
        foreach (var record in recordsToUpdate)
        {
            record.IsSynced = true;
        }
        await _database.UpdateAllAsync(recordsToUpdate);
    }

    public async Task SaveAssessmentsAsync(IEnumerable<Assessment> assessments)
    {
        await Init();
        foreach (var assessment in assessments)
        {
            var localAssessment = await _database.Table<LocalAssessment>().FirstOrDefaultAsync(a => a.ServerId == assessment.Id.ToString());
            if (localAssessment == null)
            {
                localAssessment = new LocalAssessment { ServerId = assessment.Id.ToString() };
            }
            localAssessment.ClassRoomId = assessment.ClassRoomId;
            localAssessment.TeacherId = assessment.TeacherId;
            localAssessment.Type = assessment.Type;
            localAssessment.Subject = assessment.Subject;
            localAssessment.AssessmentName = assessment.AssessmentName;
            localAssessment.Date = assessment.Date;
            localAssessment.Description = assessment.Description;
            localAssessment.IsSynced = true;
            await SaveLocalAssessmentAsync(localAssessment);
        }
    }

    public async Task SaveStudentScoresAsync(IEnumerable<StudentScore> scores)
    {
        await Init();
        foreach (var score in scores)
        {
            var localScore = await _database.Table<LocalStudentScore>().FirstOrDefaultAsync(s => s.ServerId == score.Id.ToString());
            if (localScore == null)
            {
                localScore = new LocalStudentScore { ServerId = score.Id.ToString() };
            }
            localScore.AssessmentId = score.AssessmentId;
            localScore.StudentId = score.StudentId;
            localScore.Score = score.Score;
            localScore.Status = score.Status;
            localScore.IsSynced = true;
            await SaveLocalStudentScoreAsync(localScore);
        }
    }

    // Extracurriculars
    public async Task<int> SaveExtracurricularAsync(LocalExtracurricular extracurricular)
    {
        if (extracurricular.Id != 0)
            return await _database.UpdateAsync(extracurricular);
        else
            return await _database.InsertAsync(extracurricular);
    }

    // Local Attendance
    public async Task<int> SaveAttendanceAsync(LocalAttendance attendance)
    {
        await Init();
        if (attendance.Id != 0)
            return await _database.UpdateAsync(attendance);
        else
            return await _database.InsertAsync(attendance);
    }
} 