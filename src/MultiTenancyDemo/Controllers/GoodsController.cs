using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultiTenancyDemo.Data;
using MultiTenancyDemo.Repository;
using MultiTenancyDemo.Uow;

namespace MultiTenancyDemo.Controllers
{
    public class GoodsController : Controller
    {
        private readonly IMultiTenantRepositoryBase<Goods> _repository;
        private readonly IMultiTenancyDemoUnitOfWork _unitOfWork;

        public GoodsController(IMultiTenantRepositoryBase<Goods> repository
                              ,IMultiTenancyDemoUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork=unitOfWork;
        }

        // GET: Goods
        public async Task<IActionResult> Index()
        {
            var multiTenancyDbContext = _repository.GetAll().Include(g => g.User);
            return View(await multiTenancyDbContext.ToListAsync());
        }

        // GET: Goods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goods = await _repository.GetAll()
               
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goods == null)
            {
                return NotFound();
            }

            return View(goods);
        }

        // GET: Goods/Create
        public IActionResult Create()
        {
            //ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "Id");
            //ViewData["UserId"] = new SelectList(_context.User, "Id", "Id");
            return View();
        }

        // POST: Goods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Image,UserId,TenantId,Status")] Goods goods)
        {
            if (ModelState.IsValid)
            {
                goods.UserId=1;
               await _repository.CreateAsync(goods);
               await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "Id", goods.TenantId);
            //ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", goods.UserId);
            return View(goods);
        }

        // GET: Goods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goods = await _repository.GetAll().FirstAsync(r=>r.Id==id);
            if (goods == null)
            {
                return NotFound();
            }
            //ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "Id", goods.TenantId);
            //ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", goods.UserId);
            return View(goods);
        }

        // POST: Goods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Image,UserId,TenantId,Status")] Goods goods)
        {
            if (id != goods.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await _repository.UpdateAsync(goods);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsExists(goods.Id))
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
            //ViewData["TenantId"] = new SelectList(_context.Tenant, "Id", "Id", goods.TenantId);
            //ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", goods.UserId);
            return View(goods);
        }

        // GET: Goods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goods = await _repository.GetAll()
                
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goods == null)
            {
                return NotFound();
            }

            return View(goods);
        }

        // POST: Goods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goods = await _repository.GetAll().FirstAsync(r=>r.Id==id);
           
            return RedirectToAction(nameof(Index));
        }

        private bool GoodsExists(int id)
        {
            return _repository.GetAll().Any(e => e.Id == id);
        }
    }
}
