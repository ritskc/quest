using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Part
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public int UnitId { get; set; }
        public string Unit { get; set; }
        public int CompanyId { get; set; }
        public int WarehouseId { get; set; }
        public int MinQty { get; set; }
        public int MaxQty { get; set; }
        public int OpeningQty { get; set; }             
        public bool IsActive { get; set; }       
        public string Location { get; set; }
        public int IntransitQty { get; set; }
        public int QtyInHand { get; set; }       
        public decimal SupplierPrice { get; set; }
        public decimal CustomerPrice { get; set; }
        public string SKU { get; set; }
        public bool IsSurcharge { get; set; }
        public decimal SurchargePercentage { get; set; }
        public decimal SurchargeAmount { get; set; }
       
    }
}
