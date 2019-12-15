using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoDA.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CoDA.DAL
{
    public class CoDAContext : DbContext
    {
        static CoDAContext()
        {
            Database.SetInitializer<CoDAContext>(new CoDAInitializer());
        }
        public CoDAContext() : base("CoDAContext")
        {
        }
        public DbSet<OrderInfo> OrderInfos { get; set; }
        public DbSet<Preparation> Preparations { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<AuctionInfo> AuctionInfos { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<ShipmentInfo> ShipmentInfos { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<MainOrder> MainOrders { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}