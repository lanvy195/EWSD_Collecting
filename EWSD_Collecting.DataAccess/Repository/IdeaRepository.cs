using EWSD_Collecting.DataAccess.Data;
using EWSD_Collecting.DataAccess.Repository.IRepository;
using EWSD_Collecting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSD_Collecting.DataAccess.Repository
{
    public class IdeaRepository : Repository<Idea>, IIdeaRepository
    {
        private readonly ApplicationDbContext _db;
        public IdeaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Idea idea)
        {
            _db.Ideas.Update(idea);
        }
    }
}
