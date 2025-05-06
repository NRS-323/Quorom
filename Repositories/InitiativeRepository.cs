using Microsoft.EntityFrameworkCore;
using Quorom.Databases;
using Quorom.DbTables;
using static Quorom.ViewModels.InitiativeVM;
using Quorom.ViewModels.MyActivities;

namespace Quorom.Repositories
{
    public class InitiativeRepository : IInitiativeRepository
    {
        private readonly QuoromDbContext _db;
        public InitiativeRepository(QuoromDbContext db)
        {
            _db = db;
        }

        public async Task<Initiative> AddAsync(Initiative record)
        {
            await _db.Initiatives.AddAsync(record);
            await _db.SaveChangesAsync();
            return record;
        }

        public async Task<IEnumerable<Initiative>> ReadAllAsync()
        {
            return await _db.Initiatives.Where(a => a.IsDeleted == false).ToListAsync();
        }

        public async Task<Initiative?> ReadAsync(Guid id)
        {
            return await _db.Initiatives.Where(a => a.IsDeleted == false)
                                        .FirstOrDefaultAsync(a => a.InitiativeId == id);
        }

        public async Task<List<GetInitiativesVM>> GetInitiativesAsync()
        {
            var initiatives = await _db.Initiatives.ToListAsync();
            var initiativeTypes = await _db.InitiativeTypes.ToListAsync();

            var initiativeListRaw = (from a in initiatives
                                     join b in initiativeTypes on a.InitiativeTypeId equals b.InitiativeTypeId
                                     select new
                                     {
                                         InitiativeId = a.InitiativeId,
                                         Title = a.Title,
                                         Description = a.Description,
                                         InitiativeType = b.Title,
                                         Owner = a.Owner,
                                         Objective = a.Objective,
                                         Status = a.Status,
                                         IsArchived = a.IsArchived,
                                         AddedBy = a.CreatedByUserId,
                                         AddedOn = a.CreatedOnDateTime,
                                     }).OrderByDescending(x => x.AddedOn).ToList();

            List<GetInitiativesVM> initiativeList = new List<GetInitiativesVM>();
            foreach (var item in initiativeListRaw)
            {
                GetInitiativesVM model = new()
                {
                    InitiativeId = item.InitiativeId,
                    Title = item.Title,
                    Description = item.Description,
                    InitiativeType = item.InitiativeType,
                    Objective = item.Objective,
                    Status = item.Status,
                    Owner = item.Owner,
                    IsArchived = item.IsArchived,
                    AddedBy = item.AddedBy,
                    AddedOn = item.AddedOn,
                };
                initiativeList.Add(model);
            }
            return initiativeList;
        }

        public async Task<Initiative?> UpdateAsync(Initiative record)
        {
            var existingRecord = await _db.Initiatives.FindAsync(record.InitiativeId);
            if (existingRecord != null)
            {
                existingRecord.Title = record.Title;
                existingRecord.Description = record.Description;
                existingRecord.InitiativeTypeId = record.InitiativeTypeId;
                existingRecord.Owner = record.Owner;
                existingRecord.Objective = record.Objective;
                existingRecord.Status = record.Status;
                existingRecord.IsArchived = record.IsArchived;
                existingRecord.UpdatedOnDateTime = record.UpdatedOnDateTime;
                existingRecord.UpdatedByUserId = record.UpdatedByUserId;
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }

        public async Task<Initiative?> DeleteSoftAsync(Guid id, string user)
        {
            var existingRecord = await _db.Initiatives.FindAsync(id);
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

        public async Task<Initiative?> DeleteAsync(Guid id)
        {
            var existingRecord = await _db.Initiatives.FindAsync(id);
            if (existingRecord != null)
            {
                _db.Initiatives.Remove(existingRecord);
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }

        public async Task<IEnumerable<Initiative>> GetInitiativesByOwnerAsync(string userId)
        {
            return await _db.Initiatives
                .Where(i => i.Owner == userId && i.IsDeleted == false)
                .OrderBy(i => i.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Initiative>> GetInitiativesByUserId(string userId)
        {
            return await _db.Initiatives
                .Where(i => i.Owner == userId && !i.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<InitiativeActivityVM>> GetInitiativesWithUserTasksAsync(Guid quoromiteId)
        {
            var initiativesWithUserTasks = await _db.InitiativeTasks
                .Include(it => it.Initiative)
                .Include(it => it.Task)
                    .ThenInclude(t => t.Quoromites)
                .Where(it =>
                    !it.IsDeleted &&
                    !it.Task.IsDeleted &&
                    it.Task.Quoromites.Any(q => q.QuoromiteId == quoromiteId))
                .ToListAsync();

            var grouped = initiativesWithUserTasks
                .GroupBy(it => it.Initiative)
                .Select(g =>
                {
                    var userTasks = g
                        .Where(it => it.Task.Quoromites.Any(q => q.QuoromiteId == quoromiteId))
                        .Select(it => new Quorom.ViewModels.MyActivities.TaskListViewModel
                        {
                            TaskId = it.Task.TaskId,
                            Title = it.Task.Title,
                            IsCompleted = it.Task.IsCompleted,
                            DueDate = it.Task.PlannedStopDateTime
                        })
                        .ToList();

                    var totalTasks = _db.InitiativeTasks
                        .Where(it => it.InitiativeId == g.Key.InitiativeId &&
                                     !it.IsDeleted &&
                                     !it.Task.IsDeleted)
                        .Select(it => it.Task);

                    var completed = totalTasks.Count(t => t.IsCompleted);
                    var total = totalTasks.Count();

                    return new InitiativeActivityVM
                    {
                        InitiativeId = g.Key.InitiativeId,
                        Title = g.Key.Title,
                        Status = g.Key.Status,
                        Objective = g.Key.Objective,
                        Owner = g.Key.Owner,
                        UserTasks = userTasks,
                        TotalTasks = total,
                        CompletedTasks = completed
                    };

                })
                .ToList();

            return grouped;
        }

    }
}



    
    public interface IInitiativeRepository
    {
        Task<Initiative> AddAsync(Initiative record);
        Task<IEnumerable<Initiative>> ReadAllAsync();
        Task<Initiative?> ReadAsync(Guid id);
        Task<List<GetInitiativesVM>> GetInitiativesAsync();
        Task<Initiative?> UpdateAsync(Initiative record);
        Task<Initiative?> DeleteSoftAsync(Guid id, string user);
        Task<Initiative?> DeleteAsync(Guid id);
        Task<IEnumerable<Initiative>> GetInitiativesByOwnerAsync(string userId);
        Task<IEnumerable<Initiative>> GetInitiativesByUserId(string userId);
    // My Activities: Initiatives + User Tasks + Progress
    Task<List<InitiativeActivityVM>> GetInitiativesWithUserTasksAsync(Guid quoromiteId);
}

