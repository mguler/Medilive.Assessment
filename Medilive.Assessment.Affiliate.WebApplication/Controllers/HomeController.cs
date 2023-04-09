using Medilive.Assessment.Affiliate.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Medilive.Assessment.Affiliate.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
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

        [HttpGet]
        public IActionResult Redirect(string message,string url)
        {
            ViewBag.Message = message;
            Response.Headers.Add("refresh", $"5;url={url}");
            return View();
        }
    }
}