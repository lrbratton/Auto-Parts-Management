using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public enum AreaFrom
    {
        FR, FL, F, RR, LR, R, NA
    }

    public enum Status
    {
        Available, Reserved, Returned, Sold
    }
    public class Part
    {
        
        public int PartID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string Condition { get; set; }

        [Display(Name="Area of Car")]
        public AreaFrom Area { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName="money")]
        public int Price { get; set; }

        public bool Sold { get; set; }

        public int? CarID { get; set; }

        public virtual Car Car { get; set; }

        public Order PartOrder { get; set; }
        public int? OrderID { get; set; }

        public string Details
        {
            get
            {
                return Area + " " + Name + " Condition: " + Condition + " $" + Price;
            }
        }
    }
}