using Microsoft.EntityFrameworkCore;
using Quorom.Databases;
using Quorom.DbTables;

namespace Quorom.Repositories
{
    public class InitiativeTypeRepository : IInitiativeTypeRepository
    {
        private readonly QuoromDbContext _db;
        public InitiativeTypeRepository(QuoromDbContext db)
        {
            _db = db;
        }
        public async Task<InitiativeType> AddAsync(InitiativeType record)
        {
            await _db.InitiativeTypes.AddAsync(record);
            await _db.SaveChangesAsync();
            return record;
        }
        public async Task<IEnumerable<InitiativeType>> ReadAllAsync()
        {
            return await _db.InitiativeTypes.ToListAsync();
        }
        public async Task<IEnumerable<InitiativeType>> ReadAllActiveAsync()
        {
            return await _db.InitiativeTypes.Where(t => t.IsActive == true && t.IsDeleted == false).OrderBy(x => x.Title).ToListAsync();
        }
        public async Task<IEnumerable<InitiativeType>> ReadAllDeletedAsync()
        {
            return await _db.InitiativeTypes.Where(t => t.IsDeleted == true).OrderBy(x => x.Title).ToListAsync();
        }
        public async Task<InitiativeType?> ReadAsync(Guid id)
        {
            return await _db.InitiativeTypes.FirstOrDefaultAsync(x => x.InitiativeTypeId == id);
        }
        public async Task<InitiativeType?> UpdateAsync(InitiativeType record)
        {
            var existingRecord = await _db.InitiativeTypes.FindAsync(record.InitiativeTypeId);
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
        public async Task<InitiativeType?> DeleteSoftAsync(Guid id, string user)
        {
            var existingRecord = await _db.InitiativeTypes.FindAsync(id);
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
        public async Task<InitiativeType?> DeleteAsync(Guid id)
        {
            var existingRecord = await _db.InitiativeTypes.FindAsync(id);
            if (existingRecord != null)
            {
                _db.InitiativeTypes.Remove(existingRecord);
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
    }
    public interface IInitiativeTypeRepository
    {
        Task<InitiativeType> AddAsync(InitiativeType record);
        Task<IEnumerable<InitiativeType>> ReadAllAsync();
        Task<IEnumerable<InitiativeType>> ReadAllActiveAsync();
        Task<IEnumerable<InitiativeType>> ReadAllDeletedAsync();
        Task<InitiativeType?> ReadAsync(Guid id);
        Task<InitiativeType?> UpdateAsync(InitiativeType record);
        Task<InitiativeType?> DeleteSoftAsync(Guid id, string user);
        Task<InitiativeType?> DeleteAsync(Guid id);
    }
}
