using Microsoft.EntityFrameworkCore;
using Quorom.Databases;
using Quorom.DbTables;

namespace Quorom.Repositories
{
    public class ChallengeTypeRepository : IChallengeTypeRepository
    {
        private readonly QuoromDbContext _db;
        public ChallengeTypeRepository(QuoromDbContext db)
        {
            _db = db;
        }
        public async Task<ChallengeType> AddAsync(ChallengeType record)
        {
            await _db.ChallengeTypes.AddAsync(record);
            await _db.SaveChangesAsync();
            return record;
        }
        public async Task<IEnumerable<ChallengeType>> ReadAllAsync()
        {
            return await _db.ChallengeTypes.ToListAsync();
        }
        public async Task<IEnumerable<ChallengeType>> ReadAllActiveAsync()
        {
            return await _db.ChallengeTypes.Where(t => t.IsActive == true && t.IsDeleted == false).OrderBy(x => x.Title).ToListAsync();
        }
        public async Task<IEnumerable<ChallengeType>> ReadAllDeletedAsync()
        {
            return await _db.ChallengeTypes.Where(t => t.IsDeleted == true).OrderBy(x => x.Title).ToListAsync();
        }
        public async Task<ChallengeType?> ReadAsync(Guid id)
        {
            return await _db.ChallengeTypes.FirstOrDefaultAsync(x => x.ChallengeTypeId == id);
        }
        public async Task<ChallengeType?> UpdateAsync(ChallengeType record)
        {
            var existingRecord = await _db.ChallengeTypes.FindAsync(record.ChallengeTypeId);
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
        public async Task<ChallengeType?> DeleteSoftAsync(Guid id, string user)
        {
            var existingRecord = await _db.ChallengeTypes.FindAsync(id);
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
        public async Task<ChallengeType?> DeleteAsync(Guid id)
        {
            var existingRecord = await _db.ChallengeTypes.FindAsync(id);
            if (existingRecord != null)
            {
                _db.ChallengeTypes.Remove(existingRecord);
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
    }
    public interface IChallengeTypeRepository
    {
        Task<ChallengeType> AddAsync(ChallengeType record);
        Task<IEnumerable<ChallengeType>> ReadAllAsync();
        Task<IEnumerable<ChallengeType>> ReadAllActiveAsync();
        Task<IEnumerable<ChallengeType>> ReadAllDeletedAsync();
        Task<ChallengeType?> ReadAsync(Guid id);
        Task<ChallengeType?> UpdateAsync(ChallengeType record);
        Task<ChallengeType?> DeleteSoftAsync(Guid id, string user);
        Task<ChallengeType?> DeleteAsync(Guid id);
    }
}
