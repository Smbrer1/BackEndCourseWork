using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SnakeWebApplication.DataBase;
using SnakeWebApplication.Models;

namespace SnakeWebApplication.Controllers;

//[Route("api/[controller]/[action]")]
//[ApiController]
// 
public class RegisterController : Controller
{
    private readonly DataBaseController _controller = new();
    private readonly ILogger<HomeController> _logger;

    public RegisterController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [HttpPost]
    public IActionResult Register(HomeModel model)
    {
        if (!_controller.RegisterUser(model.Username, model.Password)) return View("~/Views/Home/Register.cshtml");
        RedirectToPage("localhost:5001/api/index");
        return View("~/Views/Home/Index.cshtml");
    }
}