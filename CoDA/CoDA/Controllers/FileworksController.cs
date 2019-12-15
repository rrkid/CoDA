using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
namespace CoDA.Controllers
{
    public class FileworksController : Controller
    {
        [HttpGet]
        public ActionResult AddTender()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddTender(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    if (extension == ".xlsx" || extension == ".xls")
                    {
                        file.SaveAs(Server.MapPath("~/Files/LastTenderFile.xlsx"));
                        ViewBag.Message = "Файл \"" + file.FileName.ToString() + "\" успешно загружен!";
                    }
                    else
                        ViewBag.Message = "Файл \"" + file.FileName.ToString() + "\" не загружен! Требуемые расширения : .xls .xlsx";
                }
                else
                    ViewBag.Message = "Файл \"" + file.FileName.ToString() + "\" пуст!";
                return View();
            }
            catch
            {
                ViewBag.Message = "Загрузка файла \"" + file.FileName.ToString() + "\" не произошла!";
                return View();
            }
        }
    }
}