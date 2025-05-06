using Microsoft.EntityFrameworkCore;
using Quorom.Databases;
using Quorom.DbTables;
using static Quorom.ViewModels.InitiativeTaskVM;

namespace Quorom.Repositories
{
    public class InitiativeTaskRepository : IInitiativeTaskRepository
    {
        private readonly QuoromDbContext _db;
        public InitiativeTaskRepository(QuoromDbContext db)
        {
            _db = db;
        }
        public async Task<InitiativeTask> AddAsync(InitiativeTask record)
        {
            await _db.InitiativeTasks.AddAsync(record);
            await _db.SaveChangesAsync();
            return record;
        }
        public async Task<IEnumerable<InitiativeTask>> ReadAllAsync(Guid id)
        {
            return await _db.InitiativeTasks.Where(x => x.InitiativeId == id).ToListAsync();
        }

        public async Task<InitiativeTask?> ReadAsync(Guid id)
        {
            return await _db.InitiativeTasks.FirstOrDefaultAsync(x => x.InitiativeTaskId == id);
        }
        public async Task<InitiativeTask?> GetInitiativeFromTask(Guid id)
        {
            return await _db.InitiativeTasks.FirstOrDefaultAsync(x => x.TaskId == id);
        }
        public async Task<GetInitiativeTaskListVM?> ReadMergeAsync(Guid id)
        {
            //id is the InitiativeId
            var thisInitiativeTask = await _db.InitiativeTasks.Where(x => x.InitiativeId == id && x.IsDeleted == false).ToListAsync();
            var thisTask = await _db.Tasks.Include(q => q.Quoromites).ToListAsync();
            var thisTaskType = await _db.TaskTypes.ToListAsync();

            var mergeList = (from a in thisInitiativeTask
                             join b in thisTask on a.TaskId equals b.TaskId
                             join c in thisTaskType on b.TaskTypeId equals c.TaskTypeId
                             select new
                             {
                                 InitiativeTaskId = a.InitiativeTaskId,
                                 Title = b.Title,
                                 TaskId = a.TaskId,
                                 InitiativeId = a.InitiativeId,
                                 PlannedStartDateTime = b.PlannedStartDateTime,
                                 PlannedStopDateTime = b.PlannedStopDateTime,
                                 ActualStartDateTime = b.ActualStartDateTime,
                                 ActualStopDateTime = b.ActualStopDateTime,
                                 CreatedOnDateTime = b.CreatedOnDateTime,
                                 UpdatedOnDateTime = b.UpdatedOnDateTime,
                                 Quoromites = b.Quoromites,
                                 TaskType = c.Title,
                                 Status = b.Status,
                             }).OrderByDescending(x => x.PlannedStartDateTime).ToList();
            List<GetInitiativeTaskVM> initiativeTaskList = new List<GetInitiativeTaskVM>();

            foreach (var record in mergeList)
            {
                GetInitiativeTaskVM model = new()
                {
                    InitiativeTaskId = record.InitiativeTaskId,
                    TaskId = record.TaskId,
                    TaskType = record.TaskType,
                    InitiativeId = record.InitiativeId,
                    Title = record.Title,
                    PlannedStartDateTime = record.PlannedStartDateTime,
                    PlannedStopDateTime = record.PlannedStopDateTime,
                    ActualStartDateTime = record.ActualStartDateTime,
                    ActualStopDateTime = record.ActualStopDateTime,
                    CreatedOnDateTime = record.CreatedOnDateTime,
                    UpdatedOnDateTime = record.UpdatedOnDateTime,
                    Quoromites = record.Quoromites,
                    ChallengeCount = await _db.TaskChallenges.Where(c => c.TaskId == record.TaskId).CountAsync(),
                    Status = record.Status,
                };
                initiativeTaskList.Add(model);
            }
            GetInitiativeTaskListVM myInitiativeTaskList = new();
            myInitiativeTaskList.InitiativeTitle = (await _db.Initiatives.FirstOrDefaultAsync(x => x.InitiativeId == id)).Title;
            myInitiativeTaskList.InitiativeId = id;
            myInitiativeTaskList.InitiativeTasks = initiativeTaskList;
            return myInitiativeTaskList;
        }
        public async Task<InitiativeTask?> UpdateAsync(InitiativeTask record)
        {
            var existingRecord = await _db.InitiativeTasks.FirstOrDefaultAsync(r => r.InitiativeTaskId == record.InitiativeTaskId);
            if (existingRecord != null)
            {
                existingRecord.UpdatedOnDateTime = DateTime.Now;
                existingRecord.UpdatedByUserId = record.UpdatedByUserId;
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
        public async Task<InitiativeTask?> DeleteSoftAsync(Guid id, string user)
        {
            var existingRecord = await _db.InitiativeTasks.FindAsync(id);
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
        public async Task<InitiativeTask?> DeleteAsync(Guid id)
        {
            var existingRecord = await _db.InitiativeTasks.FindAsync(id);
            if (existingRecord != null)
            {
                _db.InitiativeTasks.Remove(existingRecord);
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
    }
    public interface IInitiativeTaskRepository
    {
        Task<InitiativeTask> AddAsync(InitiativeTask record);
        Task<IEnumerable<InitiativeTask>> ReadAllAsync(Guid id);
        Task<InitiativeTask?> ReadAsync(Guid id);
        Task<InitiativeTask?> GetInitiativeFromTask(Guid id);
        Task<GetInitiativeTaskListVM?> ReadMergeAsync(Guid id);
        Task<InitiativeTask?> UpdateAsync(InitiativeTask record);
        Task<InitiativeTask?> DeleteSoftAsync(Guid id, string user);
        Task<InitiativeTask?> DeleteAsync(Guid id);
    }
}
