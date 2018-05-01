using System.Web.Mvc;
using ApiClient.Core.Configuration;
using ApiClient.Models;

namespace Example.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ApiClientSettings ClientSettingsProvider { get; private set; } = null;

        public HomeController()
        {
            ApiClientConfigHelper.Instance().Save();
            // ClientSettingsProvider = ApiClientSettings.CreateFromConfigFile();
        }

        public ActionResult Index()
        {
            return View(ClientSettingsProvider);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
