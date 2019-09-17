using System;
using Microsoft.AspNetCore.Mvc;

[Route("home")]
public class HomeController : Controller
{
    [HttpGet("editor")]
    public ViewResult EditorPage()
    {
        return View();
    }        
}