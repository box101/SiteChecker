
namespace SiteChecker.WebApplication.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class UrlCheckTasksController : Controller
    {
        private readonly SiteCheckerDbContext _context;

        public UrlCheckTasksController(SiteCheckerDbContext context)
        {
            _context = context;
        }

        // GET: UrlCheckTasks
        public async Task<IActionResult> Index()
        {
            return View(await _context.UrlCheckTasks.ToListAsync());
        }

        // GET: UrlCheckTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urlCheckTask = await _context.UrlCheckTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urlCheckTask == null)
            {
                return NotFound();
            }

            return View(urlCheckTask);
        }

        // GET: UrlCheckTasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UrlCheckTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Url,Login,Password")] UrlCheckTask urlCheckTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(urlCheckTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(urlCheckTask);
        }

        // GET: UrlCheckTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urlCheckTask = await _context.UrlCheckTasks.FindAsync(id);
            if (urlCheckTask == null)
            {
                return NotFound();
            }
            return View(urlCheckTask);
        }

        // POST: UrlCheckTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Url,Login,Password")] UrlCheckTask urlCheckTask)
        {
            if (id != urlCheckTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(urlCheckTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UrlCheckTaskExists(urlCheckTask.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(urlCheckTask);
        }

        // GET: UrlCheckTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urlCheckTask = await _context.UrlCheckTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urlCheckTask == null)
            {
                return NotFound();
            }

            return View(urlCheckTask);
        }

        // POST: UrlCheckTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var urlCheckTask = await _context.UrlCheckTasks.FindAsync(id);
            _context.UrlCheckTasks.Remove(urlCheckTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UrlCheckTaskExists(int id)
        {
            return _context.UrlCheckTasks.Any(e => e.Id == id);
        }
    }
}
