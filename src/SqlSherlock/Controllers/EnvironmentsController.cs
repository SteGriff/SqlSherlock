using SqlSherlock.Data;
using System.Web.Mvc;

namespace SqlSherlock.Controllers
{
    public class EnvironmentsController : BaseController
    {
        // GET: Environments
        public ActionResult Index()
        {
            var library = new ConnectionLibrary();
            var result = library.GetConnections();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}