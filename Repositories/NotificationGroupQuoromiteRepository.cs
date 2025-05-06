using Microsoft.EntityFrameworkCore;
using Quorom.Databases;
using Quorom.DbTables;
using static Quorom.ViewModels.NotificationGroupQuoromiteVM;

namespace Quorom.Repositories
{
    public class NotificationGroupQuoromiteRepository : INotificationGroupQuoromiteRepository
    {
        private readonly QuoromDbContext _db;
        public NotificationGroupQuoromiteRepository(QuoromDbContext db)
        {
            _db = db;
        }
        public async Task<NotificationGroupQuoromite> AddAsync(NotificationGroupQuoromite record)
        {
            await _db.NotificationGroupQuoromites.AddAsync(record);
            await _db.SaveChangesAsync();
            return record;
        }
        public async Task<IEnumerable<NotificationGroupQuoromite>> ReadAllAsync(Guid id)
        {
            return await _db.NotificationGroupQuoromites.Where(x => x.NotificationGroupId == id).ToListAsync();
        }

        public async Task<NotificationGroupQuoromite?> ReadAsync(Guid id)
        {
            return await _db.NotificationGroupQuoromites.FirstOrDefaultAsync(x => x.NotificationGroupQuoromiteId == id);
        }
        public async Task<GetNotificationGroupQuoromiteListVM?> ReadMergeAsync(Guid id)
        //Id = NotificationGroupId
        {
            var thisNotificationGroupQuoromite = await _db.NotificationGroupQuoromites.Where(x => x.NotificationGroupId == id && x.IsDeleted == false).ToListAsync();
            var thisNotificationGroup = await _db.NotificationGroups.ToListAsync();
            var thisQuoromite= await _db.Quoromites.ToListAsync();
            var mergeList = (from a in thisNotificationGroupQuoromite
                             join b in thisQuoromite on a.QuoromiteId equals b.QuoromiteId
                             select new
                             {
                                 a.NotificationGroupQuoromiteId,
                                 a.NotificationGroupId,
                                 a.QuoromiteId,
                                 b.FullName,
                                 b.Email,
                                 b.Description,
                                 b.IsActive                                
                             }).OrderByDescending(x => x.FullName).ToList();
            List<GetNotificationGroupQuoromiteVM> notificationGroupQuoromiteList = new List<GetNotificationGroupQuoromiteVM>();
            foreach (var record in mergeList)
            {
                GetNotificationGroupQuoromiteVM model = new()
                {
                    NotificationGroupQuoromiteId = record.NotificationGroupQuoromiteId,
                    NotificationGroupId = record.NotificationGroupId,
                    QuoromiteId = record.QuoromiteId,
                    FullName = record.FullName,
                    Email = record.Email,
                    Description = record.Description,
                    IsActive = record.IsActive
                };
                notificationGroupQuoromiteList.Add(model);
            }
            GetNotificationGroupQuoromiteListVM myNotificationGroupQuoromiteList = new();
            myNotificationGroupQuoromiteList.NotificationGroupName = (await _db.NotificationGroups.FirstOrDefaultAsync(x => x.NotificationGroupId == id)).Name;
            myNotificationGroupQuoromiteList.NotificationGroupId = id;
            myNotificationGroupQuoromiteList.GetNotificationGroupQuoromites = notificationGroupQuoromiteList;
            return myNotificationGroupQuoromiteList;
        }
        public async Task<NotificationGroupQuoromite?> UpdateAsync(NotificationGroupQuoromite record)
        {
            var existingRecord = await _db.NotificationGroupQuoromites.FindAsync(record.NotificationGroupQuoromiteId);
            if (existingRecord != null)
            {
                existingRecord.UpdatedOnDateTime = DateTime.Now;
                existingRecord.UpdatedByUserId = record.UpdatedByUserId;
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
        public async Task<NotificationGroupQuoromite?> DeleteSoftAsync(Guid id, string user)
        {
            var existingRecord = await _db.NotificationGroupQuoromites.FindAsync(id);
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
        public async Task<NotificationGroupQuoromite?> DeleteAsync(Guid id)
        {
            var existingRecord = await _db.NotificationGroupQuoromites.FindAsync(id);
            if (existingRecord != null)
            {
                _db.NotificationGroupQuoromites.Remove(existingRecord);
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }

        public async Task<NotificationGroupQuoromite?> GetByGroupAndQuoromiteAsync(Guid groupId, Guid quoromiteId)
        {
            return await _db.NotificationGroupQuoromites
                .FirstOrDefaultAsync(x =>
                    x.NotificationGroupId == groupId &&
                    x.QuoromiteId == quoromiteId &&
                    !x.IsDeleted);
        }
    }

    public interface INotificationGroupQuoromiteRepository
    {
        Task<NotificationGroupQuoromite> AddAsync(NotificationGroupQuoromite record);
        Task<IEnumerable<NotificationGroupQuoromite>> ReadAllAsync(Guid id);
        Task<NotificationGroupQuoromite?> ReadAsync(Guid id);
        Task<GetNotificationGroupQuoromiteListVM?> ReadMergeAsync(Guid id);
        Task<NotificationGroupQuoromite?> UpdateAsync(NotificationGroupQuoromite record);
        Task<NotificationGroupQuoromite?> DeleteSoftAsync(Guid id, string user);
        Task<NotificationGroupQuoromite?> DeleteAsync(Guid id);
        Task<NotificationGroupQuoromite?> GetByGroupAndQuoromiteAsync(Guid groupId, Guid quoromiteId);
    }
}
