using Microsoft.EntityFrameworkCore;
using Quorom.Databases;
using Quorom.DbTables;

namespace Quorom.Repositories
{
    public class QuoromiteRepository : IQuoromiteRepository
    {
        private readonly QuoromDbContext _db;
        public QuoromiteRepository(QuoromDbContext db)
        {
            _db = db;
        }
        public async Task<Quoromite> AddAsync(Quoromite record)
        {
            await _db.Quoromites.AddAsync(record);
            await _db.SaveChangesAsync();
            return record;
        }
        public async Task<IEnumerable<Quoromite>> ReadAllAsync()
        {
            return await _db.Quoromites.ToListAsync();
        }
        public async Task<IEnumerable<Quoromite>> ReadAllActiveAsync()
        {
            return await _db.Quoromites.Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync();
        }
        public async Task<Quoromite?> ReadAsync(Guid id)
        {
            return await _db.Quoromites.Where(a => a.IsDeleted == false).FirstOrDefaultAsync(a => a.QuoromiteId == id);
        }
        public async Task<Quoromite?> UpdateAsync(Quoromite record)
        {
            var existingRecord = await _db.Quoromites.FindAsync(record.QuoromiteId);
            if (existingRecord != null)
            {
                existingRecord.FirstName = record.FirstName;
                existingRecord.LastName = record.LastName;
                existingRecord.FullName = record.FullName;
                existingRecord.Description = record.Description;
                existingRecord.Email = record.Email;
                existingRecord.IsActive = record.IsActive;
                existingRecord.UpdatedOnDateTime = record.UpdatedOnDateTime;
                existingRecord.UpdatedByUserId = record.UpdatedByUserId;
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
        public async Task<Quoromite?> DeleteSoftAsync(Guid id, string user)
        {
            var existingRecord = await _db.Quoromites.FindAsync(id);
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
        public async Task<Quoromite?> DeleteAsync(Guid id)
        {
            var existingRecord = await _db.Quoromites.FindAsync(id);
            if (existingRecord != null)
            {
                _db.Quoromites.Remove(existingRecord);
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
        public async Task<Quoromite?> GetQuoromiteByEmailAsync(string email)
        {
            return await _db.Quoromites
                .FirstOrDefaultAsync(q => q.Email.ToLower() == email.ToLower() && !q.IsDeleted);
        }

    }
    public interface IQuoromiteRepository
    {
        Task<Quoromite> AddAsync(Quoromite record);
        Task<IEnumerable<Quoromite>> ReadAllAsync();
        Task<IEnumerable<Quoromite>> ReadAllActiveAsync();
        Task<Quoromite?> ReadAsync(Guid id);
        Task<Quoromite?> UpdateAsync(Quoromite record);
        Task<Quoromite?> DeleteSoftAsync(Guid id, string user);
        Task<Quoromite?> DeleteAsync(Guid id);
        Task<Quoromite?> GetQuoromiteByEmailAsync(string email);

    }
}
