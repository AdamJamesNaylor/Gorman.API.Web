
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
        //todo rework errors as follows http://stackoverflow.com/a/1081853/17540
        public async Task<ActionResult> Index() {
            var endpoints = await Endpoints.Get(new Uri("http://localhost:6947"));
            var activityService = new ActivityService(endpoints);
            var mapService = new MapService(endpoints);

            var portOlisarInteriorMap = new Map { TileUrl = "blargh" };
            portOlisarInteriorMap = await mapService.Add(portOlisarInteriorMap);
            var rootStrategy = new Activity { MapId = portOlisarInteriorMap.Id };
            rootStrategy = await activityService.Add(rootStrategy);
            var moveIntoPositionActivity = new Activity { ParentId = rootStrategy.Id };
            moveIntoPositionActivity = await activityService.Add(moveIntoPositionActivity);
            var sniper = new Actor();
            var grunt = new Actor();
            rootStrategy.
            var rootActivity = await activityService.Get(moveIntoPositionActivity.Id);
            /*var graphService = new FullGraphService(endpoints);
            map = await graphService.Get(map.Id);*/
            return View();
        }
    }
}