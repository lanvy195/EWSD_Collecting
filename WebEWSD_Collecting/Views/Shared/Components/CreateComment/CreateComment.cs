using EWSD_Collecting.DataAccess.Repository.IRepository;
using EWSD_Collecting.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebEWSD_Collecting.Views.Shared.Components.CreateComment
{
    public class CreateComment : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateComment(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IViewComponentResult Invoke(int id)
        {
            Comment comment = new Comment();
            ViewBag.IdeaId = id;
            ViewBag.FinalClosureDate = CheckFinalClosureDate(id);
            return View("_PartialCreateComment", comment);
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