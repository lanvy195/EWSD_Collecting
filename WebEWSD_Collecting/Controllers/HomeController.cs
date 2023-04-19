using EWSD_Collecting.DataAccess.Repository.IRepository;
using EWSD_Collecting.Models;
using EWSD_Collecting.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WebEWSD_Collecting.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
        {
            HomeVM  homeVM = new HomeVM();
            homeVM.MostViews = _unitOfWork.Idea.GetAll(includeProperties: "ApplicationUser").OrderByDescending(x=>x.Views).Take(5);
            homeVM.MostPopular = _unitOfWork.Idea.GetAll(includeProperties: "ApplicationUser").OrderByDescending(x => x.Likes- x.Dislikes).Take(5);
            homeVM.LatestIdeas = _unitOfWork.Idea.GetAll(includeProperties: "ApplicationUser").OrderByDescending(x => x.CreateDatetime).Take(5);
            homeVM.LatestComments= _unitOfWork.Comment.GetAll(includeProperties: "ApplicationUser,Idea").OrderByDescending(x => x.CreateDatetime).Take(5);
			return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}