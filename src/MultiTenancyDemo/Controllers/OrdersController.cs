using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultiTenancyDemo.Data;
using System.Linq;
using System.Threading.Tasks;
using MultiTenancyDemo.Repository;

namespace MultiTenancyDemo.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IMultiTenantRepositoryBase<Order> _repository;
        private readonly IMultiTenantRepositoryBase<Tenant> _tenantRepository;
        private readonly IMultiTenantRepositoryBase<User> _userRepository;

        public OrdersController(IMultiTenantRepositoryBase<Order> repository,
               IMultiTenantRepositoryBase<Tenant> tenantRepository,
               IMultiTenantRepositoryBase<User> userRepository)
        {
            _tenantRepository = tenantRepository;
            _userRepository = userRepository;
            _repository = repository;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var multiTenancyDbContext = _repository.GetAll().Include(o => o.User);
            return View(await multiTenancyDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _repository.GetAll()
                
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["TenantId"] = new SelectList(_tenantRepository.GetAll(), "Id", "Id");
            ViewData["UserId"] = new SelectList(_userRepository.GetAll(), "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TenantId,OrderDes")] Order order)
        {
            if (ModelState.IsValid)
            {
                _repository.Create(order);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenantId"] = new SelectList(_tenantRepository.GetAll(), "Id", "Id");
            ViewData["UserId"] = new SelectList(_userRepository.GetAll(), "Id", "Id");
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _repository.GetAll().FirstAsync(r=>r.Id==id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["TenantId"] = new SelectList(_tenantRepository.GetAll(), "Id", "Id");
            ViewData["UserId"] = new SelectList(_userRepository.GetAll(), "Id", "Id");
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TenantId,OrderDes")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.UpdateAsync(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["TenantId"] = new SelectList(_tenantRepository.GetAll(), "Id", "Id", order.TenantId);
            ViewData["UserId"] = new SelectList(_userRepository.GetAll(), "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _repository.GetAll()
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _repository.GetAll().FirstAsync(r=>r.Id==id);
           
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _repository.GetAll().Any(e => e.Id == id);
        }
    }
}
