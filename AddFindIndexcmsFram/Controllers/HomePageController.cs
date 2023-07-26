using AddFindIndexcmsFram.Models.Pages;
using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace AddFindIndexcmsFram.Controllers
{
    public class HomePageController : PageController<HomePage>
    {
        public ActionResult Index(HomePage currentPage)
        {
            /* Implementation of action. You can create your own view model class that you pass to the view or
             * you can pass the page type for simpler templates */

            return View(currentPage);
        }
    }
}