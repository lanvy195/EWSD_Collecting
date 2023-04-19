﻿using EWSD_Collecting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSD_Collecting.DataAccess.Repository.IRepository
{
    public interface IDepartmentRepository:IRepository<Department>
    {
        void Update(Department department);
    }
}
