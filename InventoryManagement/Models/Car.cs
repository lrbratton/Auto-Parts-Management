using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class Car
    {
        public int ID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Make { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Model { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public int Cost { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Colour { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Chassis { get; set; }

        [Range(1800, 2100)]
        [Display(Name ="Vehicle Year")]
        public int Year { get; set; }

        public int? SupplierID { get; set; }

        public virtual Supplier Supplier { get; set; }

        public string Details
        {
            get
            {
                return Make + " " + Model + " " + Chassis + " " + Colour + " " + Year;
            }
        }
    }
}