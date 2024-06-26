﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestionStages.Models;

namespace GestionStages.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupsController(ApplicationDbContext context)
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

        // GET: GroupsController
        public ActionResult Index()
        {
            // if not admin, redirect to home
            if (!isAdmin())
            {
                return RedirectToAction("Index", "Home");
            }
            // get all groups
            var groups = _context.Groups.ToList();
            return View(groups);
        }

        // GET: GroupsController/Details/5
        public ActionResult Details(int id)
        {
            // if not admin, redirect to home
            if (!isAdmin())
            {
                return RedirectToAction("Index", "Home");
            }
            // get group by id
            var group = _context.Groups.FirstOrDefault(g => g.Id == id);
            return View(group);
        }

        // GET: GroupsController/Create
        public ActionResult Create()
        {
            // if not admin, redirect to home
            if (!isAdmin())
            {
                return RedirectToAction("Index", "Home");
            }
            Group group = new Group();
            return View(group);
        }

        // POST: GroupsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // create group
                Group group = new Group();
                group.Name = collection["Name"];
                _context.Groups.Add(group);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GroupsController/Edit/5
        public ActionResult Edit(int id)
        {
            // if not admin, redirect to home
            if (!isAdmin())
            {
                return RedirectToAction("Index", "Home");
            }
            // get group by id
            var group = _context.Groups.FirstOrDefault(g => g.Id == id);
            return View(group);
        }

        // POST: GroupsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // update group
                var group = _context.Groups.FirstOrDefault(g => g.Id == id);
                group.Name = collection["Name"];
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GroupsController/Delete/5
        public ActionResult Delete(int id)
        {
            // if not admin, redirect to home
            if (!isAdmin())
            {
                return RedirectToAction("Index", "Home");
            }
            // get group by id
            var group = _context.Groups.FirstOrDefault(g => g.Id == id);
            return View(group);
        }

        // POST: GroupsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // delete group
                var group = _context.Groups.FirstOrDefault(g => g.Id == id);
                _context.Groups.Remove(group);
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
