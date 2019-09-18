using System;
using Microsoft.AspNetCore.Mvc;
public class HomeController : Controller
{
    [HttpGet("")]
    public ViewResult EditorPage()
    {
        return View();
    }        
}