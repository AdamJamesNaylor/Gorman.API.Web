
namespace Gorman.API.Web.Controllers {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Framework;
    using Framework.Domain;
    using Framework.Services;
    using Newtonsoft.Json;
    using Action = Framework.Domain.Action;
    using Params = System.Collections.Generic.Dictionary<string, string>;

    //http://tacnet.io/tacsketch/#
    //http://www.stratuga.com/editor/
    //http://www.csgoboard.com/board
    //https://katanaapp.com
    //http://cdn3.dualshockers.com/wp-content/uploads/2016/03/Starfarer_map.jpg


    public class HomeController : Controller {
        //todo rework errors as follows http://stackoverflow.com/a/1081853/17540
        public async Task<ActionResult> Index() {
            var endpoints = await Endpoints.Get(new Uri("http://localhost:6947/v0.1"));
            var activityService = new ActivityService(endpoints);
            //var mapService = new MapService(endpoints);

            //http://www.zetaunit.com/wp-content/uploads/2016/03/ZETA-Unit-Tactics-and-Strategy-Guide-29.2.16.pdf page 36
            //https://github.com/ewoken/Leaflet.MovingMarker
            //http://easings.net/
            var heyoka = new Map {TileUrl = "blargh"};
            //heyoka = await mapService.Add(heyoka);

            var sunderer1 = new Actor { PositionX = 100, PositionY = 100, ImageUrl = "/images/sunderer_diagram.png" };
            var sunderer2 = new Actor { PositionX = 100, PositionY = 100, ImageUrl = "/images/sunderer_diagram.png" };
            var infantry = new Actor { PositionX = 100, PositionY = 100, ImageUrl = "/images/sunderer_diagram.png" }; //
            var infiltratorTeam = new Actor { PositionX = 100, PositionY = 100, ImageUrl = "/images/infiltrator.png" };
            var hackIcon1 = new Actor { PositionX = 100, PositionY = 100, ImageUrl = "/images/hack.png" };
            var hackIcon2 = new Actor { PositionX = 100, PositionY = 100, ImageUrl = "/images/hack.png" };
            var armour = new Actor { PositionX = 100, PositionY = 100, ImageUrl = "/images/magrider_diagram.png" };
            var supress = new Actor { PositionX = 100, PositionY = 100, ImageUrl = "/images/icon_attack.png" };
            var enemy = new Actor { PositionX = 100, PositionY = 100, ImageUrl = "/images/sunderer_diagram.png" }; //
            var airDrop = new Actor { PositionX = 100, PositionY = 100, ImageUrl = "/images/sunderer_diagram.png" }; //
            var reinforcements = new Actor { PositionX = 100, PositionY = 100, ImageUrl = "/images/sunderer_diagram.png" }; //

            var rootStrategy = new Activity {
                MapId = heyoka.Id,
                Actors = new Collection<Actor> { sunderer1, sunderer2, infantry, infiltratorTeam, hackIcon1, armour, enemy, airDrop, reinforcements }
            };

            #region Move sunderers

            var moveSunderersActivity = new Activity {ParentId = rootStrategy.Id};
            //"Move sunderers into position near the shields and begin an infantry only assault on the shields. The tower at position at yellow dot is a good sunderer location, as it can be very easy to defend.  The main aim is to get the enemy into an infantry based defend/respawn routine at the shields."

            moveSunderersActivity.AddAction(sunderer1, new Params { { ActionParameter.PositionX, "200" }, { ActionParameter.Display, "show" } });
            moveSunderersActivity.AddAction(sunderer2, new Params { { ActionParameter.PositionX, "200" }, { ActionParameter.Display, "show" } });

            rootStrategy.Activities.Add(moveSunderersActivity);

            #endregion

            #region Attack shield gens

            var attackShieldGensActivity = new Activity { ParentId = rootStrategy.Id }; //needs to continue displaying text from previous activity
            attackShieldGensActivity.AddAction(infantry, new Params { { ActionParameter.PositionX, "200" }, { ActionParameter.Display, "show" } });
            //moveInfantryAction.Parameters.Add(ActionParameter.PositionY, "200");

            rootStrategy.Activities.Add(attackShieldGensActivity);

            #endregion

            #region Send Infiltrators
            var sendInfiltratorsActivity = new Activity { ParentId = rootStrategy.Id }; //"Whilst attacking the shields, send a team of infiltrators to hack the turrets at the very top level of the techplant (blue), but don’t open fire yet.Concentrate on the turrets overlooking the enemy spawn room."

            sendInfiltratorsActivity.AddAction(infiltratorTeam, new Params { {ActionParameter.PositionX, "200"}, { ActionParameter.Display, "show" } });
            rootStrategy.Activities.Add(sendInfiltratorsActivity);
            #endregion

            #region Hack turrets
            var hackTurretsActivity = new Activity { ParentId = rootStrategy.Id }; //needs to display above

            hackTurretsActivity.AddAction(hackIcon1, new Params { { ActionParameter.Display, "show" } });
            rootStrategy.Activities.Add(hackTurretsActivity);
            #endregion

            #region Bring in armour
            var bringInAntiInfantryArmourActivity = new Activity { ParentId = rootStrategy.Id }; //"As the shields reach overloading point and infantry combat is at its fullest, bring in a strong anti - infantry armour column to patrol the red line."
            bringInAntiInfantryArmourActivity.AddAction(armour, new Params { { ActionParameter.Display, "show" }, { ActionParameter.PositionX, "200" } });
            rootStrategy.Activities.Add(bringInAntiInfantryArmourActivity);
            #endregion

            #region Patrol
            var patrolActivity = new Activity { ParentId = rootStrategy.Id }; //as above
            patrolActivity.AddAction(armour, new Params { { ActionParameter.PositionX, "200" } });
            rootStrategy.Activities.Add(patrolActivity);
            #endregion

            #region Relocate infilitrators
            var relocateInfiltratorsActivity = new Activity { ParentId = rootStrategy.Id }; //"This sudden shift in weight should make it easy to hack the vehicle terminal at green dot and thus to keep the vehicle presence high."
            relocateInfiltratorsActivity.AddAction(infiltratorTeam, new Params { { ActionParameter.PositionX, "200" } });
            relocateInfiltratorsActivity.AddAction(hackIcon1, new Params { { ActionParameter.Display, "hide" } });
            rootStrategy.Activities.Add(relocateInfiltratorsActivity);
            #endregion

            #region Hack vehicle terminal
            var hackVehicleTerminalActivity = new Activity { ParentId = rootStrategy.Id }; //as above
            hackVehicleTerminalActivity.AddAction(hackIcon2, new Params { { ActionParameter.Display, "show" } });
            rootStrategy.Activities.Add(hackVehicleTerminalActivity);
            #endregion

            #region Supress spawn activity
            var suppressSpawnActivity = new Activity { ParentId = rootStrategy.Id }; //"With turret and vehicle support, suppress the enemy at spawn whilst either capturing the SCU.Put a sunderer as close to the SCU as possible (blue dot)."
            suppressSpawnActivity.AddAction(supress, new Params { { ActionParameter.Display, "show" } });
            rootStrategy.Activities.Add(suppressSpawnActivity);
            #endregion

            #region Capture SCU
            var captureSCUActivity = new Activity { ParentId = rootStrategy.Id }; //as above: capture SCU + deploy sunderer simultaneously
            captureSCUActivity.AddAction(infantry, new Params { { ActionParameter.PositionX, "200" } });
            captureSCUActivity.AddAction(sunderer2, new Params { { ActionParameter.PositionX, "200" } });
            rootStrategy.Activities.Add(captureSCUActivity);
            #endregion

            #region Enemy counter SCU
            var enemyCounterActivity = new Activity { ParentId = rootStrategy.Id }; //"If the enemy is able to resist the SCU attack, shift your infantry weight to the A point (via an air drop) and back again to the SCU to cause some of their forces to relocate and remain there, but maintain the vehicle and turret majority, a priority over your infantry forces."
            enemyCounterActivity.AddAction(enemy, new Params { { ActionParameter.Display, "show" } });
            rootStrategy.Activities.Add(enemyCounterActivity);
            #endregion

            #region Air drop point A
            var airDropPointActivity = new Activity { ParentId = rootStrategy.Id }; //as above
            airDropPointActivity.AddAction(airDrop, new Params { { ActionParameter.Display, "show" }, { ActionParameter.PositionX, "200" } });
            airDropPointActivity.AddAction(reinforcements, new Params { { ActionParameter.Display, "show" } });
            rootStrategy.Activities.Add(airDropPointActivity);
            #endregion

            #region Re-counter SCU
            var counterSCUActivity = new Activity { ParentId = rootStrategy.Id }; //as above
            counterSCUActivity.AddAction(reinforcements, new Params { { ActionParameter.PositionX, "200" } });
            counterSCUActivity.AddAction(infantry, new Params { { ActionParameter.PositionX, "200" } });
            rootStrategy.Activities.Add(counterSCUActivity);
            #endregion

            var result = await activityService.Add(rootStrategy);

            result = await activityService.Get(result.Id, true);
            var model = JsonConvert.SerializeObject(result);
            return new ContentResult { Content = model };
        }

        [Route("player")]
        [HttpGet]
        public async Task<ActionResult> Player() {
            var endpoints = await Endpoints.Get(new Uri("http://localhost:6947/v0.1"));
            var activityService = new ActivityService(endpoints);

            var activity = await activityService.Get(103, true);

            return View(activity);
        }
    }
}