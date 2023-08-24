using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using applicaton.Models;
using System.Numerics;
using System.Text.Encodings.Web;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Net.WebSockets;
using System.Text;

namespace applicaton.Controllers;

public class HomeController : Controller
{
    private DiffieHelmanModel dh = new();
    private AESModel aes = new();
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

    private (string, string) CalcModularExpansionWithRandomB(string id){
        BigInteger b = dh.Random256BitNumber();
        string serverExponent = b.ToString();
        TempData[id] = serverExponent;
        string calculation = dh.ModularExponentiation(dh.Generator, b).ToString();
        return (calculation, serverExponent);
    }

    private byte[] CheckCommonKey(BigInteger b){
        byte[] bytes = b.ToByteArray();
        if (bytes.Length > 256){
            byte[] correctedBytes = new byte[256];
            for (int i = 0; i<256; i++){
                correctedBytes[i] = bytes[i];
            }
            return correctedBytes;
        }
        return bytes;
    }
    public IActionResult CreateUser(string id, string username, string password, string clientKey){
        if (id == null){
            Guid newID = Guid.NewGuid();
            string newIDString = newID.ToString();
            var result = CalcModularExpansionWithRandomB(newIDString);
            return View((result.Item1, newIDString));
        }
        if (clientKey == null){
            if (TempData.ContainsKey(id)){
                return View((TempData[id], id));
            } else {
                return View(("", id));
            }
        }
        if (username == null | password == null){
            return Error();
        }
        BigInteger b = BigInteger.Parse(TempData[id].ToString());
        BigInteger clientKeyInt = BigInteger.Parse(clientKey);
        BigInteger commonKey = dh.ModularExponentiation(clientKeyInt, b); 
        byte[] commonKeyByte = CheckCommonKey(commonKey);
        Console.WriteLine(commonKey.ToString());
        Console.WriteLine("commonkey is " + string.Join(" ", commonKeyByte));
        byte[] iv = Encoding.UTF8.GetBytes("simonmoritzjense");
        Console.WriteLine(password);
        byte[] secret = aes.ConvertHexToByte(password);
        byte[] fakeKey = Encoding.UTF8.GetBytes("whatever");
        Console.WriteLine("cipher is " + string.Join(" ", secret));
        string decPassword = aes.DecryptStringFromBytes_Aes(secret, commonKeyByte, iv);
        Console.WriteLine(decPassword);
        return View("Login");
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
