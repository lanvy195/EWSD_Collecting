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
    public class TopicRepository: Repository<Topic>, ITopicRepository
    {
        private readonly ApplicationDbContext _db;
        public TopicRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Topic topic)
        {
            _db.Topics.Update(topic);
        }
    }
}
