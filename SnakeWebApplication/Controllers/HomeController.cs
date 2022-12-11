using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using Newtonsoft.Json;
using SnakeWebApplication.DataBase;
using SnakeWebApplication.Models;

namespace SnakeWebApplication.Controllers;

//[Route("api/[controller]/[action]")]
//[ApiController]
// 
public class HomeController : Controller
{
    private readonly DataBaseController _controller = new();
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    [Route("api/Index")]
    [HttpPost]
    public IActionResult Login(HomeModel model)
    {
        if (!_controller.LogInUser(model.Username, model.Password)) return View("Index");
        GeneralOptions.UserLoggedIn = true;
        RedirectToPage("localhost:5001/api/index");
        return View("~/Views/Snake/Index.cshtml");
    }
    [HttpPost]
    public IActionResult Register(HomeModel model)
    {
        RedirectToPage("localhost:5001/Register");
        return View("Register");
        if (!_controller.RegisterUser(model.Username, model.Password)) return View("Index");
        
    }

    //[HttpGet]
    //[ProducesResponseType(200)]
    public IActionResult Index()
    {
        GeneralOptions.UserLoggedIn = false;
        return View();
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
