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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public void Save()
        {
            _db.SaveChanges();
        }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            Category = new CategoryRepository(_db);
            Department = new DepartmentRepository(_db);
            Topic = new TopicRepository(_db);
            Idea = new IdeaRepository(_db);
            View =new ViewRepository(_db);
            Comment = new CommentRepository(_db);
        }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public ITopicRepository Topic { get; private set;}
        public IIdeaRepository Idea { get; private set; }
        public IViewRepository View { get; private set; }
        public ICommentRepository Comment { get; private set; }
    }
}
