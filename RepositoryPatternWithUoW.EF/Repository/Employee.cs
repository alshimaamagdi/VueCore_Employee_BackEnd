using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using RepositoryPatternWithUoW.Core.Interface;
using RepositoryPatternWithUoW.Core.Models.AutoMapper;
using RepositoryPatternWithUoW.Core.Models.Domain;
using RepositoryPatternWithUoW.Core.Models.Dto.Employee;
using RepositoryPatternWithUoW.Core.Models.Enum;
using RepositoryPatternWithUoW.Core.Models.Helper;
using RepositoryPatternWithUoW.EF.Context;
using RepositoryPatternWithUoW.EF.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RepositoryPatternWithUoW.EF.Repository
{
    public class Employee : Generic<Employees>, IEmployee
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly HttpContext _HttpContext;

  
        public Employee(ApplicationDbContext context,  IMapper mapper, HttpContext HttpContext) : base(context)
        {
            _mapper = mapper;
            _HttpContext = HttpContext;
        }

        public async Task<EmployeeAddEditModel> AddEmployee(EmployeeAddModel model)
        {
            #region newEmployee
            var newEmployee = new Employees();
            newEmployee.Name = model.Email;
            newEmployee.phoneNumber = model.phoneNumber;
            newEmployee.AcademicLevel = model.AcademicLevel;
            newEmployee.CreatedBy = model.createdBy;
            #endregion

            #region Photo
            var file = _HttpContext.Request.Form.Files.GetFile("ImageFile");
            if (file != null)
            {
                model.image = await MediaControl.Upload(FilePath.EmployeeImage, file);
            }
            await SaveChanges();

            #endregion

            #region Mapper
            EmployeeAddEditModel result = _mapper.Map<EmployeeAddEditModel>(newEmployee);
          
            return result;
            #endregion

        }

        public async Task DeleteEmployee(List<int> ids,string accountId)
        {
            foreach (var id in ids)
            {
                Employees GetEmployee =await _context.Employees.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (GetEmployee != null)
                {
                    GetEmployee.IsDeleted = true;
                    GetEmployee.DeletedBy = accountId;
                    GetEmployee.DeletedOn = DateTime.Now;
                }
                await Update(GetEmployee);
            }
        }

        public async Task<EmployeeAddEditModel> EditEmployee(EmployeeEditModel model)
        {
            Employees GetEmployee = await _context.Employees.Where(a => a.Id == model.Id).FirstOrDefaultAsync();
            if(GetEmployee != null)
            {
                #region UpdateExistEmployee
                GetEmployee.Name = model.Email;
                GetEmployee.phoneNumber = model.phoneNumber;
                GetEmployee.AcademicLevel = model.AcademicLevel;
                GetEmployee.ModifiedOn = DateTime.Now;
                GetEmployee.IsModified = true;
                GetEmployee.ModifiedBy = model.modifiedBy;
                #endregion

                #region Photo
                var file = _HttpContext.Request.Form.Files.GetFile("ImageFile");
                if (file != null)
                {
                    model.image = await MediaControl.Upload(FilePath.EmployeeImage, file);
                }
                await Update(GetEmployee);

                #endregion
            }

            #region Mapper
            EmployeeAddEditModel result = _mapper.Map<EmployeeAddEditModel>(GetEmployee);
            return result;
            #endregion
        }

        public async Task<List<EmployeeGetModel>> GetAllEmployees(int pageNumber, int pageSize, string accountId)
        {
            var isAdmin = RolesUser.IsUserAdmin();
            IQueryable<Employees> query;
            #region Admin
            if (isAdmin)
            {
                // If admin, materialize the query asynchronously and then use AutoMapper to map to EmployeeGetModel
                query= _context.Employees.AsQueryable().Where(a => a.IsDeleted == null)
                                         .OrderByDescending(a => a.Id)
                                         .Skip(pageNumber * pageSize)
                                         .Take(pageSize);
                List<Employees> employees = await query.ToListAsync();
                List<EmployeeGetModel> result = _mapper.Map<List<EmployeeGetModel>>(employees);
                return result;
            }
            #endregion

            #region User
            else
            {
                // If not admin, directly project to EmployeeGetModel without materializing the query
                query = _context.Employees.AsQueryable().Where(a => a.IsDeleted == null && a.CreatedBy == accountId)
                                         .OrderByDescending(a => a.Id)
                                         .Skip(pageNumber * pageSize)
                                         .Take(pageSize);
                List<EmployeeGetModel> result = await _mapper.ProjectTo<EmployeeGetModel>(query).ToListAsync();
                return result;
            }
            #endregion
        }
        public async Task<EmployeeGetModel> GetEmployeeById(int id)
        {
            Employees query =await _context.Employees.AsQueryable().Where(a => a.Id == id).FirstOrDefaultAsync();
            EmployeeGetModel result = _mapper.Map<EmployeeGetModel>(query);
            return result;
        }
    }
}
