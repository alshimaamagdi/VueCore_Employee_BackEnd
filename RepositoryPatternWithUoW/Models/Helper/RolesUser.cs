using Microsoft.AspNetCore.Http;
using RepositoryPatternWithUoW.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUoW.Core.Models.Helper
{
    public static class RolesUser
    {
        private static IHttpContextAccessor _httpContextAccessor;

        // Call this method during application startup to set the IHttpContextAccessor
        public static void Initialize(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        //That is Simple Function To Switch Between User And Admin only
        public static bool IsUserAdmin()
        {
            if (_httpContextAccessor == null)
            {
                throw new InvalidOperationException("UserContextHelper has not been initialized. Call Initialize method during application startup.");
            }

            var isAdminHeader = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("IsAdmin", out var isAdminValue) && isAdminValue == "true";
            var isInAdminRole = _httpContextAccessor.HttpContext.User.IsInRole("Admin");

            return isAdminHeader || isInAdminRole;
        }
    }

}
