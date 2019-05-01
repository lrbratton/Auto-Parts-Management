using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class Supplier
    {
        public int ID { get; set; }

        [StringLength(50, MinimumLength=2)]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Location { get; set; }

        public string Details
        {
            get
            {
                return Name + ", " + Location;
            }
        }
    }
}