﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Infrastructure
{
    public interface IDatabaseConnectionSettings
    {
        string ConnectionString { get; }
    }
}
