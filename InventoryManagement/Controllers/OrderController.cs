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
        public ActionResult Index()
        {
            ViewBag.partDetail = "";
            var order = db.Order.Include(o => o.Customer);
            return View(order.ToList());
        }

        public void CreatePartDetail(Order order)
        {
            int orderPrice = 0;
            //Create list of part(s) in order
            List<Part> partList = db.Parts.Where(b => b.OrderID == order.ID).ToList();

            //Cycle through parts in order
            foreach (var part in partList)
            {
                //Locate original car
                Car carToFind = db.Cars.Find(part.CarID);

                //Cast part and car details to ViewBag.partDetail
                ViewBag.partDetail += part.Details + " " + carToFind.Details + "<br />";

                //Cast sale price
                orderPrice += part.Price;
            }
            ViewBag.salePrice = "$" + orderPrice.ToString();
            
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
            var orderPrice = new int();
            //Cast PartList session to int list
            List<int> partList = Session["PartList"] as List<int>;
            //Sort through all items in part list
            //Obtain detail for each part
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
            foreach (var i in partList)
            {
                var partToUpdate = db.Parts.Find(i);
                order.Parts.Add(partToUpdate);
                partToUpdate.Sold = true;
                partToUpdate.OrderID = order.ID;
            }
            if (ModelState.IsValid)
            {
                db.Order.Add(order);
                db.SaveChanges();
                PartController.OrderList.Clear();
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
        public ActionResult Edit([Bind(Include = "ID,Date,Paid,CustomerID")] Order order)
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

        public ActionResult ReturnParts(int? orderID, int? partID)
        {
            if (orderID != null)
            {
                Order order = db.Order.Find(orderID);
                return View(order);
            }
            if(partID != null)
            {
                Part part = db.Parts.Find(partID);
                part.Sold = false;
                Order order = db.Order.Where(d => d.ID == part.OrderID).FirstOrDefault();
                order.Parts.Remove(part);
                db.SaveChanges();
                return View(order);
            }

            return RedirectToAction("Index");

        }
    }
}
