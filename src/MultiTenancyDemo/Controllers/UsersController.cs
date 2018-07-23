using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenancyDemo.Data;
using MultiTenancyDemo.Repository;
using System.Linq;
using System.Threading.Tasks;
using MultiTenancyDemo.Uow;

namespace MultiTenancyDemo.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMultiTenantRepositoryBase<User> _repository;
        private readonly IMultiTenancyDemoUnitOfWork _unitOfWork;

        public UsersController(IMultiTenantRepositoryBase<User> repository,
                              IMultiTenancyDemoUnitOfWork unitOfWork)
        {
            _repository = repository;
            this._unitOfWork=unitOfWork;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var multiTenancyDbContext = _repository.GetAll();//.User.Include(u => u.Tenant);
            return View(await multiTenancyDbContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _repository.GetAll()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            //ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "Id");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,TenantId")] User user)
        {
            if (ModelState.IsValid)
            {
                _repository.Create(user);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "Id", user.TenantId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _repository.GetAll().FirstAsync(r=>r.Id==id);
            if (user == null)
            {
                return NotFound();
            }
            //ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "Id", user.TenantId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,TenantId")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await _repository.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            //ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "Id", user.TenantId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _repository.GetAll()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _repository.GetAll().FirstAsync(r=>r.Id==id);
            
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _repository.GetAll().Any(e => e.Id == id);
        }
    }
}
