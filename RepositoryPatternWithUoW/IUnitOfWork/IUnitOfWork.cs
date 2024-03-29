﻿using RepositoryPatternWithUoW.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUoW.Core.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthorization Authorization { get; }
        IEmployee Employee { get; }
    }
}
