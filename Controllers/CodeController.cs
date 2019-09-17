using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
[Route("code")]
public class CodeController : Controller
{
    IFileStorage myFileStorage;
    ICompiler myCodeCompiler;
    string path;
    public CodeController(IFileStorage fileStorage, ICompiler codeCompiler)
    {
        myFileStorage = fileStorage;
        myCodeCompiler = codeCompiler;

    }
    [HttpPost("execute")]
    public JsonResult CompileAndExecute()
    {
        string IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
        path = Utilities.GetFolderName(IpAddress);

        List<string> list = new List<string>();
        foreach (var fileName in myFileStorage.GetFileNames(path))
        {
            list.Add(myFileStorage.GetFileContents(path, fileName).sourceContent);
        }
        (bool, string) res = myCodeCompiler.CompileFiles(list.ToArray(), path);
        if (res.Item1)
        {
            //make the execute request
            var executionResult = myCodeCompiler.ExecuteFiles(res.Item2, path);
            if (!executionResult.Item1)
            {
                return Json(new { code = 500, status = executionResult.Item1, message = executionResult.Item2 });
            }
            else
            {
                return Json(new { code = 200, status = executionResult.Item1, message = executionResult.Item2 });
            }
        }
        else
        {
            return Json(new { code = 500, status = res.Item1, message = res.Item2 });
        }
    }
}