using Microsoft.AspNetCore.Mvc;
using MultiTenancyDemo.Data;
using MultiTenancyDemo.Models;
using MultiTenancyDemo.Repository;
using System.Diagnostics;
using System.Linq;

namespace MultiTenancyDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMultiTenantRepositoryBase<Tenant> _tenantRepository;
        public HomeController(IMultiTenantRepositoryBase<Tenant> tenantRepository)
        {
            _tenantRepository=tenantRepository;
        }

        #region 租户相关

        public IActionResult Tenant()
        {
            return View("ManagerTenancy");
        }
        public IActionResult GetTenant()
        {
            var result=_tenantRepository.GetAll().ToList();

            return Json(new PageModel<Tenant>(){Total=result.Count,Rows=result});
        }

        public IActionResult CreateTenantView()
        {
            return View("CreateTenant");
        }
        public IActionResult CreateTenant(Tenant tenant)
        {
            _tenantRepository.Create(tenant);
            return Ok();
        }

        #endregion
        

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ManagerTenancy()
        {
            return View();
        }
        
        public IActionResult CreateTenancy()
        {
            return Ok();
        }
    }
}
