using EWSD_Collecting.DataAccess.Repository.IRepository;
using EWSD_Collecting.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebEWSD_Collecting.Views.Shared.Components.ShowComment
{
    public class ShowComment : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShowComment(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IViewComponentResult Invoke(int id)
        {
            IEnumerable<Comment> objCommentList = _unitOfWork.Comment.GetAll(i => i.IdeaId == id, includeProperties: "ApplicationUser");
            return View("_PartialShowComment", objCommentList);
        }
    }
}