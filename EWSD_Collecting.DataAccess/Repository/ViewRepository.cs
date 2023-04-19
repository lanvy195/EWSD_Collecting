using EWSD_Collecting.DataAccess.Data;
using EWSD_Collecting.DataAccess.Repository.IRepository;
using EWSD_Collecting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSD_Collecting.DataAccess.Repository
{
    public class ViewRepository : Repository<View>, IViewRepository
    {
        private readonly ApplicationDbContext _db;
        public ViewRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(View view)
        {
            _db.Views.Update(view);
        }
    }
}
