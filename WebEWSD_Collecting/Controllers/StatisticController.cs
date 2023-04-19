using EWSD_Collecting.DataAccess.Repository.IRepository;
using EWSD_Collecting.Models.ViewModel;
using EWSD_Collecting.Utility;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebEWSD_Collecting.Controllers
{
    [Authorize(Roles = SD.Role_User_QACoordinator + "," + SD.Role_User_QAManager + "," + SD.Role_User_Administrator)]
    public class StatisticController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public StatisticController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            StatisticVM obj = new StatisticVM();
            obj.Total_Ideas_without_comment = _unitOfWork.Idea.GetAll(u => u.Comments == 0).Count();
            obj.List_Ideas_without_comment = _unitOfWork.Idea.GetAll(u => u.Comments == 0, includeProperties: "ApplicationUser").OrderByDescending(x => x.CreateDatetime);
            obj.Total_Ideas_ANONYMOUS = _unitOfWork.Idea.GetAll(u => u.isDisplay == false).Count();
            obj.List_Ideas_ANONYMOUS = _unitOfWork.Idea.GetAll(u=>u.isDisplay == false).OrderByDescending(x => x.CreateDatetime);
            obj.Total_COMMENT_ANONYMOUS = _unitOfWork.Comment.GetAll(u => u.isDisplay == false).Count();
            obj.List_COMMENT_ANONYMOUS = _unitOfWork.Comment.GetAll(u => u.isDisplay == false, includeProperties: "Idea").OrderByDescending(x => x.CreateDatetime);
            return View(obj);
        }
        [HttpPost]
        public List<object> Percentageofideas()
        {
            List<object> data = new List<object>();
            List<string> labels = _unitOfWork.Department.GetAll().Select(i => i.Name).ToList();
            data.Add(labels);
            int totalIdeas = _unitOfWork.Idea.GetAll().Count();
            List<int> labelsint = _unitOfWork.Department.GetAll().Select(i => i.Id).ToList();
            List<int> datasets = new List<int>();
            foreach (int i in labelsint)
            {
                datasets.Add((int)Math.Round((double)(100 * _unitOfWork.Idea.GetAll(h => h.ApplicationUser.DepartmentId == i).Count()) / totalIdeas));
            }
            data.Add(datasets);
            return data;
        }
        [HttpPost]
        public List<object> Numberofideas()
        {
            List<object> data = new List<object>();
            List<string> labels = _unitOfWork.Department.GetAll().Select(i => i.Name).ToList();
            data.Add(labels);
            List<int> labelsint = _unitOfWork.Department.GetAll().Select(i => i.Id).ToList();
            List<int> datasets = new List<int>();
            foreach (int i in labelsint)
            {
                datasets.Add(_unitOfWork.Idea.GetAll(h => h.ApplicationUser.DepartmentId == i).Count());
            }
            data.Add(datasets);
            return data;
        }
        [HttpPost]
        public List<object> Numberofcontributors()
        {
            List<object> data = new List<object>();
            List<string> labels = _unitOfWork.Department.GetAll().Select(i => i.Name).ToList();
            data.Add(labels);
            List<int> labelsint = _unitOfWork.Department.GetAll().Select(i => i.Id).ToList();
            List<int> datasets = new List<int>();
            foreach (int i in labelsint)
            {
                datasets.Add(_unitOfWork.ApplicationUser.GetAll(h => h.DepartmentId == i).Count());
            }
            data.Add(datasets);
            return data;
        }
    }
}
