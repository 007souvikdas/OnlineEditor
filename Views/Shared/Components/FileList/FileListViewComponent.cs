using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public class FileListViewComponent : ViewComponent
{
    IFileStorage myFileStorage;
    public FileListViewComponent(IFileStorage fileStorage)
    {
        myFileStorage = fileStorage;
    }
    public IViewComponentResult Invoke()    
    {
        string IpAddress=HttpContext.Connection.RemoteIpAddress.ToString();
        string path=Utilities.GetFolderName(IpAddress);

        var fileNames = myFileStorage.GetFileNames(path);
        return View("FilesListhandler", fileNames);
    }
}
