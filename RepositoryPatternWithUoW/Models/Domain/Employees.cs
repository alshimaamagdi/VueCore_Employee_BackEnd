using RepositoryPatternWithUoW.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUoW.Core.Models.Domain
{
    public class Employees: BaseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string phoneNumber { get; set; }
        public AcademicLevel AcademicLevel { get; set; }
        public string? image { get; set; }

    

    }
}
