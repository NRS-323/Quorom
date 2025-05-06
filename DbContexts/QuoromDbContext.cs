using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quorom.DbTables;

namespace Quorom.Databases
{
    public class QuoromDbContext : IdentityDbContext
    {
        public QuoromDbContext(DbContextOptions<QuoromDbContext> options) : base(options) { }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<ChallengeType> ChallengeTypes { get; set; }
        public DbSet<Initiative> Initiatives { get; set; }
        public DbSet<InitiativeTask> InitiativeTasks { get; set; }
        public DbSet<InitiativeType> InitiativeTypes { get; set; }
        public DbSet<NotificationGroup> NotificationGroups { get; set; }
        public DbSet<NotificationGroupQuoromite> NotificationGroupQuoromites { get; set; }
        public DbSet<NotificationLog> NotificationLogs { get; set; }
        public DbSet<QuoromUser> QuoromUsers { get; set; }
        public DbSet<Quoromite> Quoromites { get; set; }
        public DbSet<DbTables.Task> Tasks { get; set; }
        public DbSet<TaskChallenge> TaskChallenges { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Seed Roles
            var roles = new List<IdentityRole>
            {
                //Administrator
                new IdentityRole
                {
                    Name = MyConstants.QuoromRoleNames.Administrator,
                    NormalizedName = MyConstants.QuoromRoleNames.Administrator.ToUpper(),
                    Id = MyConstants.QuoromRoleGuids.Administrator,
                    ConcurrencyStamp = MyConstants.QuoromRoleGuids.Administrator,
                },
                //Contributer
                new IdentityRole
                {
                    Name = MyConstants.QuoromRoleNames.Contributer,
                    NormalizedName = MyConstants.QuoromRoleNames.Contributer.ToUpper(),
                    Id = MyConstants.QuoromRoleGuids.Contributer,
                    ConcurrencyStamp = MyConstants.QuoromRoleGuids.Contributer,
                },
                //Deleter
                new IdentityRole
                {
                    Name = MyConstants.QuoromRoleNames.Deleter,
                    NormalizedName = MyConstants.QuoromRoleNames.Deleter.ToUpper(),
                    Id = MyConstants.QuoromRoleGuids.Deleter,
                    ConcurrencyStamp = MyConstants.QuoromRoleGuids.Deleter,
                },
                //Modifier
                new IdentityRole
                {
                    Name = MyConstants.QuoromRoleNames.Modifier,
                    NormalizedName = MyConstants.QuoromRoleNames.Modifier.ToUpper(),
                    Id = MyConstants.QuoromRoleGuids.Modifier,
                    ConcurrencyStamp = MyConstants.QuoromRoleGuids.Modifier,
                },
                //SuperUser
                new IdentityRole
                {
                    Name = MyConstants.QuoromRoleNames.SuperUser,
                    NormalizedName = MyConstants.QuoromRoleNames.SuperUser.ToUpper(),
                    Id = MyConstants.QuoromRoleGuids.SuperUser,
                    ConcurrencyStamp = MyConstants.QuoromRoleGuids.SuperUser,
                },
                //Viewer
                new IdentityRole
                {
                    Name = MyConstants.QuoromRoleNames.Viewer,
                    NormalizedName = MyConstants.QuoromRoleNames.Viewer.ToUpper(),
                    Id = MyConstants.QuoromRoleGuids.Viewer,
                    ConcurrencyStamp = MyConstants.QuoromRoleGuids.Viewer,
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);
            //Create Super Admin
            var superAdmin = new QuoromUser
            {
                FirstName = "Quorom",
                LastName = "Administrator",
                Position = "Super Administrator",
                UserName = MyConstants.QuoromSuperAdmin.Email,
                Email = MyConstants.QuoromSuperAdmin.Email,
                CreatedByUserId = MyConstants.QuoromSuperAdmin.Email,
                CreatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                UpdatedByUserId = MyConstants.QuoromSuperAdmin.Email,
                UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                NormalizedEmail = MyConstants.QuoromSuperAdmin.Email.ToUpper(),
                NormalizedUserName = MyConstants.QuoromSuperAdmin.Email.ToUpper(),
                Id = MyConstants.QuoromSuperAdmin.Id,
            };
            superAdmin.PasswordHash = new PasswordHasher<QuoromUser>().HashPassword(superAdmin, MyConstants.QuoromSuperAdmin.Password);
            builder.Entity<QuoromUser>().HasData(superAdmin);
            //Seed Superuser with All Roles
            var superUserRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = MyConstants.QuoromRoleGuids.Administrator,
                    UserId = MyConstants.QuoromSuperAdmin.Id,
                },
                new IdentityUserRole<string>
                {
                    RoleId = MyConstants.QuoromRoleGuids.Contributer,
                    UserId = MyConstants.QuoromSuperAdmin.Id,
                },
                new IdentityUserRole<string>
                {
                    RoleId = MyConstants.QuoromRoleGuids.Deleter,
                    UserId = MyConstants.QuoromSuperAdmin.Id,
                },
                new IdentityUserRole<string>
                {
                    RoleId = MyConstants.QuoromRoleGuids.Modifier,
                    UserId = MyConstants.QuoromSuperAdmin.Id,
                },
                new IdentityUserRole<string>
                {
                    RoleId = MyConstants.QuoromRoleGuids.SuperUser,
                    UserId = MyConstants.QuoromSuperAdmin.Id,
                },
                new IdentityUserRole<string>
                {
                    RoleId = MyConstants.QuoromRoleGuids.Viewer,
                    UserId = MyConstants.QuoromSuperAdmin.Id,
                },
            };
            builder.Entity<IdentityUserRole<string>>().HasData(superUserRoles);
            //Seed Task Types
            var challengeTypes = new List<ChallengeType>
            {
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.Accountability),
                    Title = MyConstants.ChallengeTypeNames.Accountability,
                    Description = MyConstants.ChallengeTypeNames.Accountability,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.Ambiguity),
                    Title = MyConstants.ChallengeTypeNames.Ambiguity,
                    Description = MyConstants.ChallengeTypeNames.Ambiguity,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.Budget),
                    Title = MyConstants.ChallengeTypeNames.Budget,
                    Description = MyConstants.ChallengeTypeNames.Budget,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.Communication),
                    Title = MyConstants.ChallengeTypeNames.Communication,
                    Description = MyConstants.ChallengeTypeNames.Communication,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.Conflict),
                    Title = MyConstants.ChallengeTypeNames.Conflict,
                    Description = MyConstants.ChallengeTypeNames.Conflict,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.Competency),
                    Title = MyConstants.ChallengeTypeNames.Competency,
                    Description = MyConstants.ChallengeTypeNames.Competency,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.Deadline),
                    Title = MyConstants.ChallengeTypeNames.Deadline,
                    Description = MyConstants.ChallengeTypeNames.Deadline,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.Goal),
                    Title = MyConstants.ChallengeTypeNames.Goal,
                    Description = MyConstants.ChallengeTypeNames.Goal,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.Resource),
                    Title = MyConstants.ChallengeTypeNames.Resource,
                    Description = MyConstants.ChallengeTypeNames.Resource,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.Risk),
                    Title = MyConstants.ChallengeTypeNames.Risk,
                    Description = MyConstants.ChallengeTypeNames.Risk,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.ScopeCreep),
                    Title = MyConstants.ChallengeTypeNames.ScopeCreep,
                    Description = MyConstants.ChallengeTypeNames.ScopeCreep,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.Scheduling),
                    Title = MyConstants.ChallengeTypeNames.Scheduling,
                    Description = MyConstants.ChallengeTypeNames.Scheduling,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.Stakeholder),
                    Title = MyConstants.ChallengeTypeNames.Stakeholder,
                    Description = MyConstants.ChallengeTypeNames.Stakeholder,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new ChallengeType
                {
                    ChallengeTypeId = Guid.Parse(MyConstants.ChallengeTypeGuids.Technology),
                    Title = MyConstants.ChallengeTypeNames.Technology,
                    Description = MyConstants.ChallengeTypeNames.Technology,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
            };
            builder.Entity<ChallengeType>().HasData(challengeTypes);
            //Seed Initiative Types
            var initiativeTypes = new List<InitiativeType>
            {
                new InitiativeType
                {
                    InitiativeTypeId = Guid.Parse(MyConstants.InitiaiveTypeGuids.Education),
                    Title = MyConstants.InitiaiveTypeNames.Education,
                    Description = MyConstants.InitiaiveTypeNames.Education,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new InitiativeType
                {
                    InitiativeTypeId = Guid.Parse(MyConstants.InitiaiveTypeGuids.Environment),
                    Title = MyConstants.InitiaiveTypeNames.Environment,
                    Description = MyConstants.InitiaiveTypeNames.Environment,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new InitiativeType
                {
                    InitiativeTypeId = Guid.Parse(MyConstants.InitiaiveTypeGuids.Financial),
                    Title = MyConstants.InitiaiveTypeNames.Financial,
                    Description = MyConstants.InitiaiveTypeNames.Financial,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new InitiativeType
                {
                    InitiativeTypeId = Guid.Parse(MyConstants.InitiaiveTypeGuids.Health),
                    Title = MyConstants.InitiaiveTypeNames.Health,
                    Description = MyConstants.InitiaiveTypeNames.Health,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new InitiativeType
                {
                    InitiativeTypeId = Guid.Parse(MyConstants.InitiaiveTypeGuids.Housing),
                    Title = MyConstants.InitiaiveTypeNames.Housing,
                    Description = MyConstants.InitiaiveTypeNames.Housing,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new InitiativeType
                {
                    InitiativeTypeId = Guid.Parse(MyConstants.InitiaiveTypeGuids.Policy),
                    Title = MyConstants.InitiaiveTypeNames.Policy,
                    Description = MyConstants.InitiaiveTypeNames.Policy,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new InitiativeType
                {
                    InitiativeTypeId = Guid.Parse(MyConstants.InitiaiveTypeGuids.Security),
                    Title = MyConstants.InitiaiveTypeNames.Security,
                    Description = MyConstants.InitiaiveTypeNames.Security,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new InitiativeType
                {
                    InitiativeTypeId = Guid.Parse(MyConstants.InitiaiveTypeGuids.Strategic),
                    Title = MyConstants.InitiaiveTypeNames.Strategic,
                    Description = MyConstants.InitiaiveTypeNames.Strategic,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                }
            };
            builder.Entity<InitiativeType>().HasData(initiativeTypes);
            //Seed Task Types
            var taskTypes = new List<TaskType>
            {
                new TaskType
                {
                    TaskTypeId = Guid.Parse(MyConstants.TaskTypeGuids.Approval),
                    Title = MyConstants.TaskTypeNames.Approval,
                    Description = MyConstants.TaskTypeNames.Approval,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new TaskType
                {
                    TaskTypeId = Guid.Parse(MyConstants.TaskTypeGuids.Coordinated),
                    Title = MyConstants.TaskTypeNames.Coordinated,
                    Description = MyConstants.TaskTypeNames.Coordinated,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new TaskType
                {
                    TaskTypeId = Guid.Parse(MyConstants.TaskTypeGuids.Dependency),
                    Title = MyConstants.TaskTypeNames.Dependency,
                    Description = MyConstants.TaskTypeNames.Dependency,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new TaskType
                {
                    TaskTypeId = Guid.Parse(MyConstants.TaskTypeGuids.FollowUp),
                    Title = MyConstants.TaskTypeNames.FollowUp,
                    Description = MyConstants.TaskTypeNames.FollowUp,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new TaskType
                {
                    TaskTypeId = Guid.Parse(MyConstants.TaskTypeGuids.Incidental),
                    Title = MyConstants.TaskTypeNames.Incidental,
                    Description = MyConstants.TaskTypeNames.Incidental,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
                new TaskType
                {
                    TaskTypeId = Guid.Parse(MyConstants.TaskTypeGuids.Planned),
                    Title = MyConstants.TaskTypeNames.Planned,
                    Description = MyConstants.TaskTypeNames.Planned,
                    IsActive = true,
                    CreatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    CreatedOnDateTime =  new DateTime(2025, 04, 01, 8, 00, 0),
                    UpdatedByUserId = MyConstants.QuoromSuperAdmin.Id,
                    UpdatedOnDateTime = new DateTime(2025, 04, 01, 8, 00, 0),
                    IsDeleted = false,
                    DeletedByUserId = null,
                    DeletedOnDateTime = null,
                },
            };
            builder.Entity<TaskType>().HasData(taskTypes);
        }
    }
}