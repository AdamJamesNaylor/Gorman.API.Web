
namespace Gorman.API.Web.Controllers {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Framework;
    using Framework.Domain;
    using Framework.Services;
    using Action = Framework.Domain.Action;
    using Params = System.Collections.Generic.Dictionary<string, string>;

    //http://tacnet.io/tacsketch/#
    //http://www.stratuga.com/editor/
    //http://www.csgoboard.com/board
    //https://katanaapp.com

    public class HomeController : Controller {
        //todo rework errors as follows http://stackoverflow.com/a/1081853/17540
        public async Task<ActionResult> Index() {
            //var endpoints = await Endpoints.Get(new Uri("http://localhost:6947"));
            //var activityService = new ActivityService(endpoints);
            //var mapService = new MapService(endpoints);

            //http://www.zetaunit.com/wp-content/uploads/2016/03/ZETA-Unit-Tactics-and-Strategy-Guide-29.2.16.pdf page 36
            //https://github.com/ewoken/Leaflet.MovingMarker
            var heyoka = new Map {TileUrl = "blargh"};
            //heyoka = await mapService.Add(heyoka);

            var sunderer1 = new Actor { PositionX = 100, PositionY = 100 };
            var sunderer2 = new Actor { PositionX = 100, PositionY = 100 };
            var infantry = new Actor { PositionX = 100, PositionY = 100 };
            var infiltratorTeam = new Actor { PositionX = 100, PositionY = 100 };
            var armour = new Actor { PositionX = 100, PositionY = 100 };
            var enemy = new Actor { PositionX = 100, PositionY = 100 };
            var airDrop = new Actor { PositionX = 100, PositionY = 100 };
            var reinforcements = new Actor { PositionX = 100, PositionY = 100 };

            var rootStrategy = new Activity {
                MapId = heyoka.Id,
                Actors = new Collection<Actor> { sunderer1, sunderer2, infantry, infiltratorTeam, armour, enemy, airDrop, reinforcements }
            };

            #region Move sunderers

            var moveSunderersActivity = new Activity {ParentId = rootStrategy.Id};
            //"Move sunderers into position near the shields and begin an infantry only assault on the shields. The tower at position at yellow dot is a good sunderer location, as it can be very easy to defend.  The main aim is to get the enemy into an infantry based defend/respawn routine at the shields."

            moveSunderersActivity.AddAction(sunderer1, new Params { { ActionParameter.PositionX, "200" } });
            moveSunderersActivity.AddAction(sunderer2, new Params { { ActionParameter.PositionX, "200" } });

            rootStrategy.Activities.Add(moveSunderersActivity);

            #endregion

            #region Attack shield gens

            var attackShieldGensActivity = new Activity { ParentId = rootStrategy.Id }; //needs to continue displaying text from previous activity
            attackShieldGensActivity.AddAction(infantry, new Params { { ActionParameter.PositionX, "200" } });
            //moveInfantryAction.Parameters.Add(ActionParameter.PositionY, "200");

            rootStrategy.Activities.Add(attackShieldGensActivity);

            #endregion

            #region Send Infiltrators
            var sendInfiltratorsActivity = new Activity { ParentId = rootStrategy.Id }; //"Whilst attacking the shields, send a team of infiltrators to hack the turrets at the very top level of the techplant (blue), but don’t open fire yet.Concentrate on the turrets overlooking the enemy spawn room."

            sendInfiltratorsActivity.AddAction(infiltratorTeam, new Params {{ActionParameter.PositionX, "200"}});
            rootStrategy.Activities.Add(sendInfiltratorsActivity);
            #endregion

            var hackTurretsActivity = new Activity { ParentId = rootStrategy.Id }; //needs to display above
            var bringInAntiInfantryArmourActivity = new Activity { ParentId = rootStrategy.Id }; //"As the shields reach overloading point and infantry combat is at its fullest, bring in a strong anti - infantry armour column to patrol the red line."
            var patrolActivity = new Activity { ParentId = rootStrategy.Id }; //as above
            var relocateInfiltratorsActivity = new Activity { ParentId = rootStrategy.Id }; //"This sudden shift in weight should make it easy to hack the vehicle terminal at green dot and thus to keep the vehicle presence high."
            var hackVehicleTerminalActivity = new Activity { ParentId = rootStrategy.Id }; //as above

            var suppressSpawnActivity = new Activity { ParentId = rootStrategy.Id }; //"With turret and vehicle support, suppress the enemy at spawn whilst either capturing the SCU.Put a sunderer as close to the SCU as possible (blue dot)."
            var captureSCUActivity = new Activity { ParentId = rootStrategy.Id }; //as above: capture SCU + deploy sunderer simultaneously

            var enemyCounterActivity = new Activity { ParentId = rootStrategy.Id }; //"If the enemy is able to resist the SCU attack, shift your infantry weight to the A point (via an air drop) and back again to the SCU to cause some of their forces to relocate and remain there, but maintain the vehicle and turret majority, a priority over your infantry forces."
            var airDropPointActivity = new Activity { ParentId = rootStrategy.Id }; //as above
            var counterSCUActivity = new Activity { ParentId = rootStrategy.Id }; //as above

            rootStrategy.Activities.Add(moveSunderersActivity);
            //rootStrategy = await activityService.Add(rootStrategy);

            //moveSniper.Parameters.Add(ActionParameter.PositionY, "200");


            //var rootActivity = await activityService.Get(moveSunderersActivity.Id);
            return View();
        }
    }
}