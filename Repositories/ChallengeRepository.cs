using Microsoft.EntityFrameworkCore;
using Quorom.Databases;
using Quorom.DbTables;

namespace Quorom.Repositories
{
    public class ChallengeRepository : IChallengeRepository
    {
        private readonly QuoromDbContext _db;
        public ChallengeRepository(QuoromDbContext db)
        {
            _db = db;
        }
        public async Task<Challenge> AddAsync(Challenge record)
        {
            await _db.Challenges.AddAsync(record);
            await _db.SaveChangesAsync();
            return record;
        }
        public async Task<IEnumerable<Challenge>> ReadAllAsync()
        {
            return await _db.Challenges.Where(a => a.IsDeleted == false).ToListAsync();
        }

        public async Task<Challenge?> ReadAsync(Guid id)
        {
            return await _db.Challenges.Where(a => a.IsDeleted == false).FirstOrDefaultAsync(a => a.ChallengeId == id);
        }

        public async Task<Challenge?> UpdateAsync(Challenge record)
        {
            var existingRecord = await _db.Challenges.FindAsync(record.ChallengeId);
            if (existingRecord != null)
            {
                existingRecord.Title = record.Title;
                existingRecord.Description = record.Description;
                existingRecord.ChallengeTypeId = record.ChallengeTypeId;
                existingRecord.UpdatedOnDateTime = record.UpdatedOnDateTime;
                existingRecord.UpdatedByUserId = record.UpdatedByUserId;
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
        public async Task<Challenge?> DeleteSoftAsync(Guid id, string user)
        {
            var existingRecord = await _db.Challenges.FindAsync(id);
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
        public async Task<Challenge?> DeleteAsync(Guid id)
        {
            var existingRecord = await _db.Challenges.FindAsync(id);
            if (existingRecord != null)
            {
                _db.Challenges.Remove(existingRecord);
                await _db.SaveChangesAsync();
                return existingRecord;
            }
            return null;
        }
    }
    public interface IChallengeRepository
    {
        Task<Challenge> AddAsync(Challenge record);
        Task<IEnumerable<Challenge>> ReadAllAsync();
        Task<Challenge?> ReadAsync(Guid id);
        Task<Challenge?> UpdateAsync(Challenge record);
        Task<Challenge?> DeleteSoftAsync(Guid id, string user);
        Task<Challenge?> DeleteAsync(Guid id);
    }
}
