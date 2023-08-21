using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using applicaton.Models;
using System.Numerics;
using System.Text.Encodings.Web;
using System.Runtime.CompilerServices;
using System.Xml;

namespace applicaton.Controllers;

public class HomeController : Controller
{
    private DiffieHelmanModel dh = new();
    private Dictionary<string, BigInteger> serverExponents ;
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

    public IActionResult CreateUser(string id, string username, string password, string clientKey){
        if (id == null){
            Guid newID = Guid.NewGuid();
            BigInteger b = dh.Random256BitNumber();
            string serverExponent = b.ToString();
            string newIDString = newID.ToString();
            TempData[newIDString] = serverExponent;
            string calculation = dh.ModularExponentiation(dh.Generator, b).ToString();
            Console.WriteLine(serverExponent);
            return View((calculation, newIDString));
        }
        if (clientKey == null){
            string serverExponent;
            if (TempData.ContainsKey(id)){
                serverExponent = TempData[id].ToString();
            } else {
                serverExponent = "";
            }
            return View((serverExponent, id));
        }
        if (username == null | password == null){
            return Error();
        }
        Console.WriteLine(TempData[id]);
        return View("Hello");
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
