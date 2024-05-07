using GestionStages.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionStages.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: CompaniesController
        public ActionResult Index()
        {
            // get all companies
            List<Company> companies = _context.Companies.ToList();
            return View(companies);
        }

        // GET: CompaniesController/Details/5
        public ActionResult Details(int id)
        {
            // get company by id
            Company? company = _context.Companies.SingleOrDefault(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);

        }

        // GET: CompaniesController/Create
        public ActionResult Create()
        {
            Company company = new Company();
            return View(company);

        }

        // POST: CompaniesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Company company = new Company();
                company.Name = collection["Name"];
                company.Email = collection["Email"];
                _context.Companies.Add(company);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompaniesController/Edit/5
        public ActionResult Edit(int id)
        {
            // get company by id
            Company? company = _context.Companies.SingleOrDefault(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: CompaniesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Company? company = _context.Companies.SingleOrDefault(c => c.Id == id);
                if (company == null)
                {
                    return NotFound();
                }
                company.Name = collection["Name"];
                company.Email = collection["Email"];
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompaniesController/Delete/5
        public ActionResult Delete(int id)
        {
            // get company by id
            Company? company = _context.Companies.SingleOrDefault(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: CompaniesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Company? company = _context.Companies.SingleOrDefault(c => c.Id == id);
                if (company == null)
                {
                    return NotFound();
                }
                _context.Companies.Remove(company);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
