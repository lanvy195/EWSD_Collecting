using EWSD_Collecting.DataAccess.Data;
using EWSD_Collecting.DataAccess.Repository.IRepository;
using EWSD_Collecting.Models;
using EWSD_Collecting.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace WebEWSD_Collecting.Controllers
{
    [Authorize(Roles = SD.Role_User_QAManager + "," + SD.Role_User_Administrator)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
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
            IEnumerable<Category> items = _unitOfWork.Category.GetAll().OrderByDescending(x => x.Id);
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
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
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
            //var categoryformDb = _db.Categories.Find(id);
            var categoryformDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            //var categoryformDbsingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (categoryformDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryformDbFirst);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
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
            var objIdeaTemp = _unitOfWork.Idea.GetFirstOrDefault(u => u.CategoryId == id);
            if (objIdeaTemp == null)
            {
                var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
                if (obj != null)
                {
                    _unitOfWork.Category.Remove(obj);
                    _unitOfWork.Save();
                    TempData["Deleted"] = "Delete successfully";
                    return Json(new { success = true });
                }
                else
                {
                    return NotFound();
                }
            }
            TempData["Deleted"] = "Cannot be deleted because there are ideas link to this category.";
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
                        var objIdeaTemp = _unitOfWork.Idea.GetFirstOrDefault(u => u.CategoryId == Convert.ToInt32(item));
                        if (objIdeaTemp != null)
                        {
                            TempData["Deleted"] = "Cannot be deleted because there are ideas link to some categories.";
                            return Json(new { success = false });
                        }
                    }
                    foreach (var item in items)
                    {
                        var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == Convert.ToInt32(item));
                        _unitOfWork.Category.Remove(obj);
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
