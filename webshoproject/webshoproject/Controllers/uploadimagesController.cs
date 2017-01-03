using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webshoproject.Models;

namespace webshoproject.Controllers
{



    public class uploadimagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult UploadImages()
        {
            return View();
        }

      
        public ActionResult saveimage (HttpPostedFileBase uploadImages , int? carid)
        {
            //Car car = new Car();
            Car car = db.cars.Find(carid);



            if (uploadImages.ContentLength > 0)
            {
                //byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImages.InputStream))
                {
                   car.Image = binaryReader.ReadBytes(uploadImages.ContentLength);
                }

                db.SaveChanges();

            }

            return RedirectToAction("Index", "Home"); 
        }
    }
}