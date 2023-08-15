using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using applicaton.Models;
using System.Text.Encodings.Web;
using System.Runtime.CompilerServices;

namespace applicaton.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Hello(string name, int num){
        return View((name, num));
    }


    public IActionResult Login(string name, int num = 1){
        ViewData["name1"] = name;
        ViewData["num"] = num;
        (string, int) a = (name, num);
        Console.WriteLine(a.GetType());
        return View(a);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
