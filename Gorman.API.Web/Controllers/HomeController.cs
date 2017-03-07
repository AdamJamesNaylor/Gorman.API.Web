
namespace Gorman.API.Web.Controllers {
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Framework;
    using Framework.Domain;
    using Framework.Services;

    //http://tacnet.io/tacsketch/#
    //http://www.stratuga.com/editor/
    //http://www.csgoboard.com/board
    //https://katanaapp.com

    public class HomeController : Controller {
        //todo rework errors as follows http://stackoverflow.com/a/1081853/17540
        public async Task<ActionResult> Index() {
            var endpoints = await Endpoints.Get(new Uri("http://localhost:6947"));
            var activityService = new ActivityService(endpoints);
            var mapService = new MapService(endpoints);

            var portOlisarInteriorMap = new Map {TileUrl = "blargh"};
            portOlisarInteriorMap = await mapService.Add(portOlisarInteriorMap);

            var sniper = new Actor { PositionX = 100 };
            var grunt = new Actor();
            var rootStrategy = new Activity {
                MapId = portOlisarInteriorMap.Id,
                Actors = new Collection<Actor> {sniper, grunt}
            };

            var moveIntoPositionActivity = new Activity { ParentId = rootStrategy.Id };
            var moveSniper = new Framework.Domain.Action {
                ActorId = rootStrategy.Actors[0].Id,
                ActivityId = moveIntoPositionActivity.Id,
            };
            moveIntoPositionActivity.Actions.Add(moveSniper);
            rootStrategy.Activities.Add(moveIntoPositionActivity);
            rootStrategy = await activityService.Add(rootStrategy);

            //https://github.com/ewoken/Leaflet.MovingMarker
            moveSniper.Parameters.Add(ActionParameter.PositionX, "200");
            
            var rootActivity = await activityService.Get(moveIntoPositionActivity.Id);
            return View();
        }
    }
}