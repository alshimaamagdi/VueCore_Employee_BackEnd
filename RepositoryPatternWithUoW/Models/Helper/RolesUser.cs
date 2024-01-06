using Microsoft.AspNetCore.Http;
using RepositoryPatternWithUoW.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUoW.Core.Models.Helper
{
    public  class RolesUser
    {
        private static IHttpContextAccessor _httpContextAccessor;

        // To Make Lock To sure No More Instance done on that class
        private static object _LockObject = new object();
        //Decleration For Instance Use
        private static RolesUser _instance;
        //To Prevent Instanse From This Class
        private RolesUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public static RolesUser GetInstance()
        {
            //the double-checked locking 
            if (_instance == null)
            {
                lock (_LockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new RolesUser(_httpContextAccessor);
                    }
                }
            }
            return _instance;

        }
  
        //That is Simple Function To Switch Between User And Admin only
        public  bool IsUserAdmin()
        {
            if (_httpContextAccessor == null)
            {
                throw new InvalidOperationException("UserContextHelper has not been initialized. Call Initialize method during application startup.");
            }

            //var isAdminHeader = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("IsAdmin", out var isAdminValue) && isAdminValue == "true";
            var isInAdminRole = _httpContextAccessor.HttpContext.User.IsInRole("Admin");

            return  isInAdminRole;
        }
    }

}
