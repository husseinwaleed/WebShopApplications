using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webshoproject.Models;

namespace webshoproject.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Account"); }
            return View(db.orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Account"); }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            Car car = new Car();
            OrderRow orderraw = new OrderRow();
            orderraw = db.OrderRows.Include(p => p.car).SingleOrDefault(p => p.order.Id == id);
            ViewBag.orderrawprice = orderraw.price;
            ViewBag.orderrarwfactory = orderraw.car.factory;
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Account"); }
            int carid = Convert.ToInt32(Session["carid"]);
            Car car = db.cars.Find(carid);
            return View(car);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int Id, DateTime orderdate )
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Account"); }
            if (ModelState.IsValid)
            {
                Car car = db.cars.Find(Id);
                string username = Convert.ToString(Session["user"]);
                ApplicationUser user = db.Users.Where(p => p.UserName == username).SingleOrDefault();
                Customer customer = db.customers.Where(p => p.email == user.Email).SingleOrDefault();
                Order neworder = new Order();
                neworder.customer = customer;
                neworder.orderdate = orderdate;
                OrderRow neworderaw = new OrderRow();
                neworderaw.price = car.price;
                neworderaw.car = car;
                neworderaw.order = neworder;
                List<OrderRow> rowlist = new List<OrderRow>();
                rowlist.Add(neworderaw);
                neworder.orderrow = rowlist;
                db.orders.Add(neworder);
                db.OrderRows.Add(neworderaw);
                customer.orderhistory.Add(neworder);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return RedirectToAction("Index","Home");
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Account"); }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,orderdate")] Order order)
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Account"); }
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Account"); }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Account"); }
            Order order = db.orders.Find(id);
            order.orderrow.Clear();
            order.customer = null;
             OrderRow orderraw= db.OrderRows.Include(p => p.order).SingleOrDefault(p => p.order.Id == id);
            db.OrderRows.Remove(orderraw);
            db.orders.Remove(order);
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
