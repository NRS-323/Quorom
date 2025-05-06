using Microsoft.EntityFrameworkCore;
using Quorom.Databases;
using Quorom.DbTables;
using static Quorom.ViewModels.TaskChallengeVM;

namespace Quorom.Repositories
{
    public class TaskChallengeRepository : ITaskChallengeRepository
    {
        private readonly QuoromDbContext _db;
        public TaskChallengeRepository(QuoromDbContext db)
        {
            _db = db;
        }
        public async Task<TaskChallenge> AddAsync(TaskChallenge record)
        {
            await _db.TaskChallenges.AddAsync(record);
            await _db.SaveChangesAsync();
            return record;
        }
        public async Task<IEnumerable<TaskChallenge>> ReadAllAsync(Guid id)
        {
            return await _db.TaskChallenges.Where(x => x.TaskId == id).ToListAsync();
        }

        public async Task<TaskChallenge?> ReadAsync(Guid id)
        {
            return await _db.TaskChallenges.FirstOrDefaultAsync(x => x.TaskChallengeId == id);
        }
        public async Task<TaskChallenge?> GetTaskFromTask(Guid id)
        {
            return await _db.TaskChallenges.FirstOrDefaultAsync(x => x.TaskId == id);
        }
        public async Task<GetTaskChallengeListVM?> ReadMergeAsync(Guid id)
        {
            //id is the TaskId
            var thisTaskChallenge = await _db.TaskChallenges.Where(x => x.TaskId == id && x.IsDeleted == false).ToListAsync();
            var thisChallenge = await _db.Challenges.ToListAsync();
            var thisChallengeType = await _db.ChallengeTypes.ToListAsync();

            var mergeList = (from a in thisTaskChallenge
                             join b in thisChallenge on a.ChallengeId equals b.ChallengeId
                             join c in thisChallengeType on b.ChallengeTypeId equals c.ChallengeTypeId
                             select new
                             {
                                 TaskChallengeId = a.TaskChallengeId,
                                 TaskId = a.TaskId,
                                 ChallengeId = a.ChallengeId,
                                 Title = b.Title,
                                 ChallengeType = c.Title,
                                 Description = b.Description,
                                 CreatedOnDateTime = b.CreatedOnDateTime,
                                 CreatedByUserId = b.CreatedByUserId,
                                 UpdatedOnDateTime = b.UpdatedOnDateTime

            }).OrderByDescending(x => x.CreatedOnDateTime).ToList();
            List<GetTaskChallengeVM> initiativeTaskList = new List<GetTaskChallengeVM>();

            foreach (var record in mergeList)
            {
                GetTaskChallengeVM model = new()
                {
                    TaskChallengeId = record.TaskChallengeId,
                    TaskId = record.TaskId,
                    ChallengeId = record.ChallengeId,
                    Title = record.Title,
                    ChallengeType = record.ChallengeType,
                    Description = record.Description,
                    CreatedOnDateTime = record.CreatedOnDateTime,
                    CreatedByUserId = record.CreatedByUserId,
                    UpdatedOnDateTime = record.UpdatedOnDateTime
                    };
                initiativeTaskList.Add(model);
            }
            GetTaskChallengeListVM myTaskChallengeList = new();
            myTaskChallengeList.TaskTitle = (await _db.Tasks.FirstOrDefaultAsync(x => x.TaskId == id)).Title;
            myTaskChallengeList.TaskId = id;
            myTaskChallengeList.TaskChallenges = initiativeTaskList;
            return myTaskChallengeList;
        }
        public async Task<TaskChallenge?> UpdateAsync(TaskChallenge record)
        {
            var existingRecord = await _db.TaskChallenges.FirstOrDefaultAsync(r => r.TaskChallengeId == record.TaskChallengeId);
            if (existingRecord != null)
            {
                existingRecord.UpdatedOnDateTime = DateTime.Now;
                existingRecord.UpdatedByUserId = record.UpdatedByUserId;
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
        public async Task<TaskChallenge?> DeleteSoftAsync(Guid id, string user)
        {
            var existingRecord = await _db.TaskChallenges.FindAsync(id);
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
        public async Task<TaskChallenge?> DeleteAsync(Guid id)
        {
            var existingRecord = await _db.TaskChallenges.FindAsync(id);
            if (existingRecord != null)
            {
                _db.TaskChallenges.Remove(existingRecord);
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
    }
    public interface ITaskChallengeRepository
    {
        Task<TaskChallenge> AddAsync(TaskChallenge record);
        Task<IEnumerable<TaskChallenge>> ReadAllAsync(Guid id);
        Task<TaskChallenge?> ReadAsync(Guid id);
        Task<TaskChallenge?> GetTaskFromTask(Guid id);
        Task<GetTaskChallengeListVM?> ReadMergeAsync(Guid id);
        Task<TaskChallenge?> UpdateAsync(TaskChallenge record);
        Task<TaskChallenge?> DeleteSoftAsync(Guid id, string user);
        Task<TaskChallenge?> DeleteAsync(Guid id);
    }
}
