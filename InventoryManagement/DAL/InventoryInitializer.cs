using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.DAL
{
    public class InventoryInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<InventoryContext>
    {
        protected override void Seed(InventoryContext context)
        {
            var suppliers = new List<Supplier>
            {
                new Supplier{Name="Turners",Location="Auckland"}
            };
            suppliers.ForEach(s => context.Suppliers.Add(s));
            context.SaveChanges();

            var cars = new List<Car>
            {
                new Car{Make="Subaru", Model="Impreza", Cost=int.Parse("1200"),
                    Colour="Red", Chassis="GG2",
                    Year=int.Parse("2004")}
            };
            cars.ForEach(s => context.Cars.Add(s));
            context.SaveChanges();

            var parts = new List<Part>
            {
                new Part{Name="Wing Mirror"}
            };
        }
    }
}