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

namespace InventoryManagement.Controllers
{
    public class OrderController : Controller
    {
        private InventoryContext db = new InventoryContext();

        // GET: Order
        public ActionResult Index(string sortOrder, string customerSearch)
        {
            ViewBag.partDetail = "";
            var order = db.Order.Include(o => o.Customer);

            ViewBag.DateSort = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.PaidSort = sortOrder == "Paid" ? "paid_desc" : "Paid";
            ViewBag.CustomerSort = sortOrder == "Customer" ? "cust_desc" : "Customer";

            if(!String.IsNullOrEmpty(customerSearch))
            {
                order = order.Where(c => c.Customer.Name.Contains(customerSearch));
            }
            switch(sortOrder)
            {
                case "Date":
                    order = order.OrderByDescending(c => c.Date);
                    break;
                case "Paid":
                    order = order.OrderBy(c => c.Paid);
                    break;
                case "paid_desc":
                    order = order.OrderByDescending(c => c.Paid);
                    break;
                case "Customer":
                    order = order.OrderBy(c => c.Customer.Name);
                    break;
                case "cust_desc":
                    order = order.OrderByDescending(c => c.Customer.Name);
                    break;
                default:
                    order = order.OrderByDescending(c => c.Date);
                    break;
            }
            ModelState.Clear();
            return View(order.ToList());
        }

        

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            //Obtain ViewBag data
            CreatePartDetail(order);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            //Variable to store total order price
            var orderPrice = new double();
            //Cast PartList session to int list
            List<int> partList = Session["PartList"] as List<int>;
            //Sort through all items in part list
            //Obtain detail for each part
            if (partList == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foreach (var i in partList)
            {
                Part part = db.Parts.Find(i);
                Car car = db.Cars.Find(part.CarID);
                //Cast to ViewBag so user can verify order details
                ViewBag.part += car.Details + " " + part.Details + " " + "<br/>";
                orderPrice += part.Price;
            }

            //Cast orderPrice variable to ViewBag
            ViewBag.orderPrice = "$" + orderPrice.ToString();
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "Name");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Date,Paid,CustomerID")] Order order)
        {
            List<int> partList = Session["PartList"] as List<int>;
            order.Parts = new List<Part>();
            //Check if partList has been cleared
            if(partList == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Add parts to order, update part to match
            foreach (var i in partList)
            {
                var partToUpdate = db.Parts.Find(i);
                order.Parts.Add(partToUpdate);
                partToUpdate.Status = "Sold";
                partToUpdate.OrderID = order.ID;
            }
            if (ModelState.IsValid)
            {
                db.Order.Add(order);
                db.SaveChanges();
                //Clear orderlist, and set session to null
                PartController.OrderList.Clear();
                Session["PartList"] = null;
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "Name", order.CustomerID);
            return View(order);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "Name", order.CustomerID);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Paid,CustomerID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "Name", order.CustomerID);
            return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            CreatePartDetail(order);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Order.Find(id);
            db.Order.Remove(order);
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

        public ActionResult CreateNewCustomer()
        {
            return RedirectToAction("Create", "Customer");
        }

        //Allow user to cancel order on create page - redirect to part index and clear cart
        public ActionResult CancelOrder()
        {
            PartController.OrderList.Clear();
            return RedirectToAction("Index", "Part");
        }

        //Allow user to return parts
        //Will pass a orderID when called from the Index page, redirecting to ReturnParts view
        //Will pass a partID when called from ReturnParts View
        public ActionResult ReturnParts(int? orderID, int? partID, bool? partDefective)
        {
            if (orderID != null)
            {
                //Find order from ID
                Order order = db.Order.Find(orderID);
                //Return ReturnParts view with found order as context
                return View(order);
            }
            if(partID != null)
            {
                //Find part, mark status as returned
                Part part = db.Parts.Find(partID);
                if(partDefective == true)
                {
                    part.Status = "Defective";
                }
                else { 
                    part.Status = "Available";
                }
                //Find order and remove returned part

                Order order = db.Order.Where(d => d.ID == part.OrderID).FirstOrDefault();
                order.Parts.Remove(part);

                //Add a record of removed parts
                Car car = db.Cars.Find(part.CarID);
                order.OrderHistory += (part.Area + " " + part.Car.Make + " " + 
                                    part.Car.Model + " " + part.Name + "</ br>");
                db.SaveChanges();
                
                if(order.Parts.Count() == 0)
                    //No more parts in order, redirect to order index
                {
                    return RedirectToAction("Index");
                }
                return View(order);
            }

            return RedirectToAction("Index");

        }

        //Method to mark an order as paid
        public ActionResult MarkPaid(int? orderID)
        {
            //Check orderID
            if(orderID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            //Find order in db and mark paid as true
            Order order = db.Order.Find(orderID);
            order.Paid = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Helper to fetch part information pertaining to order
        public void CreatePartDetail(Order order)
        {
            int orderPrice = 0;
            //Create list of part(s) in order
            List<Part> partList = db.Parts.Where(b => b.OrderID == order.ID).ToList();
            //Cycle through parts in order
            foreach (var part in partList)
            {
                //Cast part and car details to ViewBag.partDetail
                ViewBag.partDetail += part.Details + " " + part.Car.Details + "<br />";

                //Cast sale price
                orderPrice += part.Price;
            }
            ViewBag.salePrice = "$" + orderPrice.ToString();
        }
    }
}
