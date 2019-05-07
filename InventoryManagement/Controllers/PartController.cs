using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryManagement.DAL;
using InventoryManagement.Models;
using System.Data.Entity.Infrastructure;
using System.Web.UI;


namespace InventoryManagement.Controllers
{
    public class PartController : Controller
    {
        private InventoryContext db = new InventoryContext();
        //List to contain database references of parts in order
        public static List<int> OrderList = new List<int>();

        // GET: Part
        public ViewResult Index(string sortOrder, string nameSearch, string makeSearch,
                                string modelSearch, string chassisSearch)
        {
            //Logic to find and display order related information
            //Fetch number of parts in order
            ViewBag.partCount = OrderList.Count();
            //Fetch details of each item in order
            foreach (var i in OrderList)
            {
                Part part = db.Parts.Find(i);
                Car car = db.Cars.Find(part.CarID);
                ViewBag.partDetail += car.Details + " " + part.Details + "<br/>";
            }
            ViewBag.orderedParts = OrderList;

            var cars = db.Parts.Include(d => d.Car);
            var parts = from p in db.Parts select p;
            //Iterate through search criteria
            //Allows users the flexibility of searching per category, but unsure if this is the most efficient method
            if (!String.IsNullOrEmpty(nameSearch))
            {
                parts = parts.Where(p => p.Name.Contains(nameSearch)); 
            }
            if (parts.Count() > 1 && !String.IsNullOrEmpty(makeSearch))
            {
                parts = parts.Where(p => p.Car.Make.Contains(makeSearch));
            }
            if (parts.Count() > 1 && !String.IsNullOrEmpty(modelSearch))
            {
                parts = parts.Where(p => p.Car.Model.Contains(modelSearch));
            }
            if (parts.Count() > 1 && !String.IsNullOrEmpty(chassisSearch))
            {
                parts = parts.Where(p => p.Car.Chassis.Contains(chassisSearch));
            }
            //Code for sort order
            //Allows user to sort by name, price, or status
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSort = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.StatusSort = sortOrder == "Status" ? "status_desc" : "Status";

            switch (sortOrder) {
                case "name_desc":
                    parts = parts.OrderByDescending(p => p.Name);
                    break;
                case "Price":
                    parts = parts.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    parts = parts.OrderByDescending(p => p.Price);
                    break;
                case "Status":
                    parts = parts.OrderBy(p => p.Status);
                    break;
                case "status_desc":
                    parts = parts.OrderByDescending(p => p.Status);
                    break;
                default:
                    parts = parts.OrderBy(p => p.Name);
                    break;
            }
            ModelState.Clear();
            return View(parts.ToList());
        }

        // GET: Part/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.Parts.Find(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            //Obtain sale information if part is sold
            if (part.Status == "Sold")
            {
                //Find order and respective customer, pass these to UI for user
                Order order = db.Order.Where(d => d.ID == part.OrderID).FirstOrDefault();
                Customer customer = db.Customer.Where(d => d.CustomerID == order.CustomerID).FirstOrDefault();
                ViewBag.customerDetail = customer.Name;
                ViewBag.saleDate = order.Date;
                ViewBag.orderNumber = order.ID;
            }
            return View(part);
        }

        // GET: Part/Create
        public ActionResult Create(int? ID)
        {
            SelectList carList;
            //Check if method has a car ID to reference
            if(ID != null)
            {
                //Yes, display preselected car as default option
                carList = new SelectList(db.Cars, "ID", "Details", ID);
            }
            else
            {
                //No, display default list
                carList = new SelectList(db.Cars, "ID", "Details");
            }
            ViewBag.CarID = carList;
            return View();
        }

        // POST: Part/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PartID,Name,Condition,Area,Price,CarID")] Part part)
        {
            if (ModelState.IsValid)
            {
                db.Parts.Add(part);
                part.Status = "Available";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.CarID = new SelectList(db.Cars, "ID", "Details", part.CarID);
            return View(part);
        }

        // POST: Add another part, using the same CarID
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPart([Bind(Include = "PartID,Name,Condition,Area,Price,CarID")] Part part, int? id)
        {
            if (ModelState.IsValid)
            {
                db.Parts.Add(part);
                part.Status = "Available";
                db.SaveChanges();
                return RedirectToAction("Create", new {id = part.CarID});
            }

            ViewBag.CarID = new SelectList(db.Cars, "ID", "Details", part.CarID);
            return View(part);
        }

        // GET: Part/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.Parts.Find(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarId = new SelectList(db.Cars, "ID", "Details", part.CarID);
            return View(part);
        }

        // POST: Part/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PartID,Name,Condition,Area,Price,CarID,Status")] Part part)
        {
            if (ModelState.IsValid)
            {
                db.Entry(part).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarID = new SelectList(db.Cars, "ID", "Details", part.CarID);
            return View(part);
        }

        // GET: Part/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.Parts.Find(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            if (OrderList.Contains(part.PartID))
            {
                TempData["buttonError"] = "Cannot delete item. Item currently in cart. Please clear cart to continue.";
                return RedirectToAction("Index");
            }
            return View(part);
        }

        // POST: Part/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Part part = db.Parts.Find(id);
            db.Parts.Remove(part);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //Methods for updating current order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOrder(int id)
        {
            Part part = db.Parts.Find(id);
            if (OrderList.Contains(id))
            {
                TempData["buttonError"] = "Error: This item already exists in order.";
                return RedirectToAction("Index");
            }
            TempData["buttonError"] = "";
            OrderList.Add(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromOrder(int id)
        {
            OrderList.Remove(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessOrder()
        {
            Session["PartList"] = OrderList;
            //OrderList.Clear();
            return RedirectToAction("Create", "Order");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SellPart(Part part, int id)
        {

            //if (id == null)
            //{
            //    return RedirectToAction("Create", "Order", new { partID = part.ID });
            //}
            //else
            //{
            OrderList.Add(id);
            Session["PartList"] = OrderList;
            //OrderList.Clear();
            return RedirectToAction("Create", "Order");
            //}

        }
    }
}
