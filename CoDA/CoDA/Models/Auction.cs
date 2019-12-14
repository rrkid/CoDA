using System;
using System.Collections.Generic;

namespace CoDA.Models
{
    public class Auction
    {
        public int Id { get; set; }
        public int? AuctionInfoId { get; set; }

        public AuctionInfo AuctionInfo { get; set; }
    }
}