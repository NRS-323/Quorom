using Microsoft.EntityFrameworkCore;
using Quorom.Databases;
using Quorom.DbTables;
using static Quorom.ViewModels.AuditLogVM;

namespace Quorom.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly QuoromDbContext _db;

        public AuditLogRepository(QuoromDbContext db)
        {
            _db = db;
        }
        public async Task<AuditLog> AddLogAsync(AuditLog log)
        {
            await _db.AuditLogs.AddAsync(log);
            await _db.SaveChangesAsync();
            return log;
        }
        public async Task<IEnumerable<GetAuditLogs>> ReadAllAsync()
        {
            var logs = await _db.AuditLogs.ToListAsync();
            var users = await _db.QuoromUsers.ToListAsync();
            var roles = await _db.Roles.ToListAsync();
            var mergeList = (from a in logs
                             join b in users on a.UserId equals b.Email
                             join c in roles on b.RoleId equals c.Id
                             select new
                             {
                                 AuditLogId = a.AuditLogId,
                                 LogDateTime = a.CreatedOnDateTime,
                                 Activity = a.Type,
                                 Listener = (b.FirstName + " " + b.LastName),
                                 Postion = b.Position
                             }).OrderBy(x => x.Listener).ToList();
            List<GetAuditLogs> list = new List<GetAuditLogs>();
            foreach (var log in list)
            {
                GetAuditLogs model = new()
                {
                    AuditLogId = log.AuditLogId,
                    LogDateTime = log.LogDateTime,
                    Activity = log.Activity,
                    Listener = log.Listener,
                    Position = log.Position,
                };
                list.Add(model);
            }
            return list;
        }
    }
    public interface IAuditLogRepository
    {
        Task<AuditLog> AddLogAsync(AuditLog log);
        Task<IEnumerable<GetAuditLogs>> ReadAllAsync();
    }
}
