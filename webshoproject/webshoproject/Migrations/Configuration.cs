namespace webshoproject.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<webshoproject.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(webshoproject.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.customers.AddOrUpdate(
           p => p.firstname,
           new Customer { firstname = "default", lastname = "default", phone = "default", email = "default", billingadress = "default", billingcity = "default", deliveryadress = "default", deliverycity = "default", }

         );

            context.cars.AddOrUpdate(
            p => p.model,
            new Car { model = "default", factory = "default", color = "default", price = 000, type = "default", }
             );
            var store = new RoleStore<IdentityRole>(context);
            var userstor = new UserStore<ApplicationUser>(context);
            var roleManager = new RoleManager<IdentityRole>(store);
            var UserManager = new UserManager<ApplicationUser>(userstor);


            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("User"));


            ApplicationUser hussine = new ApplicationUser();

            hussine.Email = "a@a.a";
            hussine.UserName = "hussein";
            shoppingcart cart = new shoppingcart();
            Customer customer = new Customer();
            customer.email = "a@a.a";
            cart.customer = customer;
            context.shoppingcart.Add(cart);
            var result = UserManager.Create(hussine, "Password!123");
            context.SaveChanges();

            string useridentity = context.Users.Single(p => p.UserName == "hussein").Id;
            UserManager.AddToRole(useridentity, "Admin");
        }
    }
}

