using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cake_Hut.Models;
using System.Web.Security;

namespace Cake_Hut.Controllers
{
    public class LoginController : Controller
    {
        private Cake_HutEntities1 db = new Cake_HutEntities1();

        //
        // GET: /Login/


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
           
            Session.RemoveAll();
            return RedirectToAction("index", "Login");

            //Redirect("http://AnotherApplicaton/Home/LogOut");
        }

        [HttpPost]
        public ActionResult Login(Login l)
        {
            using (Cake_HutEntities1 db = new Cake_HutEntities1())
            {
                var user = db.Logins.Where(m => m.User_Name == l.User_Name && m.Password == l.Password).FirstOrDefault();

                if (user != null)
                {
                    var role = user.Role;
                    if (role == "admin")
                    {
                        Session["admin"] = 0;
                        Session["username"] = user.User_Name;
                        Session["userid"] = user.User_Id;
                        return RedirectToAction("AdashH", "AdminDashboard");
                    }
                    else if (role == "user")
                    {
                        Session["Staff"] = 1;
                        Session["username"] = user.User_Name;
                        Session["userid"] = user.User_Id;
                        return RedirectToAction("Home", "Login");
                    }
                    else if (role == "staff")
                    {
                        Session["Staff"] = 2;
                        Session["username"] = user.User_Name;
                        Session["userid"] = user.User_Id;
                        return RedirectToAction("Index", "BContact");
                    }
                   
                    else if (role == "staffd")
                    {
                    
                        
                        Session["Staff"] = 2;
                        Session["username"] = user.User_Name;
                        Session["userid"] = user.User_Id;
                        return RedirectToAction("Index", "contact");
                    }
                }
                if (user == null)
                {
                    l.LoginError = "INVALID USER";
                    return View("Index", l);
                }
                return View();
            }
        }
        // GET: /Company/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_Id,Name,Address,Mobile,Email,Wallet,Order_HistoryPhone,User_Name,Password,Role")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Registrations.Add(registration);
                db.SaveChanges();



                Login login = new Login();
                login.User_Name = registration.Name;
                login.Password = registration.Password;
                login.Role = registration.Role;

                

                db.Logins.Add(login);
                db.SaveChanges();
                login.LoginError = "USER SUCESSFULLY CREATED";
                

            }

            return View("Index");
        }
    }
}