using Microsoft.EntityFrameworkCore;
using Quorom.Databases;
using Quorom.DbTables;

namespace Quorom.Repositories
{
    public class TaskTypeRepository : ITaskTypeRepository
    {
        private readonly QuoromDbContext _db;
        public TaskTypeRepository(QuoromDbContext db)
        {
            _db = db;
        }
        public async Task<TaskType> AddAsync(TaskType record)
        {
            await _db.TaskTypes.AddAsync(record);
            await _db.SaveChangesAsync();
            return record;
        }
        public async Task<IEnumerable<TaskType>> ReadAllAsync()
        {
            return await _db.TaskTypes.ToListAsync();
        }
        public async Task<IEnumerable<TaskType>> ReadAllActiveAsync()
        {
            return await _db.TaskTypes.Where(t => t.IsActive == true && t.IsDeleted == false).OrderBy(x => x.Title).ToListAsync();
        }
        public async Task<IEnumerable<TaskType>> ReadAllDeletedAsync()
        {
            return await _db.TaskTypes.Where(t => t.IsDeleted == true).OrderBy(x => x.Title).ToListAsync();
        }
        public async Task<TaskType?> ReadAsync(Guid id)
        {
            return await _db.TaskTypes.FirstOrDefaultAsync(x => x.TaskTypeId == id);
        }
        public async Task<TaskType?> UpdateAsync(TaskType record)
        {
            var existingRecord = await _db.TaskTypes.FindAsync(record.TaskTypeId);
            if (existingRecord != null)
            {
                existingRecord.Title = record.Title;
                existingRecord.Description = record.Description;
                existingRecord.IsActive = record.IsActive;
                existingRecord.UpdatedOnDateTime = record.UpdatedOnDateTime;
                existingRecord.UpdatedByUserId = record.UpdatedByUserId;
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
        public async Task<TaskType?> DeleteSoftAsync(Guid id, string user)
        {
            var existingRecord = await _db.TaskTypes.FindAsync(id);
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
        public async Task<TaskType?> DeleteAsync(Guid id)
        {
            var existingRecord = await _db.TaskTypes.FindAsync(id);
            if (existingRecord != null)
            {
                _db.TaskTypes.Remove(existingRecord);
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
    }
    public interface ITaskTypeRepository
    {
        Task<TaskType> AddAsync(TaskType record);
        Task<IEnumerable<TaskType>> ReadAllAsync();
        Task<IEnumerable<TaskType>> ReadAllActiveAsync();
        Task<IEnumerable<TaskType>> ReadAllDeletedAsync();
        Task<TaskType?> ReadAsync(Guid id);
        Task<TaskType?> UpdateAsync(TaskType record);
        Task<TaskType?> DeleteSoftAsync(Guid id, string user);
        Task<TaskType?> DeleteAsync(Guid id);
    }
}
