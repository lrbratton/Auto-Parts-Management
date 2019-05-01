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


namespace InventoryManagement.Controllers
{
    public class CarController : Controller
    {
        private InventoryContext db = new InventoryContext();

        // GET: Car
        public ActionResult Index()
        {
            var suppliers = db.Cars.Include(d => d.Supplier);
            return View(db.Cars.ToList());
        }

        // GET: Car/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Car/Create
        public ActionResult Create()
        {
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Details");
            return View();
        }

        // POST: Car/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Make,Model,Cost,Acquired,Colour,Chassis,Year,SupplierID")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Details", car.SupplierID);
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WreckCar([Bind(Include = "ID,Make,Model,Cost,Acquired,Colour,Chassis,Year,SupplierID")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Create", "Part");
            }

            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Details", car.SupplierID);
            return View(car);
        }

        // GET: Car/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Details", car.SupplierID);
            return View(car);
        }

        // POST: Car/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Make,Model,Cost,Acquired,Colour,Chassis,Year,SupplierID")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
                
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Details", car.SupplierID);

            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WreckExistingCar([Bind(Include = "ID,Make,Model,Cost,Acquired,Colour,Chassis,Year,SupplierID")] Car car)
        {
            var carID = car.ID;
            return RedirectToAction("Create", "Part", new { id = carID });
        }

        // GET: Car/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.hasParts = "";
            ViewBag.partDetails = "";
            List<Part> carParts = db.Parts.Where(d => d.CarID == id).ToList();
            if(carParts.Count() == 0)
            {
                ViewBag.hasParts = false;
            }
            else
            {
                foreach(var part in carParts)
                {
                    ViewBag.partDetails += part.Details + "<br />";
                }
            }
            return View(car);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
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
    }
}
