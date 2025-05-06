using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quorom.DbTables;
using Quorom.Repositories;
using static Quorom.ViewModels.TaskChallengeVM;

namespace Quorom.Controllers
{
    public class TaskChallengeController : Controller
    {
        private readonly ITaskChallengeRepository _taskChallengeStore;
        private readonly IChallengeRepository _challengeStore;
        private readonly IInitiativeTaskRepository _initiativeTaskStore;
        private readonly IQuoromiteRepository _quoromiteList;
        private readonly IChallengeTypeRepository _challengeTypeList;
        private readonly ITaskRepository _task;
        private readonly IBannerRepository _bannerList;
        private readonly UserManager<QuoromUser> _userManager;
        private readonly IAuditLogRepository _log;
        public TaskChallengeController
            (
            ITaskChallengeRepository taskChallengeStore,
            IChallengeRepository challengeStore,
            IInitiativeTaskRepository initiativeTaskStore,
            IQuoromiteRepository quoromiteList,
            IChallengeTypeRepository challengeTypeList,
            ITaskRepository task,
            IBannerRepository bannerList,
            UserManager<QuoromUser> userManager,
            IAuditLogRepository log
            )
        {
            _taskChallengeStore = taskChallengeStore;
            _challengeStore = challengeStore;
            _initiativeTaskStore = initiativeTaskStore;
            _quoromiteList = quoromiteList;
            _challengeTypeList = challengeTypeList;
            _task = task;
            _bannerList = bannerList;
            _userManager = userManager;
            _log = log;
        }

        [HttpGet]
        public async Task<IActionResult> AddTaskChallenge(Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var myTask = await _task.ReadAsync(id);
            var model = new AddTaskChallengeVM
            {
                TaskTitle = (myTask != null) ? myTask.Title : "ERROR!",
                ChallengeTypes = (await _challengeTypeList.ReadAllActiveAsync()).Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.ChallengeTypeId.ToString()
                }),
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Contributer}")]
        public async Task<IActionResult> AddTaskChallenge(AddTaskChallengeVM model, Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var myTask = await _task.ReadAsync(id);
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                model.TaskTitle = (myTask != null) ? myTask.Title : "ERROR!";
                model.ChallengeTypes = (await _challengeTypeList.ReadAllActiveAsync()).Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.ChallengeTypeId.ToString()
                });
                TempData[MyConstants.Messages.Error] = "Pay attention to the data you are entering!";
                return View(model);
            }
            var user = (await _userManager.GetUserAsync(User)).Email;

            var thisChallenge = new Challenge()
            {
                Title = model.Title,
                ChallengeTypeId = model.ChallengeTypeId,
                Description = model.Description,
                CreatedOnDateTime = DateTime.Now,
                CreatedByUserId = user,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
                IsDeleted = false,
                DeletedByUserId = null,
                DeletedOnDateTime = null,
            };
            var newChallenge = await _challengeStore.AddAsync(thisChallenge);

            var thisTaskChallenge = new TaskChallenge()
            {
                TaskId = id,
                ChallengeId = newChallenge.ChallengeId,
                CreatedOnDateTime = DateTime.Now,
                CreatedByUserId = user,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
                IsDeleted = false,
                DeletedByUserId = null,
                DeletedOnDateTime = null,
            };
            var newTaskChallenge = await _taskChallengeStore.AddAsync(thisTaskChallenge);

            var challengeLog = new AuditLog()
            {
                UserId = user,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Type = MyConstants.ProcessType.AddRecord,
                Table = MyConstants.DbTables.Challenge,
                AssociatedId = newTaskChallenge.ChallengeId,
                CreatedOnDateTime = DateTime.Now
            };
            await _log.AddLogAsync(challengeLog);

            var taskChallengeLog = new AuditLog()
            {
                UserId = user,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Type = MyConstants.ProcessType.AddRecord,
                Table = MyConstants.DbTables.TaskChallenge,
                AssociatedId = newTaskChallenge.TaskChallengeId,
                CreatedOnDateTime = DateTime.Now
            };
            await _log.AddLogAsync(taskChallengeLog);
            TempData[MyConstants.Messages.Success] = $"The Challenge {newChallenge.Title} was created on Task {myTask.Title} successfully!";
            return RedirectToAction("GetTaskChallenges", "TaskChallenge", new { id = newTaskChallenge.TaskId });
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskChallenges(Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var initiativeID = await _initiativeTaskStore.GetInitiativeFromTask(id);
            ViewBag.Initiative = initiativeID.InitiativeId;
            var records = await _taskChallengeStore.ReadMergeAsync(id);
            return View(records);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTaskChallenge(Guid id)
        {
            //id is TaskChallengeId
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var taskChallenge = await _taskChallengeStore.ReadAsync(id);
            if (taskChallenge != null)
            {
                var challenge = await _challengeStore.ReadAsync(taskChallenge.ChallengeId);
                if (challenge != null)
                {
                    var thisRecord = new UpdateTaskChallengeVM
                    {
                        TaskTitle = (await _task.ReadAsync(taskChallenge.TaskId)).Title,
                        TaskChallengeId = taskChallenge.TaskChallengeId,
                        ChallengeId = taskChallenge.ChallengeId,
                        TaskId = taskChallenge.TaskId,
                        Title = challenge.Title,
                        Description = challenge.Description,
                        ChallengeTypeId = challenge.ChallengeTypeId,
                        ChallengeTypes = (await _challengeTypeList.ReadAllActiveAsync()).Select(x => new SelectListItem
                        {
                            Text = x.Title,
                            Value = x.ChallengeTypeId.ToString()
                        }),
                    };
                    var user = (await _userManager.GetUserAsync(User)).Email;
                    var challengeLog = new AuditLog()
                    {
                        UserId = user,
                        IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Type = MyConstants.ProcessType.ReadRecord,
                        Table = MyConstants.DbTables.Challenge,
                        AssociatedId = taskChallenge.ChallengeId,
                        CreatedOnDateTime = DateTime.Now
                    };
                    await _log.AddLogAsync(challengeLog);

                    var taskChallengeLog = new AuditLog()
                    {
                        UserId = user,
                        IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Type = MyConstants.ProcessType.ReadRecord,
                        Table = MyConstants.DbTables.TaskChallenge,
                        AssociatedId = taskChallenge.TaskChallengeId,
                        CreatedOnDateTime = DateTime.Now
                    };
                    await _log.AddLogAsync(taskChallengeLog);
                    return View(thisRecord);
                }
            }
            return View(null);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Modifier}")]
        public async Task<IActionResult> UpdateTaskChallenge(UpdateTaskChallengeVM model)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var myTask = await _task.ReadAsync(model.TaskId);
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                model.TaskTitle = (myTask != null) ? myTask.Title : "ERROR!";
                model.ChallengeTypes = (await _challengeTypeList.ReadAllActiveAsync()).Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.ChallengeTypeId.ToString()
                });
                TempData[MyConstants.Messages.Error] = "Pay attention to the data you are entering!";
                return RedirectToAction("UpdateTaskChallenge", "TaskChallenge", new { id = model.TaskChallengeId });
            }
            var user = (await _userManager.GetUserAsync(User)).Email;

            var thisChallenge = new DbTables.Challenge
            {
                ChallengeId = model.ChallengeId,
                Title = model.Title,
                ChallengeTypeId = model.ChallengeTypeId,
                Description = model.Description,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
            };
        
            var updatedChallenge = await _challengeStore.UpdateAsync(thisChallenge);

            var thisTaskChallenge = new TaskChallenge
            {
                TaskChallengeId = model.TaskChallengeId,
                TaskId = model.TaskId,
                ChallengeId = model.ChallengeId,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
            };
            var updatedTaskChallenge = await _taskChallengeStore.UpdateAsync(thisTaskChallenge);

            if (updatedTaskChallenge != null && updatedChallenge != null)
            {
                var challengeLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.UpdateRecord,
                    Table = MyConstants.DbTables.Challenge,
                    AssociatedId = updatedChallenge.ChallengeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(challengeLog);

                var taskChallengeLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.UpdateRecord,
                    Table = MyConstants.DbTables.TaskChallenge,
                    AssociatedId = updatedTaskChallenge.TaskChallengeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(taskChallengeLog);
                TempData[MyConstants.Messages.Success] = $"The Challenge {updatedChallenge.Title} was created on Task {myTask.Title} successfully!";
                return RedirectToAction("GetTaskChallenges", "TaskChallenge", new { id = updatedTaskChallenge.TaskId });
            }
            else
            {
                TempData[MyConstants.Messages.Error] = "Error! Please check the data";
                return RedirectToAction("UpdateTaskChallenge", new { id = model.TaskChallengeId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Deleter}")]
        public async Task<IActionResult> DeleteSoft(Guid id)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var record = await _taskChallengeStore.ReadAsync(id);
            var deletedRecord = await _taskChallengeStore.DeleteSoftAsync(record.TaskChallengeId, user);
            var deletedRecord2 = await _challengeStore.DeleteSoftAsync(record.ChallengeId, user);
            if (deletedRecord != null && deletedRecord2 != null)
            {
                var log1 = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecordSoft,
                    Table = MyConstants.DbTables.TaskChallenge,
                    AssociatedId = record.TaskChallengeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log1);
                var log2 = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecordSoft,
                    Table = MyConstants.DbTables.Challenge,
                    AssociatedId = record.ChallengeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log2);

                TempData[MyConstants.Messages.Success] = $"The Task Challenge was Deleted Successfully!";
                return RedirectToAction("GetTaskChallenges", "TaskChallenge", new { id = record.TaskId });
            }
            return RedirectToAction("UpdateTaskChallenge", "TaskChallenge", new { id = record.TaskChallengeId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.SuperUser}")]
        public async Task<IActionResult> Delete(UpdateTaskChallengeVM model)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var deletedTaskChallengeGuid = await _taskChallengeStore.DeleteAsync(model.TaskChallengeId);
            var deletedChallengeGuid = await _challengeStore.DeleteAsync(model.ChallengeId);

            if (deletedTaskChallengeGuid != null && deletedChallengeGuid != null)
            {
                var taskChallengeLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecord,
                    Table = MyConstants.DbTables.TaskChallenge,
                    AssociatedId = model.TaskChallengeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(taskChallengeLog);

                var challengeLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecord,
                    Table = MyConstants.DbTables.Challenge,
                    AssociatedId = model.ChallengeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(challengeLog);

                TempData[MyConstants.Messages.Success] = "Umbunite! The Task Challenges was Deleted Successfully!";
                return RedirectToAction("GetTaskChallenges", "TaskChallenge");
            }
            return RedirectToAction("UpdateTaskChallenge", "TaskChallenge", new { id = model.TaskChallengeId });
        }
    }
}
