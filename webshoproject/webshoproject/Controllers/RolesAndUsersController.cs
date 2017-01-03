using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using webshoproject.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using System.Collections.Generic;

namespace WebprojectIdentity.Controllers
{



    public class RolesAndUsersController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

         [Authorize (Roles ="Admin")]
        public ActionResult Index()
        {
            var UsersList = db.Users.ToList();
            return View(UsersList);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selscteduser = db.Users.Where(p => p.Id == id).SingleOrDefault();
            if (User == null)
            {
                return HttpNotFound();
            }

             
            ViewBag.user = selscteduser;
            var rolelist = selscteduser.Roles;
            List<string> useroles = new List<string>();
            foreach (var item in rolelist)
            {
                string role = (db.Roles.Where(p => p.Id == item.RoleId).SingleOrDefault()).Name;

                useroles.Add(role);
            }

            return View(useroles);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var selecteduser = db.Users.Where(p => p.Id == id).SingleOrDefault();
            if (selecteduser == null)
            {
                return HttpNotFound();
            }

            IQueryable c = db.Roles;
            ViewBag.roleslist = c;
            return View(selecteduser);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string username, string email, string role1, string role2, string role3)
        {
            var store = new RoleStore<IdentityRole>(db);
            var userstor = new UserStore<ApplicationUser>(db);
            var roleManager = new RoleManager<IdentityRole>(store);
            var UserManager = new UserManager<ApplicationUser>(userstor);
            var user = db.Users.Where(c => c.Id == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                user.UserName = username;
                user.Email = email;
                if ((role1 != null) ||   (role3 != null)) { user.Roles.Clear(); }
                db.SaveChanges();
                if (role1 != null) { UserManager.AddToRole(id, "Admin"); }
         
                if (role3 != null) { UserManager.AddToRole(id, "User"); }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = db.Users.Find(id);
           var customer = db.customers.Find(user.Email);
            db.customers.Remove(customer);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}