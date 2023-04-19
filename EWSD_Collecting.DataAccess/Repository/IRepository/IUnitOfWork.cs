using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSD_Collecting.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IApplicationUserRepository ApplicationUser { get; }
        ICategoryRepository Category { get; }
        IDepartmentRepository Department { get; }
        ITopicRepository Topic { get; }
        IIdeaRepository Idea { get; }
        IViewRepository View { get; }
        ICommentRepository Comment { get; }
        void Save();
    }
}
