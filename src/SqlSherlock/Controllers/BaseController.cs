using SqlSherlock.Models;
using System.Text;
using System.Web.Mvc;

namespace SqlSherlock.Controllers
{
    public abstract class BaseController : Controller
    {
        protected override JsonResult Json(
            object data,
            string contentType,
            Encoding contentEncoding,
            JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}