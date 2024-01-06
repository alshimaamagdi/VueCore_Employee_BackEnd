﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUoW.Core.IUnitOfWork;
using RepositoryPatternWithUoW.Core.Models.Dto.Employee;
using System.Drawing.Printing;

namespace VueCore_Employee_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("LoadAllEmployees")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LoadAllEmployees(int pageNumber, int pageSize)
        {
            var UserId = User.Identity.Name;
            var GetAllEmployees = await _unitOfWork.Employee.GetAllEmployees(pageNumber, pageSize, UserId);
            return Ok(GetAllEmployees);
        }

        [HttpGet("GetEmployeeById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var GetEmployeeById = await _unitOfWork.Employee.GetEmployeeById(id);
            return Ok(GetEmployeeById);
        }

        [HttpPost("AddEmployee")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> AddEmployee(EmployeeAddModel model)
        {
            var UserId = User.Identity.Name;
            model.createdBy = UserId;

            // Check if user is authorized
            if (!User.IsInRole("User"))
            {
                return Forbid(); // 403 Forbidden
            }

            var AddEmployee = await _unitOfWork.Employee.AddEmployee(model);
            return Ok(AddEmployee);
        }

        [HttpPut("EditEmployee")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> EditEmployee(EmployeeEditModel model)
        {
            var UserId = User.Identity.Name;
            model.modifiedBy = UserId;

            // Check if user is authorized
            if (!User.IsInRole("Admin"))
            {
                return Forbid(); 
            }

            var EditEmployee = await _unitOfWork.Employee.EditEmployee(model);
            return Ok(EditEmployee);
        }

        [HttpDelete("DeleteEmployee")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(List<int> ids)
        {
            var UserId = User.Identity.Name;

            // Check if user is authorized
            if (!User.IsInRole("Admin"))
            {
                return Forbid(); 
            }

            await _unitOfWork.Employee.DeleteEmployee(ids, UserId);
            return Ok();
        }

    }
}