using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoDA.DAL;
using CoDA.Models;
using System.IO;
using System.IO.Packaging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CoDA.Helpers
{
    public class FileHelper
    {
        private string path;
        private string pathTender = "LastTenderFile.xlsx";
        public int error { get; private set; }
        public FileHelper(HttpPostedFileBase file, string path, CoDAContext db)
        {
            this.path = path;
            Int32 strLen = Convert.ToInt32(file.InputStream.Length);
            byte[] byteArr = new byte[strLen];     // Create a byte array.
            Int32 strRead = file.InputStream.Read(byteArr, 0, strLen);    // Read stream into byte array.
            string ext = Path.GetExtension(file.FileName);
            this.path += "OrderFile" + ext;
            this.pathTender = path + this.pathTender;
            File.WriteAllBytes(this.path, byteArr);
            ExcelWorks(db);
        }

        private void ExcelWorks(CoDAContext db)
        {
            SpreadsheetDocument doc = SpreadsheetDocument.Open(this.path, false);
            WorkbookPart workbookPart = doc.WorkbookPart;
            WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            int maxrows = sheetData.Elements<Row>().Count() - 3;
            int maxcols = sheetData.Elements<Row>().First().Elements<Cell>().Count();
            if (maxcols != 11) //неправильное кол-во колонок в файле
            {
                this.error = -1;
                return;
            }
            List<string> array = new List<string>();
            var row = sheetData.Elements<Row>().Skip(1);
            foreach (Cell c in row.First())
            {
                if (c.CellValue != null)
                    array.Add(getCellValue(c, workbookPart));
                else
                    array.Add("null");
            }
            List<List<string>> preparationsinfo = new List<List<string>>();
            for (int i = 0; i < maxrows; i++)
            {
                preparationsinfo.Add(new List<string>());
                for (int j = 2; j < 5; j++)
                {
                    preparationsinfo[i].Add(getCellValue(row.ElementAt(i).Elements<Cell>().ElementAt(j), workbookPart));
                }
            }

            SpreadsheetDocument tender = SpreadsheetDocument.Open(this.pathTender, false);
            WorkbookPart wbPartTender = tender.WorkbookPart;
            WorksheetPart wsPartTender = wbPartTender.WorksheetParts.First();
            SheetData sheetDataTender = wsPartTender.Worksheet.Elements<SheetData>().First();
            row = sheetDataTender.Elements<Row>().Skip(1);
            foreach (Row r in row)
            {
                string val1 = getCellValue(r.Elements<Cell>().First(), wbPartTender);
                string val2 = getCellValue(r.Elements<Cell>().Last(), wbPartTender);
                if (val1 == array[0] && val2 == array[9])
                {
                    AddRecord(db, array, preparationsinfo);
                    break;
                }
            }

        }
        private string getCellValue(Cell currentcell, WorkbookPart wbPart)
        {
            string currentcellvalue = string.Empty;
            if (currentcell.DataType != null)
            {
                if (currentcell.DataType == CellValues.SharedString)
                {
                    int id = -1;

                    if (Int32.TryParse(currentcell.InnerText, out id))
                    {
                        SharedStringItem item = GetSharedStringItemById(wbPart, id);

                        if (item.Text != null)
                        {
                            //code to take the string value  
                            currentcellvalue = item.Text.Text;
                        }
                        else if (item.InnerText != null)
                        {
                            currentcellvalue = item.InnerText;
                        }
                        else if (item.InnerXml != null)
                        {
                            currentcellvalue = item.InnerXml;
                        }
                    }
                }
            }
            return currentcellvalue;
        }
        private static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }

        private void AddRecord(CoDAContext db, List<string> array, List<List<string>> preparationsinfo)
        {
            int orderInfoId = db.OrderInfos.Max(u => u.Id) + 1;
            List<string> namelst = (from warehouse in db.Warehouses
                                    select warehouse.Name).ToList();
            List<double> pricelst = (from warehouse in db.Warehouses
                                     select warehouse.Price).ToList();
            
            double price = -1;
            for (int i = 0; i < preparationsinfo.Count; i++)
            {
                for (int j = 0; j < namelst.Count; j++)
                {
                    if (namelst[j] == preparationsinfo[i][0])
                    {
                        price = pricelst[i];
                        break;
                    }
                }
                if (price == -1)
                {
                    this.error = -2; //нет такого препарата
                    return;
                }
                int amount = Int32.Parse(preparationsinfo[i][1].Split('.')[0]);
                db.Preparations.Add(new Models.Preparation // check for errors
                {
                    Name = preparationsinfo[i][0],
                    Amount = amount,
                    ExpirationDate = preparationsinfo[i][2],
                    OrderInfoId = orderInfoId,
                    PaymentDate = "none",// status maybe?
                    Total = MoneyWorks.GetTotal(amount, price),
                    TotalVAT = MoneyWorks.GetTotalVAT(amount, price)
                });
            }

            int distributorId = db.Distributors.Max(u => u.Id) + 1;
            int auctionInfoId = db.AuctionInfos.Max(u => u.Id) + 1;
            int shipmentInfoId = db.ShipmentInfos.Max(u => u.Id) + 1;
            
            int shipmentId = db.Shipments.Max(u => u.Id) + 1;
            int auctionId = db.Auctions.Max(u => u.Id) + 1;
            int orderId = db.MainOrders.Max(u => u.Id) + 1;

            db.Distributors.Add(new Models.Distributor
            {
                Id = distributorId,
                Name = array[0]
            });

            db.OrderInfos.Add(new Models.OrderInfo  // check for errors
            {
                Id = orderInfoId,
                Date = array[1].Split(' ')[0],
                PreShipmentDate = array[5].Split(' ')[0],
                CustomerName = array[6],
                CustomerLocationArea = array[7],
                CustomerCity = array[8],
                Status = "InProgress"
            });

            db.AuctionInfos.Add(new Models.AuctionInfo
            {
                Id = auctionInfoId,
                AuctionNumber = array[9],
                Date = array[10].Split(' ')[0],
                Status = "OK"
            });


            db.ShipmentInfos.Add(new Models.ShipmentInfo
            {
                Id = shipmentInfoId,
                Date = array[5].Split(' ')[0],
                Status = "InProgress",
                DistributorId = distributorId
            });


            db.Shipments.Add(new Models.Shipment
            {
                Id = shipmentId,
                ShipmentInfoId = shipmentInfoId
            });


            db.Auctions.Add(new Models.Auction
            {
                Id = auctionId,
                AuctionInfoId = auctionInfoId
            });

            db.MainOrders.Add(new Models.MainOrder
            {
                Id = orderId,
                OrderInfoId = orderInfoId,
                ShipmentId = shipmentId,
                AuctionId = auctionId
            });

            db.SaveChanges();
        }
    }
}