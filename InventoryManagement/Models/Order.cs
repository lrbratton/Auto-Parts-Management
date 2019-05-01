using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class Order
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name ="Date Sold")]
        public DateTime Date { get; set; }

        public bool Paid { get; set; }

        public ICollection<int> PartID { get; set; }
        public int CustomerID { get; set; }

        public virtual ICollection<Part> Parts { get; set; }
        public virtual Customer Customer { get; set; }
    }
}