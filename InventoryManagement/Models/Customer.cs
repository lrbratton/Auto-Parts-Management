using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DisplayFormat(NullDisplayText = "No Number")]
        [Display(Name="Phone Number")]
        public string PhNum { get; set; }

        [StringLength(10, MinimumLength = 1)]
        public string StreetNum { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string StreetName { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string Region { get; set; }

        public string PostCode { get; set; }
    }
}