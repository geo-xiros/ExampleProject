using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private ExampleDb db = new ExampleDb();

        public ActionResult Index()
        {
            List<Employee> employees = db.Employees();
            ViewBag.Title = "Details";

            return View(employees);
        }

        public ActionResult CreateDb()
        {
            db.Create();

            return RedirectToIndex();

        }



        public ActionResult Details(string name)
        {
            var employee = db.Employees().Where(e => e.Name.Contains(name));
            return View(employee.FirstOrDefault());
        }


        public ActionResult Create(string name, string location)
        {
            // TODO: Add insert logic here
            Employee employee = new Employee()
            {
                Name = name,
                Location = location
            };

            db.Insert(employee);

            return RedirectToIndex();
        }

        public ActionResult Edit(string name)
        {

            db.Update(name);

            return RedirectToIndex();
        }

        public ActionResult Delete(string name)
        {
            db.Delete(name);

            return RedirectToIndex();
        }

        private ActionResult RedirectToIndex()
        {
            if (db.DbError == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = db.DbError;
            return View("Error");
        }
    }
}