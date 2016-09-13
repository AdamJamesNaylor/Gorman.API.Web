
namespace Gorman.API.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Framework;
    using Framework.Services;

    public class HomeController : Controller
    {
        public async Task<ActionResult> Index() {
            var endpoints = await Endpoints.Get(new Uri("http://localhost:6947"));
            var service = new FullGraphService(endpoints);
            var map = await service.Get(123);
            return View();
        }
    }
}