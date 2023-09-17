using SBTaskManagement.Application.Helpers;
using SBTaskManagement.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Infrastructure.ServiceImplementation
{
    public class PersistenceAuditService : IPersistenceAudit
    {
        public PersistenceAuditService()
        {
            _createdBy = WebHelper.UserId;
        }

        private Guid _createdBy;
        public Guid? GetCreatedById
        {
            get
            {
                return _createdBy;
            }
        }
    }
}
