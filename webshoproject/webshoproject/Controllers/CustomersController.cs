using Microsoft.AspNet.Identity;
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
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.customers.ToList());
        }

        // GET: Customers/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Account"); }
            string username = Convert.ToString(Session["user"]);
            ApplicationUser user = db.Users.Where(p => p.UserName == username).SingleOrDefault();
            Customer customer = db.customers.Where(p => p.email == user.Email).SingleOrDefault();
             
           
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(string firstname,string lastname,string phone ,string deliveryadress,string deliverycity , string billingadress,string billingcity,string email)
        {
            Customer customer = new Customer();
            shoppingcart cart = new shoppingcart();
            customer.firstname = firstname;customer.lastname = lastname;customer.phone = phone;customer.deliveryadress = deliveryadress;
            customer.deliverycity = deliverycity;customer.billingadress = billingadress;customer.billingcity = billingcity;customer.email = email;
          

            db.customers.Add(customer);
            cart.customer = customer;
            db.shoppingcart.Add(cart);
            
                db.SaveChanges();
            return RedirectToAction("Index", "Home");

        }

        // GET: Customers/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,firstname,lastname,phone,deliveryadress,deliverycity,billingadress,billingcity,email,user,messagecounter")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        [Authorize]
        public ActionResult orderhistory()
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Account"); }
            string username = Convert.ToString(Session["user"]);
            ApplicationUser user = db.Users.Where(p => p.UserName == username).SingleOrDefault();
            Customer customer = db.customers.Where(p => p.email == user.Email).SingleOrDefault();
           
            if (customer == null)
            {
                return HttpNotFound();
            }

            return View (customer.orderhistory);
        }





        // GET: Customers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }





        // POST: Customers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.customers.Find(id);
            db.customers.Remove(customer);
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
