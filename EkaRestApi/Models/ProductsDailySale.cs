using System;
using System.Collections.Generic;

#nullable disable

namespace RestApiCore.Models
{
    public partial class ProductsDailySale
    {
        public long Rowid { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ProductName { get; set; }
        public double? DailySales { get; set; }
    }
}
