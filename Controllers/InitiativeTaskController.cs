using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quorom.DbTables;
using Quorom.Repositories;
using static Quorom.ViewModels.InitiativeTaskVM;

namespace Quorom.Controllers
{
    public class InitiativeTaskController : Controller
    {
        private readonly IInitiativeTaskRepository _initiativeTaskStore;
        private readonly ITaskRepository _taskStore;
        private readonly IQuoromiteRepository _quoromiteList;
        private readonly ITaskTypeRepository _taskTypeList;
        private readonly IInitiativeRepository _initiative;
        private readonly IBannerRepository _bannerList;
        private readonly UserManager<QuoromUser> _userManager;
        private readonly IAuditLogRepository _log;

        public InitiativeTaskController
            (
            IInitiativeTaskRepository initiativeTaskStore,
            ITaskRepository taskStore,
            IQuoromiteRepository quoromiteList,
            ITaskTypeRepository taskTypeList,
            IInitiativeRepository initiative,
            IBannerRepository bannerList,
            UserManager<QuoromUser> userManager,
            IAuditLogRepository log
            )
        {
            _initiativeTaskStore = initiativeTaskStore;
            _taskStore = taskStore;
            _quoromiteList = quoromiteList;
            _taskTypeList = taskTypeList;
            _initiative = initiative;
            _bannerList = bannerList;
            _userManager = userManager;
            _log = log;
        }

        [HttpGet]
        public async Task<IActionResult> AddInitiativeTask(Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var myInitiative = await _initiative.ReadAsync(id);
            var model = new AddInitiativeTaskVM
            {
                InitiativeTitle = (myInitiative != null) ? myInitiative.Title : "ERROR!",
                Quoromites = (await _quoromiteList.ReadAllActiveAsync()).Select(x => new SelectListItem
                {
                    Text = $"{x.FullName} - {x.Email}",
                    Value = x.QuoromiteId.ToString()
                }),
                TaskTypes = (await _taskTypeList.ReadAllActiveAsync()).Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.TaskTypeId.ToString()
                }),
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Contributer}")]
        public async Task<IActionResult> AddInitiativeTask(AddInitiativeTaskVM model, Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var myInitiative = await _initiative.ReadAsync(id);
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                model.InitiativeTitle = (myInitiative != null) ? myInitiative.Title : "ERROR!";
                model.Quoromites = (await _quoromiteList.ReadAllActiveAsync()).Select(x => new SelectListItem
                {
                    Text = $"{x.FullName} - {x.Email}",
                    Value = x.QuoromiteId.ToString()
                });
                model.TaskTypes = (await _taskTypeList.ReadAllActiveAsync()).Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.TaskTypeId.ToString()
                });
                TempData[MyConstants.Messages.Error] = "Pay attention to the data you are entering!";
                return View(model);
            }
            var user = (await _userManager.GetUserAsync(User)).Email;

            var thisTask = new DbTables.Task()
            {
                Title = model.Title,
                TaskTypeId = model.TaskTypeId,
                PlannedStartDateTime = model.PlannedStartDateTime,
                PlannedStopDateTime = model.PlannedStopDateTime,
                Description = model.Description,
                ActualStartDateTime = new DateTime(0001, 01, 01, 00, 00, 00),
                ActualStopDateTime = new DateTime(0001, 01, 01, 00, 00, 00),
                Status = MyConstants.Statuses.Opened,
                SubStatus = MyConstants.SubStatuses.NoActivity,
                IsCompleted = false,
                CompletedByUserId = null,
                CompletedOnDateTime = null,
                CreatedOnDateTime = DateTime.Now,
                CreatedByUserId = user,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
                IsDeleted = false,
                DeletedByUserId = null,
                DeletedOnDateTime = null,
            };
            var selectedQuoromites = new List<Quoromite>();
            foreach (var quoromiteId in model.QuoromiteEmails)
            {
                var existingRecord = await _quoromiteList.ReadAsync(Guid.Parse(quoromiteId));
                if (existingRecord != null)
                    selectedQuoromites.Add(existingRecord);
            }
            thisTask.Quoromites = selectedQuoromites;
            var newTask = await _taskStore.AddAsync(thisTask);


            var thisInitiativeTask = new InitiativeTask()
            {
                InitiativeId = id,
                TaskId = newTask.TaskId,
                CreatedOnDateTime = DateTime.Now,
                CreatedByUserId = user,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
                IsDeleted = false,
                DeletedByUserId = null,
                DeletedOnDateTime = null,
            };
            var newInitiativeTask = await _initiativeTaskStore.AddAsync(thisInitiativeTask);

            var taskLog = new AuditLog()
            {
                UserId = user,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Type = MyConstants.ProcessType.AddRecord,
                Table = MyConstants.DbTables.Task,
                AssociatedId = newInitiativeTask.TaskId,
                CreatedOnDateTime = DateTime.Now
            };
            await _log.AddLogAsync(taskLog);

            var initiativeTaskLog = new AuditLog()
            {
                UserId = user,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Type = MyConstants.ProcessType.AddRecord,
                Table = MyConstants.DbTables.InitiativeTask,
                AssociatedId = newInitiativeTask.InitiativeTaskId,
                CreatedOnDateTime = DateTime.Now
            };
            await _log.AddLogAsync(initiativeTaskLog);
            TempData[MyConstants.Messages.Success] = $"The Task {newTask.Title} was created on Initiative {myInitiative.Title} successfully!";
            return RedirectToAction("GetInitiativeTasks", "InitiativeTask", new { id = newInitiativeTask.InitiativeId });
        }
        [HttpGet]
        public async Task<IActionResult> GetInitiativeTasks(Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var records = await _initiativeTaskStore.ReadMergeAsync(id);
            return View(records);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateInitiativeTask(Guid id)
        {
            //id is InitiativeTaskId
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var initiativeTask = await _initiativeTaskStore.ReadAsync(id);
            if (initiativeTask != null)
            {
                var task = await _taskStore.ReadAsync(initiativeTask.TaskId);
                if (task != null)
                {
                    var thisRecord = new UpdateInitiativeTaskVM
                    {
                        InitiativeTitle = (await _initiative.ReadAsync(initiativeTask.InitiativeId)).Title,
                        InitiativeTaskId = initiativeTask.InitiativeTaskId,
                        TaskId = initiativeTask.TaskId,
                        InitiativeId = initiativeTask.InitiativeId,
                        Title = task.Title,
                        Description = task.Description,
                        TaskTypeId = task.TaskTypeId,
                        PlannedStartDateTime = task.PlannedStartDateTime,
                        PlannedStopDateTime = task.PlannedStopDateTime,
                        ActualStartDateTime = new DateTime(0001, 01, 01, 00, 00, 00),
                        ActualStopDateTime = new DateTime(0001, 01, 01, 00, 00, 00),
                        QuoromiteEmails = task.Quoromites.Select(q => q.QuoromiteId.ToString()).ToArray(),
                        TaskTypes = (await _taskTypeList.ReadAllActiveAsync()).Select(x => new SelectListItem
                        {
                            Text = x.Title,
                            Value = x.TaskTypeId.ToString()
                        }),
                        Quoromites = (await _quoromiteList.ReadAllActiveAsync()).Select(x => new SelectListItem
                        {
                            Text = $"{x.FullName} - {x.Email}",
                            Value = x.QuoromiteId.ToString()
                        }),
                    };
                    var user = (await _userManager.GetUserAsync(User)).Email;
                    var taskLog = new AuditLog()
                    {
                        UserId = user,
                        IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Type = MyConstants.ProcessType.ReadRecord,
                        Table = MyConstants.DbTables.Task,
                        AssociatedId = initiativeTask.TaskId,
                        CreatedOnDateTime = DateTime.Now
                    };
                    await _log.AddLogAsync(taskLog);

                    var initiativeTaskLog = new AuditLog()
                    {
                        UserId = user,
                        IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Type = MyConstants.ProcessType.ReadRecord,
                        Table = MyConstants.DbTables.InitiativeTask,
                        AssociatedId = initiativeTask.InitiativeTaskId,
                        CreatedOnDateTime = DateTime.Now
                    };
                    await _log.AddLogAsync(initiativeTaskLog);
                    return View(thisRecord);
                }
            }
            return View(null);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Modifier}")]
        public async Task<IActionResult> UpdateInitiativeTask(UpdateInitiativeTaskVM model)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            ViewBag.Route = model.InitiativeId;
            var myInitiative = await _initiative.ReadAsync(model.InitiativeId);
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                model.InitiativeTitle = (myInitiative != null) ? myInitiative.Title : "ERROR!";
                model.Quoromites = (await _quoromiteList.ReadAllActiveAsync()).Select(x => new SelectListItem
                {
                    Text = $"{x.FullName} - {x.Email}",
                    Value = x.QuoromiteId.ToString()
                });
                model.TaskTypes = (await _taskTypeList.ReadAllActiveAsync()).Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.TaskTypeId.ToString()
                });
                TempData[MyConstants.Messages.Error] = "Pay attention to the data you are entering!";
                return RedirectToAction("UpdateInitiativeTask", "InitiativeTask", new { id = model.InitiativeTaskId });
            }
            var user = (await _userManager.GetUserAsync(User)).Email;

            var thisTask = new DbTables.Task
            {
                TaskId = model.TaskId,
                Title = model.Title,
                TaskTypeId = model.TaskTypeId,
                PlannedStartDateTime = model.PlannedStartDateTime,
                PlannedStopDateTime = model.PlannedStopDateTime,
                ActualStartDateTime = model.ActualStartDateTime,
                ActualStopDateTime = model.ActualStopDateTime,
                Description = model.Description,
                IsCompleted = model.ActualStopDateTime == new DateTime(0001, 01, 01, 00, 00, 00) ? false : true,
                CompletedOnDateTime = model.ActualStopDateTime == new DateTime(0001, 01, 01, 00, 00, 00) ? null : DateTime.Now,
                CompletedByUserId = model.ActualStopDateTime == new DateTime(0001, 01, 01, 00, 00, 00) ? null : user,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
            };
            var selectedQuoromites = new List<Quoromite>();
            foreach (var selectedQuoromiteId in model.QuoromiteEmails)
            {
                if (Guid.TryParse(selectedQuoromiteId, out var quoromiteId))
                {
                    var foundQuoromite = await _quoromiteList.ReadAsync(quoromiteId);
                    if (foundQuoromite != null)
                        selectedQuoromites.Add(foundQuoromite);
                }
            }
            thisTask.Quoromites = selectedQuoromites;
            if (model.ActualStopDateTime == new DateTime(0001, 01, 01, 00, 00, 00))
            {
                if (model.ActualStartDateTime == new DateTime(0001, 01, 01, 00, 00, 00))
                {
                    thisTask.Status = MyConstants.Statuses.Opened;
                    thisTask.SubStatus = MyConstants.SubStatuses.NoActivity;
                }
                else if (model.ActualStartDateTime != new DateTime(0001, 01, 01, 00, 00, 00) && model.ActualStartDateTime < model.PlannedStartDateTime)
                {
                    thisTask.Status = MyConstants.Statuses.InProgress;
                    thisTask.SubStatus = MyConstants.SubStatuses.OnTime;
                }
                else if (model.ActualStartDateTime != new DateTime(0001, 01, 01, 00, 00, 00) && model.ActualStartDateTime > model.PlannedStopDateTime)
                {
                    thisTask.Status = MyConstants.Statuses.InProgress;
                    thisTask.SubStatus = MyConstants.SubStatuses.LateStart;
                }
                else
                {
                    thisTask.Status = MyConstants.Statuses.InProgress;
                    thisTask.SubStatus = MyConstants.SubStatuses.Overdue;
                }
            }
            else if (model.ActualStopDateTime != new DateTime(0001, 01, 01, 00, 00, 00))
            {
                if (model.ActualStopDateTime <= model.PlannedStopDateTime)
                {
                    thisTask.Status = MyConstants.Statuses.Completed;
                    thisTask.SubStatus = MyConstants.SubStatuses.OnTime;
                }
                else if (model.ActualStopDateTime > model.PlannedStopDateTime)
                {
                    thisTask.Status = MyConstants.Statuses.Completed;
                    thisTask.SubStatus = MyConstants.SubStatuses.Overdue;
                }
            }
            else
            {
                thisTask.Status = MyConstants.Statuses.Opened;
                thisTask.SubStatus = MyConstants.SubStatuses.NoActivity;
            }

            var updatedTask = await _taskStore.UpdateAsync(thisTask);

            var thisInitiativeTask = new InitiativeTask
            {
                InitiativeTaskId = model.InitiativeTaskId,
                InitiativeId = model.InitiativeId,
                TaskId = model.TaskId,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
            };
            var updatedInitiativeTask = await _initiativeTaskStore.UpdateAsync(thisInitiativeTask);

            if (updatedInitiativeTask != null && updatedTask != null)
            {
                var taskLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.UpdateRecord,
                    Table = MyConstants.DbTables.Task,
                    AssociatedId = updatedTask.TaskId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(taskLog);

                var initiativeTaskLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.UpdateRecord,
                    Table = MyConstants.DbTables.InitiativeTask,
                    AssociatedId = updatedInitiativeTask.InitiativeTaskId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(initiativeTaskLog);
                TempData[MyConstants.Messages.Success] = $"The Task {updatedTask.Title} was created on Initiative {myInitiative.Title} successfully!";
                return RedirectToAction("GetInitiativeTasks", "InitiativeTask", new { id = updatedInitiativeTask.InitiativeId });
            }
            else
            {
                TempData[MyConstants.Messages.Error] = "Error! Please check the data";
                return RedirectToAction("UpdateInitiativeTask", new { id = model.InitiativeTaskId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Deleter}")]
        public async Task<IActionResult> DeleteSoft(Guid id)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var record = await _initiativeTaskStore.ReadAsync(id);
            var deletedRecord = await _initiativeTaskStore.DeleteSoftAsync(record.InitiativeTaskId, user);
            var deletedRecord2 = await _taskStore.DeleteSoftAsync(record.TaskId, user);
            if (deletedRecord != null && deletedRecord2 != null)
            {
                var log1 = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecordSoft,
                    Table = MyConstants.DbTables.InitiativeTask,
                    AssociatedId = record.InitiativeTaskId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log1);
                var log2 = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecordSoft,
                    Table = MyConstants.DbTables.Task,
                    AssociatedId = record.TaskId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log2);

                TempData[MyConstants.Messages.Success] = $"The Initiative Task was Deleted Successfully!";
                return RedirectToAction("GetInitiativeTasks", "InitiativeTask", new { id = record.InitiativeId });
            }
            return RedirectToAction("UpdateInitiativeTask", "InitiativeTask", new { id = record.InitiativeTaskId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.SuperUser}")]
        public async Task<IActionResult> Delete(UpdateInitiativeTaskVM model)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var deletedInitiativeTaskGuid = await _initiativeTaskStore.DeleteAsync(model.InitiativeTaskId);
            var deletedTaskGuid = await _taskStore.DeleteAsync(model.TaskId);

            if (deletedInitiativeTaskGuid != null && deletedTaskGuid != null)
            {
                var initiativeTaskLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecord,
                    Table = MyConstants.DbTables.InitiativeTask,
                    AssociatedId = model.InitiativeTaskId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(initiativeTaskLog);

                var taskLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecord,
                    Table = MyConstants.DbTables.Task,
                    AssociatedId = model.TaskId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(taskLog);

                TempData[MyConstants.Messages.Success] = "Umbunite! The Initiative Tasks was Deleted Successfully!";
                return RedirectToAction("GetInitiativeTasks", "InitiativeTask");
            }
            return RedirectToAction("UpdateInitiativeTask", "InitiativeTask", new { id = model.InitiativeTaskId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateActualStartDateTime(Guid id, DateTime actualStartDateTime)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var initiativeTask = await _initiativeTaskStore.ReadAsync(id);

            if (initiativeTask != null)
            {
                var task = await _taskStore.ReadAsync(initiativeTask.TaskId);
                if (task != null)
                {
                    task.ActualStartDateTime = actualStartDateTime;
                    task.UpdatedOnDateTime = DateTime.Now;
                    task.UpdatedByUserId = user;

                    // Update status based on new start time
                    if (task.ActualStartDateTime < task.PlannedStartDateTime)
                    {
                        task.Status = MyConstants.Statuses.InProgress;
                        task.SubStatus = MyConstants.SubStatuses.EarlyStart;
                    }
                    else if (task.ActualStartDateTime > task.PlannedStartDateTime)
                    {
                        task.Status = MyConstants.Statuses.InProgress;
                        task.SubStatus = MyConstants.SubStatuses.LateStart;
                    }
                    else
                    {
                        task.Status = MyConstants.Statuses.InProgress;
                        task.SubStatus = MyConstants.SubStatuses.OnTime;
                    }

                    await _taskStore.UpdateAsync(task);

                    // Log the update
                    var log = new AuditLog()
                    {
                        UserId = user,
                        IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Type = MyConstants.ProcessType.UpdateRecord,
                        Table = MyConstants.DbTables.Task,
                        AssociatedId = task.TaskId,
                        CreatedOnDateTime = DateTime.Now
                    };
                    await _log.AddLogAsync(log);

                    TempData[MyConstants.Messages.Success] = "Start time updated successfully!";
                    return RedirectToAction("GetInitiativeTasks", new { id = initiativeTask.InitiativeId });
                }
            }

            TempData[MyConstants.Messages.Error] = "Error updating start time!";
            return RedirectToAction("GetInitiativeTasks", new { id = initiativeTask.InitiativeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateActualStopDateTime(Guid id, DateTime actualStopDateTime)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var initiativeTask = await _initiativeTaskStore.ReadAsync(id);

            if (initiativeTask != null)
            {
                var task = await _taskStore.ReadAsync(initiativeTask.TaskId);
                if (task != null && task.ActualStartDateTime != new DateTime(0001, 01, 01, 00, 00, 00))
                {
                    task.ActualStopDateTime = actualStopDateTime;
                    task.IsCompleted = true;
                    task.CompletedOnDateTime = DateTime.Now;
                    task.CompletedByUserId = user;
                    task.UpdatedOnDateTime = DateTime.Now;
                    task.UpdatedByUserId = user;

                    // Update status based on completion time
                    if (task.ActualStopDateTime <= task.PlannedStopDateTime)
                    {
                        task.Status = MyConstants.Statuses.Completed;
                        task.SubStatus = MyConstants.SubStatuses.OnTime;
                    }
                    else
                    {
                        task.Status = MyConstants.Statuses.Completed;
                        task.SubStatus = MyConstants.SubStatuses.Overdue;
                    }

                    await _taskStore.UpdateAsync(task);

                    // Log the update
                    var log = new AuditLog()
                    {
                        UserId = user,
                        IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Type = MyConstants.ProcessType.UpdateRecord,
                        Table = MyConstants.DbTables.Task,
                        AssociatedId = task.TaskId,
                        CreatedOnDateTime = DateTime.Now
                    };
                    await _log.AddLogAsync(log);

                    TempData[MyConstants.Messages.Success] = "Stop time updated successfully!";
                    return RedirectToAction("GetInitiativeTasks", new { id = initiativeTask.InitiativeId });
                }
            }

            TempData[MyConstants.Messages.Error] = "Error updating stop time! Task must be started first.";
            return RedirectToAction("GetInitiativeTasks", new { id = initiativeTask.InitiativeId });
        }
    }
}
