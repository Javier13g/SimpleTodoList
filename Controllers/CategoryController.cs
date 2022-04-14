using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleTodoList.Models;

namespace SimpleTodoList
{
    public class CategoryController : Controller
    {
        private readonly TodoListContext Database = new TodoListContext();
        private readonly INotyfService _notyf;

        public CategoryController(INotyfService notyf)
        {
            _notyf = notyf;
        }

        // GET: Category
        public ActionResult Index()
        {
            return View(Database.Categories.ToList());
        }

        // GET: Category/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = Database.Categories
                .FirstOrDefault(m => m.IdCategory == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("IdCategory,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.IdCategory = Guid.NewGuid();
                Database.Add(category);
                _notyf.Success("Added Category");
                Database.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = Database.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }


            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("IdCategory,CategoryName")] Category category)
        {
            if (id != category.IdCategory)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Database.Update(category);
                    _notyf.Success("Edited Category");
                    Database.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.IdCategory))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = Database.Categories
                .FirstOrDefault(m => m.IdCategory == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var category = Database.Categories.Find(id);
            Database.Categories.Remove(category);
            _notyf.Success("Deleted Category");
            Database.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
            return Database.Categories.Any(e => e.IdCategory == id);
        }
    }
}