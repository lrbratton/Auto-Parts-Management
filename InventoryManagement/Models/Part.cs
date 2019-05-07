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

    public class Part
    {
        //Primary key
        public int PartID { get; set; }

        //Part name
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        //Condition of part
        [StringLength(50, MinimumLength = 1)]
        public string Condition { get; set; }

        //Location of part on car
        [Display(Name="Area of Car")]
        public AreaFrom Area { get; set; }

        /// <summary>
        /// Part status
        /// This will be limited to a few options:
        /// Available, sold, returned, and defective
        /// Leaving as string for more options i.e. reasons for defective, or returned
        /// </summary>
        public string Status { get; set; }

        //Price of part
        [DataType(DataType.Currency)]
        [Column(TypeName="money")]
        public int Price { get; set; }

        //ID of Car the part originated from
        public int? CarID { get; set; }

        //ID of Order (only used when part is sold)
        public int? OrderID { get; set; }


        public virtual Car Car { get; set; }

        public Order PartOrder { get; set; }
        

        public string Details
        {
            get
            {
                return Area + " " + Name + " | Condition: " + Condition + " | $" + Price;
            }
        }
    }
}