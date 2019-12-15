using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoDA.Models;
using System.Data.Entity.Validation;
using CoDA.Helpers;

namespace CoDA.DAL
{
    public class CoDAInitializer : System.Data.Entity.DropCreateDatabaseAlways<CoDAContext> //IfModelChanges
    {
        protected override void Seed(CoDAContext context)
        {
            context.Warehouses.Add(new Warehouse { Id = 1, Name = "Сепротин 500 МЕ", Amount = 5000, Price = 49870.44, PackageType = "Коробка", ProductionCode = "123", Unit = "Шт.", Weight = 0.0001 });
            context.Warehouses.Add(new Warehouse { Id = 2, Name = "АТ 1000", Amount = 5000, Price = 48740.65, PackageType = "Пакет", ProductionCode = "456", Unit = "Шт.", Weight = 0.0005 });
            context.Warehouses.Add(new Warehouse { Id = 3, Name = "АТ 500", Amount = 5000, Price = 24374.53, PackageType = "Пакет", ProductionCode = "789", Unit = "Шт.", Weight = 0.00025 });
            context.Warehouses.Add(new Warehouse { Id = 4, Name = "Фейба 1000", Amount = 5000, Price = 48781.35, PackageType = "Коробка", ProductionCode = "098", Unit = "Шт.", Weight = 0.0004 });
            context.Warehouses.Add(new Warehouse { Id = 5, Name = "Фейба 500", Amount = 5000, Price = 24384.03, PackageType = "Коробка", ProductionCode = "765", Unit = "Шт.", Weight = 0.0002 });
            context.Warehouses.Add(new Warehouse { Id = 6, Name = "ПРОТРОМПЛЕКС 600", Amount = 5000, Price = 28525.07, PackageType = "Пакет", ProductionCode = "432", Unit = "Шт.", Weight = 0.0002 });
            context.Warehouses.Add(new Warehouse { Id = 7, Name = "Октофактор 500 МЕ", Amount = 5000, Price = 5581.1, PackageType = "Пакет", ProductionCode = "1009", Unit = "Шт.", Weight = 0.0001 });
            context.Warehouses.Add(new Warehouse { Id = 8, Name = "Октофактор 1000 МЕ", Amount = 5000, Price = 11044.6, PackageType = "Пакет", ProductionCode = "3429", Unit = "Шт.", Weight = 0.0002 });
            context.Warehouses.Add(new Warehouse { Id = 9, Name = "Октофактор 2000 МЕ", Amount = 5000, Price = 20580, PackageType = "Пакет", ProductionCode = "9329", Unit = "Шт.", Weight = 0.0004 });
            context.Warehouses.Add(new Warehouse { Id = 10, Name = "Коагил 1,2", Amount = 5000, Price = 26068, PackageType = "Коробка", ProductionCode = "4583", Unit = "Шт.", Weight = 0.00015 });
            context.Warehouses.Add(new Warehouse { Id = 11, Name = "Коагил 2,4", Amount = 5000, Price = 51744, PackageType = "Коробка", ProductionCode = "8739", Unit = "Шт.", Weight = 0.0003 });
            context.Warehouses.Add(new Warehouse { Id = 12, Name = "Коагил 4,8", Amount = 5000, Price = 103586, PackageType = "Коробка", ProductionCode = "1023", Unit = "Шт.", Weight = 0.0006 });
            context.Warehouses.Add(new Warehouse { Id = 13, Name = "Иннонафактор 250", Amount = 5000, Price = 1749.3, PackageType = "Пакет", ProductionCode = "6984", Unit = "Шт.", Weight = 0.00025 });
            context.Warehouses.Add(new Warehouse { Id = 14, Name = "Иннонафактор 500", Amount = 5000, Price = 3498.6, PackageType = "Пакет", ProductionCode = "4390", Unit = "Шт.", Weight = 0.0005 });
            context.Warehouses.Add(new Warehouse { Id = 15, Name = "Иннонафактор 1000", Amount = 5000, Price = 6997.2, PackageType = "Пакет", ProductionCode = "1122", Unit = "Шт.", Weight = 0.001 });

            context.OrderInfos.Add(new OrderInfo
            {
                Id = 1,
                Date = "13.09.2019",
                PreShipmentDate = "13.10.2019",
                Status = "InProgress",
                CustomerName = "MinpromTorg",
                CustomerLocationArea = "Area of Moscow",
                CustomerCity = "Podolsk"
            });

            double price = 26068;
            int amount = 100;
            context.Preparations.Add(new Preparation
            {
                Id = 1,
                OrderInfoId = 1,
                Name = "Коагил 1,2",
                Amount = amount,
                ExpirationDate = "25.09.2025",
                Total = MoneyWorks.GetTotal(amount, price),
                TotalVAT = MoneyWorks.GetTotalVAT(amount, price),
                PaymentDate = "01.09.2019"
            });

            price = 6997.2;
            amount = 200;
            context.Preparations.Add(new Preparation
            {
                Id = 2,
                OrderInfoId = 1,
                Name = "Иннонафактор 1000",
                Amount = amount,
                ExpirationDate = "12.05.2023",
                Total = MoneyWorks.GetTotal(amount, price),
                TotalVAT = MoneyWorks.GetTotalVAT(amount, price),
            });

            context.Distributors.Add(new Distributor { Id = 1, Name = "SIA" });

            context.AuctionInfos.Add(new AuctionInfo { Id = 1, AuctionNumber = "1", Date = "13.11.2019", Status = "InProgress" });

            context.Auctions.Add(new Auction { Id = 1, AuctionInfoId = 1 });
            context.ShipmentInfos.Add(new ShipmentInfo { Id = 1, Date = "13.12.2019", DistributorId = 1, Status = "InProgress" });
            context.Shipments.Add(new Shipment { Id = 1, ShipmentInfoId = 1 });
            context.MainOrders.Add(new MainOrder { Id = 1, OrderInfoId = 1, AuctionId = 1, ShipmentId = 1 });
            //base.Seed(context);
            try
            {
                context.SaveChanges();

            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                {
                    string s1 = validationError.Entry.Entity.ToString();
                    foreach (DbValidationError err in validationError.ValidationErrors)
                    {
                        string s2 = err.ErrorMessage;
                    }
                }
            }
        }
    }
}