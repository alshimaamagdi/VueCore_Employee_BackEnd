using RepositoryPatternWithUoW.Core.Models.Dto.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUoW.Core.Interface
{
    public interface IAuthorization
    {
       Task<AuthorizeReturnModel> LoginAsync(LoginModel model);
       Task<AuthorizeReturnModel> RegisterAsync(RegisterModel model);
    }
}
