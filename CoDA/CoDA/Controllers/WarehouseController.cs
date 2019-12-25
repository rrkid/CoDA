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
        [Authorize]
        public ActionResult AddPreparation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPreparation(Warehouse warehouse)
        {
            db.Warehouses.Add(warehouse);
            db.SaveChanges();
            return RedirectToAction("LookWarehouse");
        }

        [HttpGet]
        [Authorize]
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
            return RedirectToAction("LookWarehouse");
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditPreparation(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Warehouse warehouse = db.Warehouses.Find(id);
            if (warehouse != null)
            {
                return View(warehouse);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditPreparation(Warehouse warehouse)
        {
            db.Entry(warehouse).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("LookWarehouse");
        }

        public ActionResult LookWarehouse()
        {
            IEnumerable<Warehouse> preparations = db.Warehouses;
            ViewBag.Warehouses = preparations;
            return View();
        }
    }
}