using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoDA.DAL;
using CoDA.Models;
using CoDA.Helpers;

namespace CoDA.Controllers
{
    public class SearchController : Controller
    {
        CoDAContext db = new CoDAContext();
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchInfo(string str)
        {
            var allinfo = (from order in db.MainOrders
                           join auction in db.Auctions on order.AuctionId equals auction.Id
                           join auctioninfo in db.AuctionInfos on auction.AuctionInfoId equals auctioninfo.Id
                           join orderinfo in db.OrderInfos on order.OrderInfoId equals orderinfo.Id
                           join shipment in db.Shipments on order.ShipmentId equals shipment.Id
                           join shipmentinfo in db.ShipmentInfos on shipment.ShipmentInfoId equals shipmentinfo.Id
                           join distributor in db.Distributors on shipmentinfo.DistributorId equals distributor.Id
                           where auctioninfo.AuctionNumber == str
                           select new
                           {
                               AuctionNumber = auctioninfo.AuctionNumber,
                               AuctionDate = auctioninfo.Date,
                               DistributorName = distributor.Name,
                               OrderId = orderinfo.Id.ToString(),
                               OrderDate = orderinfo.Date,
                               OrderPreShipmentDate = orderinfo.PreShipmentDate,
                               CustomerName = orderinfo.CustomerName,
                               CustomerLocationArea = orderinfo.CustomerLocationArea,
                               CustomerCity = orderinfo.CustomerCity,
                               OrderStatus = orderinfo.Status,
                               ShipmentDate = shipmentinfo.Date,
                               ShipmentStatus = shipmentinfo.Status,
                               OrderInfoId = orderinfo.Id
                           }).ToList();
            if (allinfo.Count <= 0)
            {
                return HttpNotFound();
            }

            int orderinfoid = allinfo[0].OrderInfoId;
            var allpreparations = db.Preparations.Where(a => a.OrderInfoId == orderinfoid).ToList();


            //для обхода анонимного типа IQueryble<'a>
            AllInfo info = new AllInfo(allinfo[0].AuctionNumber, allinfo[0].AuctionDate, allinfo[0].DistributorName, allinfo[0].OrderId,
                allinfo[0].OrderDate, allinfo[0].OrderPreShipmentDate, allinfo[0].CustomerName, allinfo[0].CustomerLocationArea,
                allinfo[0].CustomerCity, allinfo[0].OrderStatus, allinfo[0].ShipmentDate, allinfo[0].ShipmentStatus, allpreparations);
            var kek = (new[] { info }).ToList();

            return PartialView(kek);
        }

        [HttpGet]
        public ActionResult EditOrderInfo(int? id)
        {
            if (id == null)
                return HttpNotFound();
            OrderInfo orderInfo = db.OrderInfos.Find(id);
            if (orderInfo != null)
            {
                return View(orderInfo);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditOrderInfo(OrderInfo orderInfo)
        {
            db.Entry(orderInfo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Search/" + orderInfo.Id.ToString());
        }

        [HttpGet]
        public ActionResult EditShipmentInfo(int? id)
        {
            if (id == null)
                return HttpNotFound();
            ShipmentInfo shipmentInfo = db.ShipmentInfos.Find(id);
            if (shipmentInfo != null)
            {
                return View(shipmentInfo);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditShipmentInfo(ShipmentInfo shipmentInfo)
        {
            db.Entry(shipmentInfo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            //string status = shipmentInfo.Status; //удаление из склада
            //if (status == "Complete")
            return RedirectToAction("Search");
        }

        [HttpGet]
        public ActionResult EditPaymentDate(int? id)
        {
            if (id == null)
                return HttpNotFound();
            Preparation preparation = db.Preparations.Find(id);
            if (preparation != null)
            {
                return View(preparation);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditPaymentDate(Preparation preparation)
        {
            db.Entry(preparation).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Search");
        }
    }
}