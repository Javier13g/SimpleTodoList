using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleTodoList.Models;

namespace SimpleTodoList.Controllers
{
    public class ListController : Controller
    {
        private readonly TodoListContext Database = new TodoListContext();
        private readonly INotyfService _notyf;

        public ListController(INotyfService notyf)
        {
            _notyf = notyf;
        }

        // GET: List
        public ActionResult Index()
        {
            var todoListContext = Database.Lists.Include(l => l.Category);
            return View(todoListContext.ToList());
        }

        // GET: List/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = Database.Lists
                .Include(l => l.Category)
                .FirstOrDefault(m => m.IdList == id);
            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        // GET: List/Create
        public ActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(Database.Categories, "IdCategory", "CategoryName");
            return View();
        }

        // POST: List/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("IdList,CategoryId,TitleList,DescriptionList,Complete,CreateDate")] List list)
        {
            if (ModelState.IsValid)
            {
                list.IdList = Guid.NewGuid();
                Database.Add(list);
                Database.SaveChanges();
                _notyf.Success("Added List");
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(Database.Categories, "IdCategory", "CategoryName", list.CategoryId);
            return View(list);
        }

        // GET: List/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = Database.Lists.Find(id);
            if (list == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(Database.Categories, "IdCategory", "CategoryName", list.CategoryId);
            return View(list);
        }

        // POST: List/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("IdList,CategoryId,TitleList,DescriptionList,Complete,CreateDate")] List list)
        {
            if (id != list.IdList)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Database.Update(list);
                    Database.SaveChanges();
                    _notyf.Success("Edited List");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListExists(list.IdList))
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
            ViewData["CategoryId"] = new SelectList(Database.Categories, "IdCategory", "CategoryName", list.CategoryId);
            return View(list);
        }

        // GET: List/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = Database.Lists
                .Include(l => l.Category)
                .FirstOrDefault(m => m.IdList == id);
            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        // POST: List/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var list = Database.Lists.Find(id);
            Database.Lists.Remove(list);
            Database.SaveChanges();
            _notyf.Success("Deleted List");
            return RedirectToAction(nameof(Index));
        }

        private bool ListExists(Guid id)
        {
            return Database.Lists.Any(e => e.IdList == id);
        }
    }
}
