using EWSD_Collecting.DataAccess.Data;
using EWSD_Collecting.DataAccess.Repository.IRepository;
using EWSD_Collecting.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSD_Collecting.DataAccess.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _db;
        public DepartmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Department department)
        {
            _db.Departments.Update(department);
        }
    }
}
