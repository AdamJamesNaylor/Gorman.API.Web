
namespace Gorman.API.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Framework;
    using Framework.Services;
    using Framework.Validators;

    public class HomeController : Controller
    {
        // GET: Home
        public async Task<ActionResult> Index() {
            var endpoints = await Endpoints.Get("http://localhost:9999", new ResponseValidator());
            var service = new FullGraphService(endpoints);
            return View();
        }
    }
}