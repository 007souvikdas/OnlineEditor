using System;
using Microsoft.AspNetCore.Mvc;

public class ResultHandlerViewComponent : ViewComponent
{
    public ResultHandlerViewComponent()
    {

    }
    public IViewComponentResult Invoke(string result)
    {
        return View("ResultHandler", result);
    }
}