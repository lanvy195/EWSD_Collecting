using ClosedXML.Excel;
using EWSD_Collecting.DataAccess.Data;
using EWSD_Collecting.DataAccess.Repository.IRepository;
using EWSD_Collecting.Models;
using EWSD_Collecting.Models.ViewModel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Ionic.Zip;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Security.Claims;
using EWSD_Collecting.Utility.Mail;
using X.PagedList;
using EWSD_Collecting.Utility;
using WebEWSD_Collecting.Migrations;

namespace WebEWSD_Collecting.Controllers
{
    [Authorize(Roles = SD.Role_User_QACoordinator + "," + SD.Role_User_QAManager + "," + SD.Role_User_Administrator + "," + SD.Role_User_Staff)]
    public class IdeaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendMailService _mailService;
        private IWebHostEnvironment _webHostEnvironment;

        public IdeaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, ISendMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _mailService = mailService;
        }
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Topic> items = _unitOfWork.Topic.GetAll().OrderByDescending(x => x.ClosureDate);
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
        public IActionResult Detail(int id)
        {
            var objIdea = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,Topic,ApplicationUser");
            if (objIdea == null)
            {
                return NotFound();
            };
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claims.Value;
            var objView = _unitOfWork.View.GetFirstOrDefault(x => x.IdeaId == id && x.ApplicationUserId == userId);
            ManageView(id, objIdea, userId, objView);
            return View(objIdea);
        }
        public IActionResult View(int TopicId, string Searchtext, int? page)
        {
            var obj = _unitOfWork.Topic.GetFirstOrDefault(x => x.Id == TopicId);
            if (obj == null)
            {
                return NotFound();
            };
            ViewBag.TopicId = TopicId;
            ViewBag.TopicName = obj.Name;
            ViewBag.SearchText = Searchtext;
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Idea> items = _unitOfWork.Idea.GetAll(x => x.TopicId == TopicId).OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => EWSD_Collecting.Utility.Filter.FilterChar(x.Title).Contains(EWSD_Collecting.Utility.Filter.FilterChar(Searchtext)));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public IActionResult Create(int TopicId)
        {
            var topicCheck = _unitOfWork.Topic.GetFirstOrDefault(u => u.Id == TopicId);
            if (topicCheck == null)
            {
                return NotFound();
            };
            if (topicCheck.ClosureDate < DateTime.Now)
            {
                TempData["Deleted"] = "It's too late to create ideas now.";
                return RedirectToAction("View", "Idea", new { TopicId });
            }
            IdeaVM ideaVM = new IdeaVM();
            ideaVM.idea = new Idea();
            ideaVM.TopicList = _unitOfWork.Topic.GetAll().Select(
                    u => new SelectListItem()
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                );
            ideaVM.CategoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                );

            ViewBag.TopicId = TopicId;
            return View(ideaVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdeaVM obj)
        {
            var topicCheck = _unitOfWork.Topic.GetFirstOrDefault(u => u.Id == obj.idea.TopicId);
            if (topicCheck.ClosureDate < DateTime.Now)
            {
                TempData["Deleted"] = "It's too late to create ideas now.";
                return RedirectToAction("View", "Idea", new { obj.idea.TopicId });
            }
            if (obj.idea.isAgree != true)
            {
                obj.CategoryList = _unitOfWork.Category.GetAll().Select(
                    u => new SelectListItem()
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                );

                ViewBag.TopicId = obj.idea.TopicId;
                TempData["Deleted"] = "You must agree to the terms and conditions before submitting.";
                return View(obj);
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claims.Value;
            obj.idea.ApplicationUserId = userId;
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (obj.file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var userMail = User.FindFirstValue(ClaimTypes.Email);
                    string uploads = Path.Combine(wwwRootPath, @"file/topic_" + obj.idea.TopicId + @"/" + userMail + @"_" + obj.idea.Title);
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }

                    var extension = Path.GetExtension(obj.file.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        obj.file.CopyTo(fileStreams);
                    }
                    obj.idea.Path = @"file/topic_" + obj.idea.TopicId + @"/" + userMail + @"_" + obj.idea.Title + @"/" + fileName + extension;
                }
                _unitOfWork.Idea.Add(obj.idea);
                _unitOfWork.Save();
                var userinfo = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == userId);
                var userQA = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.DepartmentId == userinfo.DepartmentId && u.isQA == true);
                /*  if (userQA != null)
                  {
               
                  }
                */
                _mailService.IdeaSubmissionEmail(userQA.Email, "", "");
                TempData["Success"] = "Create successfully";

                return RedirectToAction("View", "Idea", new { obj.idea.TopicId });
            }
            TempData["Deleted"] = "Create failed";
            return RedirectToAction("Create", "Idea", new { obj.idea.TopicId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Like(int id)
        {
            var objIdeaTemp = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == id);
            if (objIdeaTemp == null)
            {
                return NotFound();
            };
            if (/*CheckFinalClosureDate(objIdeaTemp.Id) ==*/ true)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                string userId = claims.Value;
                var objViewTemp = _unitOfWork.View.GetFirstOrDefault(x => x.IdeaId == id && x.ApplicationUserId == userId);
                ManageView(id, objIdeaTemp, userId, objViewTemp);
                var objIdea = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,Topic");
                var objView = _unitOfWork.View.GetFirstOrDefault(x => x.IdeaId == id && x.ApplicationUserId == userId);
                switch (objView.React)
                {
                    case -1:
                        objView.React = 1;
                        objIdea.Likes += 1;
                        objIdea.Dislikes -= 1;
                        break;
                    case 1:
                        objView.React = 0;
                        objIdea.Likes -= 1;
                        break;
                    default:
                        objView.React = 1;
                        objIdea.Likes += 1;
                        break;
                }
                _unitOfWork.Idea.Update(objIdea);
                _unitOfWork.View.Update(objView);
                _unitOfWork.Save();
                return Json(new { success = true });
            }
            //TempData["Deleted"] = "It's too late to like or dislike now.";
            //return Json(new { success = false });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Dislike(int id)
        {
            var objIdeaTemp = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == id);
            if (objIdeaTemp == null)
            {
                return NotFound();
            };
            if (/*CheckFinalClosureDate(objIdeaTemp.Id) ==*/ true)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                string userId = claims.Value;
                var objViewTemp = _unitOfWork.View.GetFirstOrDefault(x => x.IdeaId == id && x.ApplicationUserId == userId);
                ManageView(id, objIdeaTemp, userId, objViewTemp);
                var objIdea = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == id);
                var objView = _unitOfWork.View.GetFirstOrDefault(x => x.IdeaId == id && x.ApplicationUserId == userId);
                switch (objView.React)
                {
                    case -1:
                        objView.React = 0;
                        objIdea.Dislikes -= 1;
                        break;
                    case 1:
                        objView.React = -1;
                        objIdea.Likes -= 1;
                        objIdea.Dislikes += 1;
                        break;
                    default:
                        objView.React = -1;
                        objIdea.Dislikes += 1;
                        break;
                }
                _unitOfWork.Idea.Update(objIdea);
                _unitOfWork.View.Update(objView);
                _unitOfWork.Save();
                return Json(new { success = true });
            }
            //TempData["Deleted"] = "It's too late to like or dislike now.";
            //return Json(new { success = false });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Comment(EWSD_Collecting.Models.Comment comment)
        {
            if (CheckFinalClosureDate(comment.IdeaId) == true)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                string userId = claims.Value;
                comment.ApplicationUserId = userId;
                if (ModelState.IsValid)
                {
                    var tempIdea = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == comment.IdeaId);
                    tempIdea.Comments += 1;
                    _unitOfWork.Idea.Update(tempIdea);
                    _unitOfWork.Comment.Add(comment);
                    _unitOfWork.Save();
                    var ideatemp = _unitOfWork.Idea.GetFirstOrDefault(i => i.Id == comment.IdeaId, includeProperties: "ApplicationUser");
                    _mailService.CommentIdeaSubmissionEmail(ideatemp.ApplicationUser.Email, "", "");
                    TempData["Success"] = "Create successfully";
                    return RedirectToAction("Detail", "Idea", new { @id = comment.IdeaId });
                }
            }
            TempData["Deleted"] = "It's too late to leave a comment now.";
            return RedirectToAction("Detail", "Idea", new { @id = comment.IdeaId });
        }
        public bool CheckFinalClosureDate(int id)
        {
            var ideaCheck = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == id);
            var topicCheck = _unitOfWork.Topic.GetFirstOrDefault(u => u.Id == ideaCheck.TopicId);
            if (topicCheck.FinalClosureDate < DateTime.Now)
            {
                return false;
            }
            return true;
        }
        public void ManageView(int id, Idea objIdea, string userId, View objView)
        {
            if (objView == null)
            {
                View objViewNew = new View();
                objViewNew.ApplicationUserId = userId;
                objViewNew.IdeaId = id;
                _unitOfWork.View.Add(objViewNew);
                objIdea.Views += 1;
                _unitOfWork.Idea.Update(objIdea);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.View.Update(objView);
                _unitOfWork.Save();
            }
        }
        [Authorize(Roles = SD.Role_User_QACoordinator + "," + SD.Role_User_QAManager + "," + SD.Role_User_Administrator)]
        public IActionResult DownloadZip(int id)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            // Path to the folder to be compressed.
            string folderPath = Path.Combine(wwwRootPath, @"file/topic_" + id); ;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            // Name of the zip file.
            string zipFileName = "topic_" + id + ".zip";
            // Path to the zip file to be created.
            string zipPath = Path.Combine(Path.GetTempPath(), zipFileName);
            // Use DotNetZip to create the zip file.
            using (ZipFile zip = new ZipFile())
            {
                // Add the files in the folder to the zip file.
                zip.AddDirectory(folderPath);

                // Save the zip file to the specified path.
                zip.Save(zipPath);
            }
            // Return the zip file to the user.
            byte[] fileBytes = System.IO.File.ReadAllBytes(zipPath);
            return File(fileBytes, MediaTypeNames.Application.Zip, zipFileName);
        }
        [Authorize(Roles = SD.Role_User_QACoordinator + "," + SD.Role_User_QAManager + "," + SD.Role_User_Administrator)]
        public IActionResult DownloadExcel(int id)
        {
            var ideas = _unitOfWork.Idea.GetAll(i => i.TopicId == id, includeProperties: "Category,ApplicationUser");
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Ideas");
            worksheet.Cell("A1").Value = "ID";
            worksheet.Cell("B1").Value = "First Name";
            worksheet.Cell("C1").Value = "Last Name";
            worksheet.Cell("D1").Value = "Title";
            worksheet.Cell("E1").Value = "Description";
            worksheet.Cell("F1").Value = "Category";
            worksheet.Cell("G1").Value = "Path";
            worksheet.Cell("H1").Value = "Views";
            worksheet.Cell("I1").Value = "Like";
            worksheet.Cell("J1").Value = "Dislike";
            int row = 2;
            int i = 1;
            foreach (var idea in ideas)
            {
                worksheet.Cell("A" + row).Value = i;
                worksheet.Cell("B" + row).Value = idea.ApplicationUser.FirstName;
                worksheet.Cell("C" + row).Value = idea.ApplicationUser.LastName;
                worksheet.Cell("D" + row).Value = idea.Title;
                worksheet.Cell("E" + row).Value = idea.Description;
                worksheet.Cell("F" + row).Value = idea.Category.Name;
                worksheet.Cell("G" + row).Value = idea.Path;
                worksheet.Cell("H" + row).Value = idea.Views;
                worksheet.Cell("I" + row).Value = idea.Likes;
                worksheet.Cell("J" + row).Value = idea.Dislikes;
                row++;
                i++;
            }
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "Topic_" + id + ".xlsx";
            return File(stream, contentType, fileName);
        }
    }
}