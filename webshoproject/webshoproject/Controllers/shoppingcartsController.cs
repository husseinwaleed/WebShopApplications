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
    public class shoppingcartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult itemdetails(int id)
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Account"); }
            Session["carid"] = id;
            return RedirectToAction("Details", "Cars");
        }
        public ActionResult GoTOKassa(int id)
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Account"); }
            Session["carid"] = id;
            return RedirectToAction("Create", "Orders");
        }

        // GET: shoppingcarts

        //[Authorize]
        //public ActionResult Index()
        //{

        //    string username = Convert.ToString(Session["user"]);

        //    ApplicationUser user = db.Users.Where(p => p.UserName == username).SingleOrDefault();

        //    Customer customer = db.customers.Where(p => p.email == user.Email).SingleOrDefault();
        //    shoppingcart shoppingcart = db.shoppingcart.Where(p => p.customer.Id == customer.Id).SingleOrDefault();
        //    return View("Details",shoppingcart);
        //}

        //// GET: shoppingcarts/Details/5

        [Authorize]
        public ActionResult Details()
        {
            if (Session["user"] == null) { return RedirectToAction("Login","Account"); }
            string username = Convert.ToString(Session["user"]);
            ApplicationUser user = db.Users.Where(p => p.UserName == username).SingleOrDefault();
            Customer customer = db.customers.Where(p => p.email == user.Email).SingleOrDefault();
            shoppingcart shoppingcart = db.shoppingcart.Where(p => p.customer.Id == customer.Id).SingleOrDefault();
             
         
            if (shoppingcart == null)
            {
                return HttpNotFound();
            }
        
            return View(shoppingcart);
        }
        //[Authorize]
        //// GET: shoppingcarts/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: shoppingcarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id")] shoppingcart shoppingcart)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.shoppingcart.Add(shoppingcart);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(shoppingcart);
        //}




        //[Authorize]
        //// GET: shoppingcarts/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    shoppingcart shoppingcart = db.shoppingcart.Find(id);
        //    if (shoppingcart == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(shoppingcart);
        //}

        // POST: shoppingcarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id")] shoppingcart shoppingcart)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(shoppingcart).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(shoppingcart);
        //}

        // GET: shoppingcarts/Delete/5
        
        [Authorize]
        public ActionResult Deleteitem(int? id )
        {
            if (Session["user"] == null) { return RedirectToAction("Login", "Account"); }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string username = Convert.ToString(Session["user"]);
            ApplicationUser user = db.Users.Where(p => p.UserName == username).SingleOrDefault();
            Customer customer = db.customers.Where(p => p.email == user.Email).SingleOrDefault();
            shoppingcart shoppingcart = db.shoppingcart.Where(p => p.customer.Id == customer.Id).SingleOrDefault();
            if (shoppingcart == null)
            {
                return HttpNotFound();
            }
            Car car = db.cars.Where(p => p.Id == id).SingleOrDefault();
            shoppingcart.car.Remove(car);
            db.SaveChanges();
           
           return RedirectToAction("Details");
        }

        //// POST: shoppingcarts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    shoppingcart shoppingcart = db.shoppingcart.Find(id);
        //    db.shoppingcart.Remove(shoppingcart);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
