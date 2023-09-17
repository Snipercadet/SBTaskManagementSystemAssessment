using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SBTaskManagement.Application.DTOs;
using SBTaskManagement.Application.Exceptions;
using SBTaskManagement.Application.Helpers;
using SBTaskManagement.Domain.Common;
using SBTaskManagement.Domain.Entities;
using SBTaskManagement.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SBTaskManagement.Application.Services.Interfaces;

namespace SBTaskManagement.Infrastructure.ServicesImplementation
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> _notificationRepo;
        private readonly IMapper _mapper;
        public NotificationService(IMapper mapper, IRepository<Notification> notificationRepo)
        {
            _mapper = mapper;
            _notificationRepo = notificationRepo;
        }

        public async Task<SuccessResponse<NotificationDto>> CreateAsync(CreateNotificationDto model)
        {
            if (model == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.PayloadCannotBeNull);
            }

            var notification = _mapper.Map<Notification>(model);
            await _notificationRepo.AddAsync(notification);
            await _notificationRepo.SaveChangesAsync();

            var response = _mapper.Map<NotificationDto>(notification);

            return new SuccessResponse<NotificationDto>
            {
                Data = response,
                Message = ResponseMessages.NotificationCreationResponse
            };
        }

        public async Task<SuccessResponse<bool>> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.Invalid);

            var notification = await _notificationRepo.GetByIdAsync(id)
                ?? throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.UnableToRetrieveData);

            _notificationRepo.Remove(notification);
            await _notificationRepo.SaveChangesAsync();

            return new SuccessResponse<bool>
            {
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<IEnumerable<NotificationDto>>> GetAllNotifications()
        {

            var notification = await _notificationRepo.GetAll()
                ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            var notificationDto = _mapper.Map<IEnumerable<NotificationDto>>(notification);
            return new SuccessResponse<IEnumerable<NotificationDto>>
            {
                Data = notificationDto,
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<NotificationDto>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.Invalid);
            var notification = await _notificationRepo.GetByIdAsync(id)
                ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            var notificationDto = _mapper.Map<NotificationDto>(notification);
            return new SuccessResponse<NotificationDto>
            {
                Data = notificationDto,
                Message = ResponseMessages.Successful
            };
        }

        public async Task<SuccessResponse<NotificationDto>> UpdateAsync(Guid id, UpdateNotificationDto model)
        {
            if (id == Guid.Empty || model == null)
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.PayloadCannotBeNull);

            var notification = await _notificationRepo.GetByIdAsync(id)
               ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);

            notification.Type = model.Type;
            notification.Message = model.Message;

            _notificationRepo.Update(notification);
            await _notificationRepo.SaveChangesAsync();

            var response = _mapper.Map<NotificationDto>(notification);
            return new SuccessResponse<NotificationDto>
            {
                Data = response,
                Message = ResponseMessages.Successful
            };
        }


        public async Task<SuccessResponse<bool>> MarkNotification(Guid notificationId, bool isRead)
        {
            var response = new SuccessResponse<bool>();
            if (notificationId == Guid.Empty)
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.Invalid);

            var notification = await _notificationRepo.FirstOrDefault(x => x.Id == notificationId)
                ?? throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UnableToRetrieveData);
            

            if (notification.IsRead == true)
            {
                notification.IsRead = false;
                response.Message = ResponseMessages.UnReadNotification;
            }
            else
            {
                notification.IsRead = true;
                response.Message = ResponseMessages.ReadNotification;
            }
            await _notificationRepo.SaveChangesAsync();
            return response;
        }
    }
}
