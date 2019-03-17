using System.Web.Mvc;

namespace ConferenceRoomsScheduler.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}