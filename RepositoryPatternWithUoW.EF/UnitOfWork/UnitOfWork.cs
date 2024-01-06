using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUoW.Core.Interface;
using RepositoryPatternWithUoW.Core.IUnitOfWork;
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
        public IAuthorization authorization { get; private set; }
        public IEmployee Employee { get; private set; }
        private readonly IMapper _mapper;
        private readonly HttpContext _HttpContext;
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context, HttpContext HttpContext, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _HttpContext = HttpContext;
            authorization = new Authorization();
            Employee=new Employee(_context,_mapper, _HttpContext);
        }

        public void Dispose()
        {

        }
    }
}
