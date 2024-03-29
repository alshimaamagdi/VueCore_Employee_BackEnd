﻿using RepositoryPatternWithUoW.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUoW.Core.Models.Dto.Employee
{
    public class EmployeeAddModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string phoneNumber { get; set; }
        public AcademicLevel AcademicLevel { get; set; }
        public string? image { get; set; }
        public string? createdBy { get; set; }

    }
}
