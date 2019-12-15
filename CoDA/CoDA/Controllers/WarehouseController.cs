using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoDA.DAL;
using CoDA.Models;
namespace CoDA.Controllers
{
    public class WarehouseController : Controller
    {
        CoDAContext db = new CoDAContext();
        [HttpGet]
        public ActionResult AddPreparation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPreparation(Warehouse preparationsBase)
        {
            db.Warehouses.Add(preparationsBase);
            db.SaveChanges();
            return RedirectToAction("LookPreparationBase");
        }

        [HttpGet]
        public ActionResult DeletePreparation(int id)
        {
            Warehouse b = db.Warehouses.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return View(b);
        }
        [HttpPost, ActionName("DeletePreparation")]
        public ActionResult DeleteConfirmed(int id)
        {
            Warehouse b = db.Warehouses.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            db.Warehouses.Remove(b);
            db.SaveChanges();
            return RedirectToAction("LookPreparationBase");
        }

        [HttpGet]
        public ActionResult EditPreparation(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Warehouse preparationsBase = db.Warehouses.Find(id);
            if (preparationsBase != null)
            {
                return View(preparationsBase);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditPreparation(Warehouse preparationsBase)
        {
            db.Entry(preparationsBase).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("LookPreparationBase");
        }

        public ActionResult LookPreparationBase()
        {
            IEnumerable<Warehouse> preparations = db.Warehouses;
            ViewBag.Warehouses = preparations;
            return View();
        }
    }
}