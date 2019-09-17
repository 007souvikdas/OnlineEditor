using System;
using Microsoft.AspNetCore.Mvc;
[Route("components")]
public class ComponentsController : Controller
{
    IFileStorage myFileStorage;
    public ComponentsController(IFileStorage fileStorage)
    {
        myFileStorage = fileStorage;
    }
    [HttpGet("sidebar")]
    public ViewComponentResult LoadSideBar()
    {
        return ViewComponent("FileList");
    }
    [HttpGet("Editor")]
    public ViewComponentResult LoadEditor(string fileName)
    {
        return ViewComponent("FileEditor",fileName);
    }
    [HttpGet("Result")]
    public ViewComponentResult ResultHandler(string result)
    {
        return ViewComponent("ResultHandler",result);
    }
}