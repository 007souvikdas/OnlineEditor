using System;
using Microsoft.AspNetCore.Mvc;
[Route("file")]
public class FileOpsHandlerController : Controller
{
    IFileStorage myFileStorage;
    string path;
    public FileOpsHandlerController(IFileStorage fileStorage)
    {
        myFileStorage = fileStorage;
      
    }

    [HttpPost("add")]
    public JsonResult AddNewFile(string name)
    {
        string IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
        path = Utilities.GetFolderName(IpAddress);
        var result = myFileStorage.AddFile(path,name);
        if (result)
        {
            return Json(new { code = 200, status = true });
        }
        else
        {
            return Json(new { code = 500, status = false, message = "Error occured while saving the file" });
        }
    }
    [HttpDelete("remove")]
    public JsonResult RemoveFile(string name)
    {
        string IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
        path = Utilities.GetFolderName(IpAddress);
        Console.WriteLine("File removed :" + name);
        var result = myFileStorage.RemoveFile(path,name);
        if (result)
        {
            return Json(new { code = 200, statusCode = true });
        }
        else
        {
            return Json(new { code = 500, statusCode = false, message = "Error occured while saving the file" });
        }
    }
    [HttpGet("listall")]
    public JsonResult GetFiles()
    {
        string IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
        path = Utilities.GetFolderName(IpAddress);
        var result = myFileStorage.GetFileNames(path);
        if (result.Length > 0)
        {
            return Json(new { code = 200, status = true });
        }
        else
        {
            return Json(new { code = 404, status = "false", message = "No File present in the location" });
        }
    }
    [HttpGet("list/{name}")]
    public JsonResult GetFileContents(string name)
    {
        string IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
        path = Utilities.GetFolderName(IpAddress);
        var result = myFileStorage.GetFileContents(path,name);
        if (result != null)
        {
            return Json(new { code = 200, status = true, result });
        }
        else
        {
            return Json(new { code = 404, status = "false", message = "No File present in the location" });
        }
    }
    [HttpPost("save")]
    public JsonResult SaveFile(string name, string sourceCode)
    {
        string IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
        path = Utilities.GetFolderName(IpAddress);
        var result = myFileStorage.SaveFile(path,name, sourceCode);
        if (result)
        {
            return Json(new { code = 200, status = true });
        }
        else
        {
            return Json(new { code = 404, status = "false", message = "No File present in the location" });
        }
    }

}
