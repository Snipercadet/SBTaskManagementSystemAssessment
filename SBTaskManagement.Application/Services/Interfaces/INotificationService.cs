using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<SuccessResponse<IEnumerable<NotificationDto>>> GetAllNotifications();
        Task<SuccessResponse<NotificationDto>> GetByIdAsync(Guid id);
        Task<SuccessResponse<NotificationDto>> CreateAsync(CreateNotificationDto model);
        Task<SuccessResponse<NotificationDto>> UpdateAsync(Guid id, UpdateNotificationDto model);
        Task<SuccessResponse<bool>> DeleteAsync(Guid id);
        Task<SuccessResponse<bool>> MarkNotification(Guid notificationId, bool isRead);
    }
}
