using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Domain.Common
{
    public static class ResponseMessages
    {
        public const string TaskCreationResponse = "Task Created Successfully";
        public const string NotificationCreationResponse = "Notification Created Successfully";
        public const string PayloadCannotBeNull = "Payload cannot be null";
        public const string PasswordCannotBeNull = "password cannot be null";
        public const string PasswordIncorrect = "password incorrect";
        public const string PasswordDoNotMatch = "passwords do not match";
        public const string PasswordUpdate = "your password has been updated successfully";
        public const string IncorrectOldPassword = "old password incorrect";
        public const string UsernameOrPasswordIncorrect = "incorrect username or password";
        public const string RequireUsernameandPassword = "Username and password is required";
        public const string UsernameExist = "Registration failed; An accout with this username already exist. Please Log-in or try another email";
        public const string Successful = "Successful";
        public const string Invalid = "Invalid id provided";
        public const string UnableToRetrieveData = "Unable to retrieve data";
        public const string ErrorWhileProcessing = "An error occured while processing your request";
        public const string TaskAssignmentSuccessful = "Task was successfully assigned to the project";
        public const string TaskRemovalSuccessful = "Task was successfully removed from project";
        public const string ReadNotification = "Notification marked read";
        public const string UnReadNotification = "Notification marked unread";
        public const string UserNotFound = "User not found";
        public const string EmailNotificationFailed = "An error occored while sending a mail";
        public const string InvalidToken = "Invalid token";
        public const string MissingClaim = "Missing claim";
        public const string DuplicateEmails = "Duplicate email addresses";
        public const string EmailExist = "Email already exist";
        public const string RegistrationSuccessful = "Registration Successful";
        public const string AllowedTaskStatus = "Error while processing, only; pending, completed and inProgress values are allowed";
        public const string AllowedTaskPriority = "Error while processing, only; low, medium and high values are allowed";
       
    }
}
