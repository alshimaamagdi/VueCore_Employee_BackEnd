using RepositoryPatternWithUoW.Core.Models.Domain;
using RepositoryPatternWithUoW.Core.Models.Dto.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RepositoryPatternWithUoW.Core.Interface.IGeneric;

namespace RepositoryPatternWithUoW.Core.Interface
{
    public interface IEmployee: IGeneric<Employees>
    {
        Task<List<EmployeeGetModel>> GetAllEmployees(int pageNumber,int pageSize,string accountId);
        Task<EmployeeGetModel> GetEmployeeById(int id);
        Task<EmployeeAddEditModel> AddEmployee(EmployeeAddModel model);
        Task<EmployeeAddEditModel> EditEmployee(EmployeeEditModel model);
        Task DeleteEmployee(List<int> ids, string accountId);

    }
}
