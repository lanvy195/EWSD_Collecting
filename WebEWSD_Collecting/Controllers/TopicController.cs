using EWSD_Collecting.DataAccess.Repository.IRepository;
using EWSD_Collecting.Models;
using EWSD_Collecting.Utility;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace WebEWSD_Collecting.Controllers
{
    [Authorize(Roles = SD.Role_User_QAManager + "," + SD.Role_User_Administrator + "," +SD.Role_User_QACoordinator)]
    public class TopicController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TopicController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Topic> items = _unitOfWork.Topic.GetAll().OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => EWSD_Collecting.Utility.Filter.FilterChar(x.Name).Contains(EWSD_Collecting.Utility.Filter.FilterChar(Searchtext)));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            ViewBag.SearchText = Searchtext;
            return View(items);
        }
        public IActionResult Create()
        {
            return View();
        }
        //Post Ceate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Topic obj)
        {
            obj.CreateDatetime = DateTime.Now;
            if (obj.ClosureDate < DateTime.Now)
            {
                TempData["Deleted"] = "Closure Date must be after Today";
                return View(obj);
            }
            if (obj.FinalClosureDate < obj.ClosureDate)
            {
                TempData["Deleted"] = "Final Closure Date must be after Closure Date";
                return View(obj);
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Topic.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Create successfully";
                return RedirectToAction("index");
            }
            TempData["Deleted"] = "Create Failed";
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            var topicformDb = _unitOfWork.Topic.GetFirstOrDefault(x => x.Id == id);
            //var departmentformDb = _db.Departments.Find(id);
            //var departmentformDbFirst = _db.Departments.FirstOrDefault(u=>u.Id== id);
            //var departmentformDbsingle = _db.Departments.SingleOrDefault(u => u.Id == id);
            if (topicformDb == null)
            {
                return NotFound();
            }
            return View(topicformDb);
        }
        //post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Topic obj)
        {
            if (obj.ClosureDate < obj.CreateDatetime)
            {
                TempData["Deleted"] = "Closure Date must be after Create Date";
                return View(obj);
            }
            if (obj.FinalClosureDate < obj.ClosureDate)
            {
                TempData["Deleted"] = "Final Closure Date must be after Closure Date";
                return View(obj);
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Topic.Update(obj);
                _unitOfWork.Save();
                TempData["Edited"] = "Edit successfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var objIdeaTemp = _unitOfWork.Idea.GetFirstOrDefault(u => u.TopicId == id);
            if (objIdeaTemp == null)
            {
                var obj = _unitOfWork.Topic.GetFirstOrDefault(u => u.Id == id);
                if (obj != null)
                {
                    _unitOfWork.Topic.Remove(obj);
                    _unitOfWork.Save();
                    TempData["Deleted"] = "Delete successfully";
                    return Json(new { success = true });
                }
                else
                {
                    return NotFound();
                }
            }
            TempData["Deleted"] = "Cannot be deleted because there are ideas present in this topic.";
            return Json(new { success = false });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach(var item  in items)
                    {
                        var objIdeaTemp = _unitOfWork.Idea.GetFirstOrDefault(u => u.TopicId == Convert.ToInt32(item));
                        if (objIdeaTemp != null)
                        {
                            TempData["Deleted"] = "Cannot be deleted because there are ideas present in some topic.";
                            return Json(new { success = false });
                        }
                    }
                    foreach (var item in items)
                    {
                        var obj = _unitOfWork.Topic.GetFirstOrDefault(u => u.Id == Convert.ToInt32(item));
                        _unitOfWork.Topic.Remove(obj);
                        _unitOfWork.Save();
                        TempData["Deleted"] = "Delete successfully";
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}