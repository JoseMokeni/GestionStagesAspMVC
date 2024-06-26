﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestionStages.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionStages.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool isAdmin()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return false;
            }
            return true;
        }

        private bool isStudent(int id)
        {
            // check if the student is the same as the one in the session
            if (HttpContext.Session.GetString("Role") == "Student" && HttpContext.Session.GetString("Id") != id.ToString())
            {
                return false;
            }
            return true;
        }

        // GET: StudentsController
        public ActionResult Index()
        {
            // if not admin, redirect to home
            if (!isAdmin())
            {
                return RedirectToAction("Index", "Home");
            }
            // get all students
            var students = _context.Students.Include(s => s.Group).ToList();
            return View(students);
        }

        // GET: StudentsController/Details/5
        public ActionResult Details(int id)
        {
            // if not admin and not the student, redirect to home
            if (!isAdmin() && !isStudent(id))
            {
                return RedirectToAction("Index", "Home");
            }
            // get student by id
            var student = _context.Students.Include(s => s.Group).FirstOrDefault(s => s.Id == id);

            return View(student);
        }

        // GET: StudentsController/Create
        public ActionResult Create()
        {
            // if not admin, redirect to home
            if (!isAdmin())
            {
                return RedirectToAction("Index", "Home");
            }
            Student student = new Student();
            ViewBag.Groups = _context.Groups.OrderBy(g => g.Name).ToList();
            return View(student);
        }

        // POST: StudentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // create student
                Student student = new Student();
                student.Name = collection["Name"];
                student.Email = collection["Email"];
                student.GroupId = int.Parse(collection["GroupId"]);
                // check if CV is not empty, CV is a file
                if (collection.Files.Count > 0)
                {
                    // if wwwroot/Files does not exist, create it
                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files")))
                    {
                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files"));
                    }
                    var file = collection.Files[0];
                    // the name of the file is the id of the student + .pdf
                    student.CV = student.Id + ".pdf";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", student.CV);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                _context.Students.Add(student);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                // print error message
                Console.WriteLine(e.Message);
                return Create();
            }
        }

        // GET: StudentsController/Edit/5
        public ActionResult Edit(int id)
        {
            // if not admin and not the student, redirect to home
            if (!isAdmin() && !isStudent(id))
            {
                return RedirectToAction("Index", "Home");
            }
            // get student by id
            var student = _context.Students.Include(s => s.Group).FirstOrDefault(s => s.Id == id);
            ViewBag.Groups = _context.Groups.OrderBy(g => g.Name).ToList();
            ViewBag.Role = HttpContext.Session.GetString("Role");
            return View(student);
        }

        // POST: StudentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                Console.WriteLine("Edit");
                // get student by id
                var student = _context.Students.FirstOrDefault(s => s.Id == id);
                student.Name = collection["Name"];
                student.Email = collection["Email"];
                student.GroupId = int.Parse(collection["GroupId"]);
                // check if CV is not empty, CV is a file
                if (collection.Files.Count > 0)
                {
                    // if wwwroot/Files does not exist, create it
                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files")))
                    {
                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files"));
                    }
                    var file = collection.Files[0];
                    // the name of the file is the id of the student + .pdf
                    student.CV = student.Id + ".pdf";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", student.CV);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                _context.Students.Update(student);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentsController/Delete/5
        public ActionResult Delete(int id)
        {
            // if not admin, redirect to home
            if (!isAdmin())
            {
                return RedirectToAction("Index", "Home");
            }
            // get student by id
            var student = _context.Students.Include(s => s.Group).FirstOrDefault(s => s.Id == id);
            return View(student);
        }

        // POST: StudentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // get student by id
                var student = _context.Students.FirstOrDefault(s => s.Id == id);

                // check if the student has a CV
                if (student.CV != null)
                {
                    // delete the CV
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", student.CV);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                // remove student
                _context.Students.Remove(student);
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
