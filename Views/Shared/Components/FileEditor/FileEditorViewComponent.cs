using System;
using Microsoft.AspNetCore.Mvc;
using OnlineEditor.Models;
public class FileEditorViewComponent : ViewComponent
{
    IFileStorage myFileStorage;
    public FileEditorViewComponent(IFileStorage fileStorage)
    {
        myFileStorage = fileStorage;
    }
    public IViewComponentResult Invoke(string fileName)
    {
        string IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
        string path = Utilities.GetFolderName(IpAddress);

        SourceCode sourceCode;
        if (fileName == null || fileName.Equals(string.Empty))
        {
            string[] strArray = myFileStorage.GetFileNames(path);
            string str = strArray.Length > 0 ? strArray[0] : string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                sourceCode = myFileStorage.GetFileContents(path, str);
            }
            else
            {
                sourceCode = new SourceCode();
            }
        }
        else
        {
            //open the file , read it and display
            sourceCode = myFileStorage.GetFileContents(path, fileName);
        }
        return View("FileEditorHandler", sourceCode);
    }

}