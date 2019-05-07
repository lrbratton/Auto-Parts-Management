using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class Order
    {
        //Primary Key
        public int ID { get; set; }

        //Date part was sold
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name ="Date Sold")]
        public DateTime Date { get; set; }

        //Has the order been paid for, or will payment come later
        public bool Paid { get; set; }

        //Collection of PartId's belonging to order - unsure if necessary
        public ICollection<int> PartID { get; set; }

        //Store history of parts, update when a part is removed from order
        public string OrderHistory { get; set; }

        //Foreign Key
        public int CustomerID { get; set; }

        //Collection of Parts in order
        public virtual ICollection<Part> Parts { get; set; }

        //Customer object
        public virtual Customer Customer { get; set; }
    }
}