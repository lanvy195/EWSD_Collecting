using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebEWSD_Collecting.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/{statusCode}")] //The Route attribute is used to route the URL "/Error/{statusCode}" to the ErrorController's HttpStatusCodeHandler method.
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            string message;
            if (statusCode == 404)
            {
                message = "Sorry, the resource you requested could not be found";
            }
            else
            {
                message = "An error occurred while processing your request";
            }

            ViewBag.ErrorMessage = message;
            return View("Index");
        }
    }
}
