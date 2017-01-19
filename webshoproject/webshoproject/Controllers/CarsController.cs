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
    public class carangular
    {
       public int id { get; set; }
         public string carmodel { get; set; }
        public   string carfactory { get; set; }
        public string cartype { get; set; }
        public string carcolor { get; set; }
        public int carprice { get; set; }

    }
    public class CarsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Cars
        public ActionResult Index()
        {
            return View(db.cars.ToList());
        }

        public ActionResult UserIndex()
        {
            return View(db.cars.ToList());
        }

        // Get:cars list

        public JsonResult Getdatalist()
        {
            List<carangular> carlist = new List<carangular>();


            var databaselist = db.cars.ToList();
            foreach(var item in databaselist)
            {
                carangular care = new carangular();
                care.id = item.Id;
                care.carfactory = item.factory;
                care.carmodel = item.model;
                care.cartype = item.type;
                care.carcolor = item.color;
                care.carprice = item.price;
                carlist.Add(care);
            }
         
            return Json(carlist, JsonRequestBehavior.AllowGet);
        }

    

    // GET: Cars/Details/5

    public ActionResult Details(int? id)
        {
            if (id == null && Session["carid"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != null) { Car car = db.cars.Find(id); return View(car); }
        
           else
            {
        int carid = Convert.ToInt32(Session["carid"]);
        Car car = db.cars.Find(carid);
            return View(car);
    }
}
        public ActionResult Viewimage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }

         
            return File(car.Image, "image/jpg", string.Format("{0}.jpg", id));
        }

     
        public JsonResult addtocart(int? selectedcar)
        {
            
            if  (Session["user"] == null)
              {
               
                return Json(false, JsonRequestBehavior.AllowGet);
            }
           
           Car car = db.cars.Where(p =>p.Id== selectedcar).SingleOrDefault();
            string username = Convert.ToString(Session["user"]);

            ApplicationUser user = db.Users.Where(p => p.UserName== username).SingleOrDefault();
            Customer cus = db.customers.Where(p => p.email == user.Email).SingleOrDefault();

            shoppingcart cart = db.shoppingcart.Where(p => p.customer.Id == cus.Id).SingleOrDefault();

            cart.car.Add(car);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet); ;
        }

        public ActionResult addcart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.cars.Find(id);
            string username = Convert.ToString(Session["user"]);

            ApplicationUser user = db.Users.Where(p => p.UserName == username).SingleOrDefault();
            Customer cus = db.customers.Where(p => p.email == user.Email).SingleOrDefault();

            shoppingcart cart = db.shoppingcart.Where(p => p.customer.Id == cus.Id).SingleOrDefault();

            cart.car.Add(car);

            db.SaveChanges();
            return View("UserIndex");
        }


        // GET: Cars/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,factory,model,type,color,price,Image")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.cars.Add(car);
                db.SaveChanges();
                TempData["carid"] = car.Id;
                db.SaveChanges();

                //return RedirectToAction("UploadImages", "uploadimages", TempData["carid"]);
            }

            return View("Index",db.cars.ToList());
        }

        // GET: Cars/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,factory,model,type,color,price,Image")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        // GET: Cars/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.cars.Find(id);
            db.cars.Remove(car);
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
