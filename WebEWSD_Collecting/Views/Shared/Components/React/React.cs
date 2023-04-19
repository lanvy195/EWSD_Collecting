using EWSD_Collecting.DataAccess.Repository.IRepository;
using EWSD_Collecting.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebEWSD_Collecting.Views.Shared.Components.React
{
    public class React : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public React(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IViewComponentResult Invoke(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var objView = _unitOfWork.View.GetFirstOrDefault(x => x.IdeaId == id && x.ApplicationUserId == userId);
            ViewBag.IdeaId = id;
            ViewBag.FinalClosureDate = CheckFinalClosureDate(id);
            return View("_PartialReact", objView);
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
    }
}

