using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RepositoryPatternWithUoW.Core.Interface;
using RepositoryPatternWithUoW.Core.IUnitOfWork;
using RepositoryPatternWithUoW.Core.Models.Domain;
using RepositoryPatternWithUoW.Core.Models.Helper;
using RepositoryPatternWithUoW.EF.Context;
using RepositoryPatternWithUoW.EF.Migrations;
using RepositoryPatternWithUoW.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUoW.EF.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
      
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly UserManager<Users> _userManager;
            private readonly IOptions<JWT> _jwt;

        public IAuthorization Authorization { get; private set; }
            public IEmployee Employee { get; private set; }

            public UnitOfWork(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<Users> userManager,IOptions<JWT> jwt)
            {
                _context = context;
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
                _userManager = userManager;
                 _jwt = jwt;
                Authorization = new Authorization(_userManager, _jwt);
                Employee = new Employee(_context, _mapper, _httpContextAccessor);
            }
        


        public void Dispose()
        {

        }
    }
}
