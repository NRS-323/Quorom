using Microsoft.EntityFrameworkCore;
using Quorom.Databases;
using Quorom.DbTables;

namespace Quorom.Repositories
{
    public class NotificationGroupRepository : INotificationGroupRepository
    {
        private readonly QuoromDbContext _db;
        public NotificationGroupRepository(QuoromDbContext db)
        {
            _db = db;
        }
        public async Task<NotificationGroup> AddAsync(NotificationGroup record)
        {
            await _db.NotificationGroups.AddAsync(record);
            await _db.SaveChangesAsync();
            return record;
        }
        public async Task<IEnumerable<NotificationGroup>> ReadAllAsync()
        {
            return await _db.NotificationGroups.ToListAsync();
        }
        public async Task<IEnumerable<NotificationGroup>> ReadAllActiveAsync()
        {
            return await _db.NotificationGroups.Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync();
        }
        public async Task<NotificationGroup?> ReadAsync(Guid id)
        {
            return await _db.NotificationGroups.Where(a => a.IsDeleted == false).FirstOrDefaultAsync(a => a.NotificationGroupId == id);
        }
        public async Task<NotificationGroup?> UpdateAsync(NotificationGroup record)
        {
            var existingRecord = await _db.NotificationGroups.FindAsync(record.NotificationGroupId);
            if (existingRecord != null)
            {
                existingRecord.Name = record.Name;
                existingRecord.Description = record.Description;
                existingRecord.IsActive = record.IsActive;
                existingRecord.UpdatedOnDateTime = record.UpdatedOnDateTime;
                existingRecord.UpdatedByUserId = record.UpdatedByUserId;
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
        public async Task<NotificationGroup?> DeleteSoftAsync(Guid id, string user)
        {
            var existingRecord = await _db.NotificationGroups.FindAsync(id);
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
        public async Task<NotificationGroup?> DeleteAsync(Guid id)
        {
            var existingRecord = await _db.NotificationGroups.FindAsync(id);
            if (existingRecord != null)
            {
                _db.NotificationGroups.Remove(existingRecord);
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
    }
    public interface INotificationGroupRepository
    {
        Task<NotificationGroup> AddAsync(NotificationGroup record);
        Task<IEnumerable<NotificationGroup>> ReadAllAsync();
        Task<IEnumerable<NotificationGroup>> ReadAllActiveAsync();
        Task<NotificationGroup?> ReadAsync(Guid id);
        Task<NotificationGroup?> UpdateAsync(NotificationGroup record);
        Task<NotificationGroup?> DeleteSoftAsync(Guid id, string user);
        Task<NotificationGroup?> DeleteAsync(Guid id);
    }
}
