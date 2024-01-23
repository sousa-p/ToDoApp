using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Todo
        [Authorize]
        public async Task<IActionResult> Index()
        {   
            var todos = await _context.TodoModel
                    .Where(t => t.User == User.Identity.Name)
                    .ToListAsync();
            var done = todos.Where(todo => todo.Done);
            var notDone = todos.Where(todo => !todo.Done);
            return View(new TodoIndexViewModel(notDone, done));
        }

        // GET: Todo/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoModel = await _context.TodoModel
                .FirstOrDefaultAsync(m => m.Id == id);
            bool userValidation = User.Identity != null && User.Identity.IsAuthenticated && todoModel.User == User.Identity.Name;
            
            if (todoModel == null || !userValidation)
            {
                return NotFound();
            }

            return View(todoModel);
        }

        // GET: Todo/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todo/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,DateToDone,User")] TodoModel todoModel)
        {
            if (ModelState.IsValid)
            {
                todoModel.User = User.Identity.Name;
                _context.Add(todoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todoModel);
        }

        // GET: Todo/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoModel = await _context.TodoModel.FindAsync(id);
            if (todoModel == null)
            {
                return NotFound();
            }
            return View(todoModel);
        }

        // POST: Todo/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DateToDone,Done")] TodoModel todoModel)
        {
            if (id != todoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTodo = await _context.TodoModel.FindAsync(todoModel.Id);
                    bool userValidation = User.Identity != null && User.Identity.IsAuthenticated && existingTodo.User == User.Identity.Name;

                    if (userValidation) {
                        existingTodo.Title = todoModel.Title;
                        existingTodo.Done = todoModel.Done;
                        existingTodo.Description = todoModel.Description;
                        existingTodo.DateToDone = todoModel.DateToDone;

                        _context.Update(existingTodo);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoModelExists(todoModel.Id))
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
            return View(todoModel);
        }

        // GET: Todo/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoModel = await _context.TodoModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoModel == null)
            {
                return NotFound();
            }

            return View(todoModel);
        }

        // POST: Todo/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoModel = await _context.TodoModel.FindAsync(id);
            bool userValidation = User.Identity != null && User.Identity.IsAuthenticated && todoModel.User == User.Identity.Name;


            if (todoModel != null && userValidation)
            {
                _context.TodoModel.Remove(todoModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoModelExists(int id)
        {
            return _context.TodoModel.Any(e => e.Id == id);
        }
    }
}
