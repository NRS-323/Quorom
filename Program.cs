using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quorom;
using Quorom.Databases;
using Quorom.DbTables;
using Quorom.Repositories;
using Quorom.Services;
using Umbono.Authorise;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("QuoromConnection") ?? throw new InvalidOperationException("Connection string 'QuoromConnection' not found.");

builder.Services.AddDbContext<QuoromDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("QuoromConnection")));

builder.Services.AddIdentity<QuoromUser, IdentityRole>().AddEntityFrameworkStores<QuoromDbContext>().AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailWithMailKit, EmailWithMailKit>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.SignIn.RequireConfirmedEmail = false;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole(MyConstants.QuoromRoleNames.Administrator));
    options.AddPolicy("AdminAndUser", policy => policy.RequireRole(MyConstants.QuoromRoleNames.Administrator).RequireRole((MyConstants.QuoromRoleNames.Viewer)));
    options.AddPolicy("AdminRole_CreateClaim", policy => policy.RequireRole(MyConstants.QuoromRoleNames.Administrator).RequireClaim("Create", "True"));
    options.AddPolicy("AdminRole_CreateUpdateDeleteClaim", policy => policy
    .RequireRole(MyConstants.QuoromRoleNames.Administrator)
    .RequireClaim("Create", "True")
    .RequireClaim("Update", "True")
    .RequireClaim("Delete", "True")
    );
    options.AddPolicy("AdminRole_CreateUpdateDeleteClaim_OrSuperUser", policy => policy.RequireAssertion(context =>
            AdminRole_CreateUpdateDeleteClaim_OrSuperUser(context)));
    options.AddPolicy("OnlyUmbonoAdminChecker", p => p.Requirements.Add(new OnlyQuoromAdminChecker()));
});

builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

builder.Services.AddScoped<IBannerRepository, BannerRepository>();

builder.Services.AddScoped<IChallengeRepository, ChallengeRepository>();

builder.Services.AddScoped<IChallengeTypeRepository, ChallengeTypeRepository>();

builder.Services.AddScoped<IInitiativeRepository, InitiativeRepository>();

builder.Services.AddScoped<IInitiativeTaskRepository, InitiativeTaskRepository>();

builder.Services.AddScoped<IInitiativeTypeRepository, InitiativeTypeRepository>();

builder.Services.AddScoped<INotificationGroupQuoromiteRepository, NotificationGroupQuoromiteRepository>();

builder.Services.AddScoped<INotificationGroupRepository, NotificationGroupRepository>();

builder.Services.AddScoped<INotificationLogRepository, NotificationLogRepository>();

builder.Services.AddScoped<IQuoromiteRepository, QuoromiteRepository>();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddScoped<ITaskChallengeRepository, TaskChallengeRepository>();

builder.Services.AddScoped<ITaskTypeRepository, TaskTypeRepository>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

bool AdminRole_CreateUpdateDeleteClaim_OrSuperUser(AuthorizationHandlerContext context)
{
    return
(
    context.User.IsInRole(MyConstants.QuoromRoleNames.Administrator)
    && context.User.HasClaim(c => c.Type == "Create" && c.Value == "True")
    && context.User.HasClaim(c => c.Type == "Update" && c.Value == "True")
    && context.User.HasClaim(c => c.Type == "Delete" && c.Value == "True")
)
|| context.User.IsInRole(MyConstants.QuoromRoleNames.Administrator);
}