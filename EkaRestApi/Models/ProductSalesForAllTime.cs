using System;
using System.Collections.Generic;

#nullable disable

namespace RestApiCore.Models
{
    public partial class ProductSalesForAllTime
    {
        public long Rowid { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductSales { get; set; }
    }
}
