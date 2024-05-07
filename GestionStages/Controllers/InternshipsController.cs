using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestionStages.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace GestionStages.Controllers
{
    
    public class InternshipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InternshipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InternshipsController
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string role = HttpContext.Session.GetString("Role");
            if (role.Equals("Admin"))
            {
                // get all internships
                var internships = _context.Internships.Include(i => i.Company).Include(i => i.Student).ToList();
                return View(internships);
            }

            if (role.Equals("Student"))
            {
                if (HttpContext.Session.GetString("Id") == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                int studentId = int.Parse(HttpContext.Session.GetString("Id"));

                // get internships by student id
                var internships = _context.Internships.Include(i => i.Company).Include(i => i.Student).Where(i => i.StudentId == studentId).ToList();
                return View(internships);
            }

            if (role.Equals("Company"))
            {
                if (HttpContext.Session.GetString("Id") == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                int companyId = int.Parse(HttpContext.Session.GetString("Id"));

                // get internships by company id
                var internships = _context.Internships.Include(i => i.Company).Include(i => i.Student).Where(i => i.CompanyId == companyId).ToList();
                return View(internships);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: InternshipsController/Details/5
        public ActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("Role") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // get internship by id
            var internship = _context.Internships.Include(i => i.Company).Include(i => i.Student).FirstOrDefault(i => i.Id == id);
            return View(internship);
        }

        // GET: InternshipsController/Create
        public ActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string role = HttpContext.Session.GetString("Role");
            if (role.Equals("Admin"))
            {
                ViewBag.Companies = _context.Companies.OrderBy(c => c.Name).ToList();
                ViewBag.Students = _context.Students.OrderBy(s => s.Name).ToList();
                return View();
            }

            if (role.Equals("Student"))
            {
                if (HttpContext.Session.GetString("Id") == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                int studentId = int.Parse(HttpContext.Session.GetString("Id"));

                Student student = _context.Students.FirstOrDefault(s => s.Id == studentId);

                return RedirectToAction("Ask", student);

            }

            return RedirectToAction("Index", "Home");
        }

        // POST: InternshipsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // create internship
                Internship internship = new Internship();
                internship.Title = collection["Title"];
                internship.Description = collection["Description"];
                internship.CompanyId = int.Parse(collection["CompanyId"]);
                internship.StudentId = int.Parse(collection["StudentId"]);
                internship.StartDate = DateTime.Parse(collection["StartDate"]);
                internship.EndDate = DateTime.Parse(collection["EndDate"]);
                internship.Status = collection["Status"];

                _context.Internships.Add(internship);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Ask(Student student)
        {
            if (HttpContext.Session.GetString("Role") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (HttpContext.Session.GetString("Role") != "Student")
            {
                return RedirectToAction("Index", "Home");
            }

            // get all companies
            ViewBag.Companies = _context.Companies.OrderBy(c => c.Name).ToList();
            ViewBag.Student = student;

            // create internship
            Internship internship = new Internship();

            return View(internship);
        }


        // GET: InternshipsController/Edit/5
        public ActionResult Edit(int id)
        {
            // check if it's an admin or if the internship belongs to the company

            if (HttpContext.Session.GetString("Role") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string role = HttpContext.Session.GetString("Role");
            ViewBag.Role = role;

            if (role.Equals("Admin"))
            {
                // get internship by id
                var internship = _context.Internships.Include(i => i.Company).Include(i => i.Student).FirstOrDefault(i => i.Id == id);
                ViewBag.Companies = _context.Companies.OrderBy(c => c.Name).ToList();
                ViewBag.Students = _context.Students.OrderBy(s => s.Name).ToList();
                return View(internship);
            }

            if (role.Equals("Company"))
            {
                if (HttpContext.Session.GetString("Id") == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                int companyId = int.Parse(HttpContext.Session.GetString("Id"));

                // get internship by id
                var internship = _context.Internships.Include(i => i.Company).Include(i => i.Student).FirstOrDefault(i => i.Id == id);

                // get company

                Company company = _context.Companies.FirstOrDefault(c => c.Id == companyId);

                ViewBag.Company = company;

                // get student
                Student student = _context.Students.FirstOrDefault(s => s.Id == internship.StudentId);
                ViewBag.Student = student;

                if (internship.CompanyId == company.Id)
                {
                    return View(internship);
                }
            }

            return RedirectToAction("Index");
        }

        // POST: InternshipsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // edit internship
                var internship = _context.Internships.FirstOrDefault(i => i.Id == id);
                internship.Title = collection["Title"];
                internship.Description = collection["Description"];
                internship.CompanyId = int.Parse(collection["CompanyId"]);
                internship.StudentId = int.Parse(collection["StudentId"]);
                internship.StartDate = DateTime.Parse(collection["StartDate"]);
                internship.EndDate = DateTime.Parse(collection["EndDate"]);
                internship.Status = collection["Status"];

                _context.Internships.Update(internship);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(id);
            }
        }

        // GET: InternshipsController/Delete/5
        public ActionResult Delete(int id)
        {
            // check if it's an admin or if the internship belongs to the user
            if (HttpContext.Session.GetString("Role") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string role = HttpContext.Session.GetString("Role");

            if (role.Equals("Admin"))
            {
                // get internship by id
                var internship = _context.Internships.Include(i => i.Company).Include(i => i.Student).FirstOrDefault(i => i.Id == id);
                return View(internship);
            }

            if (role.Equals("Student"))
            {
                if (HttpContext.Session.GetString("Id") == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                int studentId = int.Parse(HttpContext.Session.GetString("Id"));

                // get internship by id
                var internship = _context.Internships.Include(i => i.Company).Include(i => i.Student).FirstOrDefault(i => i.Id == id);

                if (internship.StudentId == studentId)
                {
                    return View(internship);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        // POST: InternshipsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // delete internship
                var internship = _context.Internships.FirstOrDefault(i => i.Id == id);
                _context.Internships.Remove(internship);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(id);
            }
        }
    }
}
