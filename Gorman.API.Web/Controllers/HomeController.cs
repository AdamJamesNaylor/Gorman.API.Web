
namespace Gorman.API.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Framework;
    using Framework.Domain;
    using Framework.Services;

    public class HomeController : Controller
    {
        public async Task<ActionResult> Index() {
            var endpoints = await Endpoints.Get(new Uri("http://localhost:6947"));
            var graphService = new FullGraphService(endpoints);
            var mapService = new MapService(endpoints);
            var map = await mapService.Add(new Map { TileUrl = "blargh" });
            map = await graphService.Get(map.Id);
            return View();
        }
    }
}