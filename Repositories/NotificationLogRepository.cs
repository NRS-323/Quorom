using Quorom.Databases;
using Quorom.DbTables;

namespace Quorom.Repositories
{
    public class NotificationLogRepository : INotificationLogRepository
    {
        private readonly QuoromDbContext _db;

        public NotificationLogRepository(QuoromDbContext db)
        {
            _db = db;
        }
        public async Task<NotificationLog> AddLogAsync(NotificationLog log)
        {
            await _db.NotificationLogs.AddAsync(log);
            await _db.SaveChangesAsync();
            return log;
        }
    }
    public interface INotificationLogRepository
    {
        Task<NotificationLog> AddLogAsync(NotificationLog log);
    }
}

