using EWSD_Collecting.DataAccess.Repository.IRepository;
using EWSD_Collecting.Models;
using EWSD_Collecting.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace WebEWSD_Collecting.Controllers
{
    [Authorize(Roles = SD.Role_User_Administrator)]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Department> items = _unitOfWork.Department.GetAll().OrderByDescending(x => x.Id);
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
        public IActionResult Create(Department obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Department.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Create successfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var departmentformDb = _unitOfWork.Department.GetFirstOrDefault(x => x.Id == id);
            //var departmentformDb = _db.Departments.Find(id);
            //var departmentformDbFirst = _db.Departments.FirstOrDefault(u=>u.Id== id);
            //var departmentformDbsingle = _db.Departments.SingleOrDefault(u => u.Id == id);
            if (departmentformDb == null)
            {
                return NotFound();
            }
            return View(departmentformDb);
        }
        //post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Department.Update(obj);
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
            var objTemp = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.DepartmentId == id);
            if (objTemp == null)
            {
                var obj = _unitOfWork.Department.GetFirstOrDefault(u => u.Id == id);
                if (obj != null)
                {
                    _unitOfWork.Department.Remove(obj);
                    _unitOfWork.Save();
                    TempData["Deleted"] = "Delete successfully";
                    return Json(new { success = true });
                }
                else
                {
                    return NotFound();
                }
            }
            TempData["Deleted"] = "Cannot be deleted because there are users link to this department.";
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
                    foreach (var item in items)
                    {
                        var objTemp = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.DepartmentId == Convert.ToInt32(item));
                        if (objTemp != null)
                        {
                            TempData["Deleted"] = "Cannot be deleted because there are users link to some departments.";
                            return Json(new { success = false });
                        }
                    }
                    foreach (var item in items)
                    {
                        var obj = _unitOfWork.Department.GetFirstOrDefault(u => u.Id == Convert.ToInt32(item));
                        _unitOfWork.Department.Remove(obj);
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
