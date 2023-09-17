using Microsoft.EntityFrameworkCore;
using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Application.Exceptions;
using SBTaskManagement.Application.Services.Interfaces;
using SBTaskManagement.Domain.Common;
using SBTaskManagement.Domain.Entities;
using SBTaskManagement.Domain.Enums;
using SBTaskManagement.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TaskProcessingWorker.Services
{
    public interface ITaskProcessingCron
    {
        System.Threading.Tasks.Task ProcessTask();
    }
    public class TaskProcessingCron : ITaskProcessingCron
    {
        private readonly ITaskRepository _taskRepo;
        //private readonly ITaskRepository _taskRepo;
        private readonly INotificationService _notificationService;
        private readonly IRepository<Notification> _notificationRepo;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailService _emailService;
        public TaskProcessingCron(ITaskRepository taskRepo, INotificationService notificationService, IEmailTemplateService emailTemplateService, IEmailService emailService, IRepository<Notification> notificationRepo)
        {
            _taskRepo = taskRepo;
            _notificationService = notificationService;
            _emailTemplateService = emailTemplateService;
            _emailService = emailService;
            _notificationRepo = notificationRepo;
        }

        public async System.Threading.Tasks.Task ProcessTask()
        {
            await CheckDueDateNotification();
            await CheckCompletionNotification();
            await CheckNewTaskAssignmentNotification();
        }

        private async System.Threading.Tasks.Task CheckDueDateNotification()
        {

            var tasksDueIn48Hours = _taskRepo.Query(x => x.DueDate >= DateTime.UtcNow && x.DueDate <= DateTime.UtcNow && x.Notifications.Any(u => u.IsNotificationSent == false));

            foreach (var task in tasksDueIn48Hours)
            {
                var message = _emailTemplateService.GetDueDateNotificationTemplate(task.User.Name, task.Title, task.Description);
                // create a new notification
                var newNotification = new CreateNotificationDto
                {
                    Message = message,
                    Type = ENotificationType.StatusUpdate,
                    UserId = task.UserId ?? Guid.Empty,
                    TaskId = task.Id
                };

                try
                {
                    // create notification 
                    var createNotification = await _notificationService.CreateAsync(newNotification);

                    if (createNotification.Success == true)
                    {
                        var getNotification = await _notificationRepo.FirstOrDefault(x => x.Id == createNotification.Data.Id);

                        //send notification and update the isNotificationSent Property
                        var isMailSent = await _emailService.SendMail(task.User.Email, message, "Due Date Reminder");
                        if (isMailSent)
                        {
                            getNotification.IsNotificationSent = true;
                        }
                        else
                        {
                            throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.EmailNotificationFailed);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                    var notification = await _notificationRepo.FirstOrDefault(x => x.UserId == task.UserId);
                    notification.IsNotificationSent = false;
                    notification.Errors = ex.Message.ToString();
                }
                finally
                {
                    await _notificationRepo.SaveChangesAsync();
                }
            }
        }

        private async System.Threading.Tasks.Task CheckCompletionNotification()
        {
            

            var tasksMarkedAsCompleted = await _taskRepo.GetTaskNotificationCompleted();

            foreach (var task in tasksMarkedAsCompleted)
            {
                var message = _emailTemplateService.GetTaskCompletionNotificationTemplate(task.User.Name, task.Title, task.Description);

                // create a new notification
                var newNotification = new CreateNotificationDto
                {
                    Message = message,
                    Type = ENotificationType.StatusUpdate,
                    UserId = task.UserId,
                    TaskId = task.Id
                };
                try
                {
                    // save notification to the database
                    var createNotification = await _notificationService.CreateAsync(newNotification);

                    if (createNotification.Success == true)
                    {
                        var getNotification = await _notificationRepo.FirstOrDefault(x => x.Id == createNotification.Data.Id);

                        //send notification and update the isNotificationSent Property
                        var isMailSent = await _emailService.SendMail(task.User.Email, message, "Task Completion Notification");
                        if (isMailSent)
                        {
                            getNotification.IsNotificationSent = true;
                            var gettask = await _taskRepo.FirstOrDefault(x => x.Id == getNotification.TaskId);
                            gettask.isNotificationSent = true;
                        }
                        else
                        {
                            throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.EmailNotificationFailed);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                    var notification = await _notificationRepo.FirstOrDefault(x => x.UserId == task.UserId);
                    notification.IsNotificationSent = false;
                    notification.Errors = ex.Message.ToString();
                }
                finally
                {
                    await _notificationRepo.SaveChangesAsync();
                    await _taskRepo.SaveChangesAsync();
                }
            }
        }

        private async System.Threading.Tasks.Task CheckNewTaskAssignmentNotification()
        {

            var newTaskAssignmentNotification = await _taskRepo.GetTaskNotificationForNewAssignment();

            foreach (var taskNotification in newTaskAssignmentNotification)
            {
                var message = taskNotification.Message;
                try
                {
                    var isMailSent = await _emailService.SendMail(taskNotification.User.Email, message, "Due Date Reminder");
                    if (isMailSent)
                    {
                        var userId = taskNotification.UserId;
                        var notification = await _notificationRepo.FirstOrDefault(x => x.UserId == userId);
                        notification.IsNotificationSent = true;
                    }
                    else
                    {
                        throw new RestException(System.Net.HttpStatusCode.BadRequest, ResponseMessages.EmailNotificationFailed);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                    var notification = await _notificationRepo.FirstOrDefault(x => x.UserId == taskNotification.UserId);
                    notification.IsNotificationSent = false;
                    notification.Errors = ex.Message.ToString();
                }
                finally
                {
                    await _notificationRepo.SaveChangesAsync();
                }

            }
        }

    }
}
