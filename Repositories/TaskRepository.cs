using Microsoft.EntityFrameworkCore;
using Quorom.Databases;
using Quorom.DbTables;
using Quorom.ViewModels.Dashboard;
using Quorom.ViewModels.MyActivities;
using Task = Quorom.DbTables.Task;


namespace Quorom.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly QuoromDbContext _db;
        public TaskRepository(QuoromDbContext db)
        {
            _db = db;
        }
        public async Task<DbTables.Task> AddAsync(DbTables.Task record)
        {
            await _db.Tasks.AddAsync(record);
            await _db.SaveChangesAsync();
            return record;
        }
        public async Task<IEnumerable<DbTables.Task>> ReadAllAsync()
        {
            return await _db.Tasks.Include(t => t.Quoromites).ToListAsync();
        }
        public async Task<IEnumerable<DbTables.Task>> ReadAllActiveAsync()
        {
            return await _db.Tasks.Include(t => t.Quoromites).Where(t => t.IsDeleted == false).OrderBy(x => x.Title).ToListAsync();
        }
        public async Task<IEnumerable<DbTables.Task>> ReadAllDeletedAsync()
        {
            return await _db.Tasks.Include(t => t.Quoromites).Where(t => t.IsDeleted == true).OrderBy(x => x.Title).ToListAsync();
        }
        public async Task<DbTables.Task?> ReadAsync(Guid id)
        {
            return await _db.Tasks.Include(t => t.Quoromites).FirstOrDefaultAsync(x => x.TaskId == id);
        }
        public async Task<DbTables.Task?> UpdateAsync(DbTables.Task record)
        {
            var existingRecord = await _db.Tasks.Include(t => t.Quoromites).FirstOrDefaultAsync(t => t.TaskId == record.TaskId);
            if (existingRecord != null)
            {
                existingRecord.Title = record.Title;
                existingRecord.TaskTypeId = record.TaskTypeId;
                existingRecord.Description = record.Description;
                existingRecord.PlannedStartDateTime = record.PlannedStartDateTime;
                existingRecord.PlannedStopDateTime = record.PlannedStopDateTime;
                existingRecord.ActualStartDateTime = record.ActualStartDateTime;
                existingRecord.ActualStopDateTime = record.ActualStopDateTime;
                existingRecord.Status = record.Status;
                existingRecord.SubStatus = record.SubStatus;
                existingRecord.Quoromites = record.Quoromites;
                existingRecord.UpdatedOnDateTime = record.UpdatedOnDateTime;
                existingRecord.UpdatedByUserId = record.UpdatedByUserId;
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
        public async Task<DbTables.Task?> CompletedAsync(Guid id, string user)
        {
            var existingRecord = await _db.Tasks.FindAsync(id);
            if (existingRecord != null)
            {
                existingRecord.IsCompleted = true;
                existingRecord.CompletedOnDateTime = DateTime.Now;
                existingRecord.CompletedByUserId = user;
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
        public async Task<DbTables.Task?> DeleteSoftAsync(Guid id, string user)
        {
            var existingRecord = await _db.Tasks.FindAsync(id);
            if (existingRecord != null)
            {
                existingRecord.IsDeleted = true;
                existingRecord.DeletedOnDateTime = DateTime.Now;
                existingRecord.DeletedByUserId = user;
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
        public async Task<DbTables.Task?> DeleteAsync(Guid id)
        {
            var existingRecord = await _db.Tasks.Include(t => t.Quoromites).FirstOrDefaultAsync(t => t.TaskId == id);
            if (existingRecord != null)
            {
                _db.Tasks.Remove(existingRecord);
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }

        public async Task<List<QuoromiteDashboardVM>> GetQuoromiteTaskStatsAsync(List<Quoromite> quoromites)
        {
            var allTasks = await ReadAllAsync(); // Ensure this includes Quoromites (use Include if needed)

            var stats = quoromites.Select(quoromite =>
            {
                var tasksForThisUser = allTasks
                    .Where(t => t.Quoromites.Any(q => q.QuoromiteId == quoromite.QuoromiteId) && !t.IsDeleted)
                    .ToList();

                return new QuoromiteDashboardVM
                {
                    QuoromiteId = quoromite.QuoromiteId,
                    FullName = quoromite.FullName,
                    Email = quoromite.Email,
                    TotalTasks = tasksForThisUser.Count,
                    CompletedTasks = tasksForThisUser.Count(t => t.IsCompleted)
                };
            }).ToList();

            return stats;
        }

        public async Task<List<InitiativeProgressViewModel>> GetInitiativeProgressStatsAsync()
        {
            var initiatives = await _db.Initiatives.Where(i => !i.IsDeleted).ToListAsync();

            var initiativeTasks = await _db.InitiativeTasks
                .Include(it => it.Task)
                    .ThenInclude(t => t.Quoromites) // Load Quoromites for each Task
                .Include(it => it.Initiative)
                .Where(it => !it.IsDeleted && !it.Task.IsDeleted)
                .ToListAsync();

            var grouped = initiativeTasks
                .GroupBy(it => it.Initiative)
                .Select(group =>
                {
                    var allTasks = group.Select(it => it.Task).ToList();

                    // Flatten and deduplicate all involved users from the tasks
                    var involvedUsers = allTasks
                        .SelectMany(t => t.Quoromites)
                        .GroupBy(q => q.QuoromiteId)
                        .Select(g => new QuoromiteDashboardVM
                        {
                            QuoromiteId = g.Key,
                            FullName = g.First().FullName
                            // Add other fields if needed
                        }).ToList();

                    return new InitiativeProgressViewModel
                    {
                        InitiativeId = group.Key.InitiativeId,
                        Title = group.Key.Title,
                        Owner = group.Key.Owner,
                        Status = group.Key.Status,
                        TotalTasks = group.Count(),
                        CompletedTasks = group.Count(it => it.Task.IsCompleted),
                        InvolvedUsers = involvedUsers
                    };
                }).ToList();

            return grouped;
        }
        public async Task<IEnumerable<Quorom.DbTables.Task>> GetTasksByQuoromiteIdAsync(string userId)
        {
            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return new List<Quorom.DbTables.Task>();
            }

            return await _db.Tasks
                .Include(t => t.Quoromites)
                .Where(t => t.IsDeleted == false && t.Quoromites.Any(q => q.QuoromiteId == parsedUserId))
                .OrderBy(t => t.PlannedStartDateTime)
                .ToListAsync();
        }
        public async Task<IEnumerable<Quorom.DbTables.Task>> GetTasksByUserId(string userId)
        {
            return await _db.Tasks
                .Include(t => t.Quoromites)
                .Where(t => t.Quoromites.Any(q => q.QuoromiteId.ToString() == userId) && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<InitiativeProgressViewModel>> GetMyInitiativesWithStats(string userEmail)
        {
            var user = await _db.Quoromites.FirstOrDefaultAsync(q => q.Email == userEmail);
            if (user == null) return new List<InitiativeProgressViewModel>();

            var initiativeTasks = await _db.InitiativeTasks
                .Include(it => it.Initiative)
                .Include(it => it.Task)
                .ThenInclude(t => t.Quoromites)
                .Where(it => !it.IsDeleted && it.Task.Quoromites.Any(q => q.QuoromiteId == user.QuoromiteId))
                .ToListAsync();

            var grouped = initiativeTasks
                .GroupBy(it => it.Initiative)
                .Select(group =>
                {
                    var initiative = group.Key;
                    var totalTasks = _db.InitiativeTasks
                        .Where(it => it.InitiativeId == initiative.InitiativeId && !it.IsDeleted && !it.Task.IsDeleted)
                        .Count();

                    var completedTasks = _db.InitiativeTasks
                        .Where(it => it.InitiativeId == initiative.InitiativeId && !it.IsDeleted && !it.Task.IsDeleted && it.Task.IsCompleted)
                        .Count();

                    var userTasks = group.Select(g => g.Task).Distinct().ToList();

                    return new InitiativeProgressViewModel
                    {
                        InitiativeId = initiative.InitiativeId,
                        Title = initiative.Title,
                        Owner = initiative.Owner,
                        Status = initiative.Status,
                        TotalTasks = totalTasks,
                        CompletedTasks = completedTasks,
                        UserTasks = userTasks.Select(t => new TaskListViewModel
                        {
                            TaskId = t.TaskId,
                            Title = t.Title,
                            IsCompleted = t.IsCompleted,
                            DueDate = t.PlannedStopDateTime,
                            Status = t.Status
                        }).ToList()
                    };
                }).ToList();

            return grouped;
        }




    }

    public interface ITaskRepository
    {
        Task<DbTables.Task> AddAsync(DbTables.Task record);
        Task<IEnumerable<DbTables.Task>> ReadAllAsync();
        Task<IEnumerable<DbTables.Task>> ReadAllActiveAsync();
        Task<IEnumerable<DbTables.Task>> ReadAllDeletedAsync();
        Task<DbTables.Task?> ReadAsync(Guid id);
        Task<DbTables.Task?> UpdateAsync(DbTables.Task record);
        Task<DbTables.Task?> CompletedAsync(Guid id, string user);
        Task<DbTables.Task?> DeleteSoftAsync(Guid id, string user);
        Task<DbTables.Task?> DeleteAsync(Guid id);
        Task<List<QuoromiteDashboardVM>> GetQuoromiteTaskStatsAsync(List<Quoromite> quoromites);
        Task<List<InitiativeProgressViewModel>> GetInitiativeProgressStatsAsync();
        Task<IEnumerable<DbTables.Task>> GetTasksByQuoromiteIdAsync(string userId);
        Task<IEnumerable<Quorom.DbTables.Task>> GetTasksByUserId(string userId);
    }
}
