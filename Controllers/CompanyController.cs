using System;
using System.Linq;
using System.Web.Mvc;
using fintech.DAL;

namespace fintech.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyRepository _repo = new CompanyRepository();

        public ActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        [HttpPost]
        public JsonResult UploadLogo()
        {
            var file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                var fileName = System.IO.Path.GetFileName(file.FileName);
                var ext = System.IO.Path.GetExtension(fileName);
                var allowedExt = new[] { ".jpg", ".jpeg", ".png", ".gif", ".svg" };
                if (!allowedExt.Contains(ext.ToLower()))
                    return Json(new { success = false, message = "Invalid file type." });
                if (file.ContentLength > 1024 * 1024)
                    return Json(new { success = false, message = "File size exceeds 1MB." });
                var logoFolder = "/UploadedLogos/";
                var serverPath = Server.MapPath(logoFolder);
                if (!System.IO.Directory.Exists(serverPath))
                    System.IO.Directory.CreateDirectory(serverPath);
                var uniqueName = Guid.NewGuid().ToString() + ext;
                var path = System.IO.Path.Combine(serverPath, uniqueName);
                file.SaveAs(path);
                var url = Url.Content(logoFolder + uniqueName);
                return Json(new { success = true, url });
            }
            return Json(new { success = false, message = "No file uploaded." });
        }
    }
}